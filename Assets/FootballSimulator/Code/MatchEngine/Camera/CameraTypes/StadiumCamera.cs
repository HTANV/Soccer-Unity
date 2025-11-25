
using System;
using UnityEngine;

namespace FStudio.MatchEngine.Cameras {
    [Serializable]
    public class StadiumCamera : MatchCamera {
        [Header ("Position Offsets")]
        public float ZPositionOffset = -30;
        public float YPositionOffset = 20;

        [Header("XRotation Clamper (Vertical Look)")]
        public float MinXRotation = 22.5f;
        public float MaxXRotation = 62.5f;
        public AnimationCurve XRotationCurve;

        [Header("YRotation Clamper (Horizontal Look)")]
        public float MinYRotation = 22.5f;
        public float MaxYRotation = 62.5f;
        public AnimationCurve YRotationCurve;

        [Header("XPosition Clamper (Horizontal Movement)")]
        public float MinXPosition = 22.5f;
        public float MaxXPosition = 62.5f;
        public AnimationCurve XPositionCurve;

        [Header("ZPosition Zoom Clamper (How much zoom for a far target")]
        public float MinZPositionForZoom = 0f;
        public float MaxZPositionForZoom = 30f;
        public AnimationCurve ZZoomCurve;
        public float ZZoomMultiplier = 10f;

        [Header("YPosition Add by X")]
        public float MaxZPositionForPositionOffset = 30f;
        public AnimationCurve PositionOffsetCurve;
        public Vector3 PositionOffsetMultiplier;

        [Header("XPosition Zoom Curver (not a clamper)")]
        public float ZoomMultiplier = 10f;
        public AnimationCurve ZoomCurve;

        public override (Vector3, Quaternion, float) Behave (in float deltaTime, Vector3 targetPosition) {
            return CalculateCameraTransform(targetPosition);
        }

        private (Vector3, Quaternion, float) CalculateCameraTransform (Vector3 targetPosition) {
            var (cameraXPosition, percentage) = ClampWithCurve(
                targetPosition.x,
                MinXPosition,
                MaxXPosition,
                XPositionCurve);

            var zoom = ZoomCurve.Evaluate(percentage)* ZoomMultiplier;

            var zZoomer = ClampWithCurve(targetPosition.z, MinZPositionForZoom, MaxZPositionForZoom, ZZoomCurve);

            zoom /= ZZoomMultiplier * zZoomer.Item1 + 1;

            var zPerc = targetPosition.z / MaxZPositionForPositionOffset;

            var yAdd = PositionOffsetCurve.Evaluate(zPerc) * PositionOffsetMultiplier;

            var newCameraPosition = new Vector3(cameraXPosition, YPositionOffset, ZPositionOffset) + yAdd;

            var targetRotation = Quaternion.LookRotation(targetPosition - newCameraPosition).eulerAngles;

            var (cameraXRotation,_) = ClampWithCurve(NormalizeAngle (targetRotation.x),
                MinXRotation,
                MaxXRotation,
                XRotationCurve);

            float normalizedYRot = NormalizeAngle(targetRotation.y);

            var (cameraYRotation, _) = ClampWithCurve(normalizedYRot,
                MinYRotation,
                MaxYRotation,
                YRotationCurve);

            var newCameraRotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0);

            return (newCameraPosition, newCameraRotation, zoom);
        }
                                                    //50    //20        /50     
        private (float, float) ClampWithCurve(float value, float min, float max, AnimationCurve curve) {
            var clamped = Mathf.Clamp(value, min, max);
            var size = max - min; // 30 
            var percentage = (clamped - min) / size; //  50-20 / 30
            var reaction = curve.Evaluate(percentage);

            return (Mathf.Lerp(min + size / 2, clamped, reaction), percentage);
        }

        private float NormalizeAngle (float angle) {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }
    }
}
