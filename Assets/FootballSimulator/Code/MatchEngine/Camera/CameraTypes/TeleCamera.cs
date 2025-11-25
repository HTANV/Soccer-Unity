using System;
using UnityEngine;

namespace FStudio.MatchEngine.Cameras {
    [Serializable]
    public class TeleCamera : MatchCamera {
        [Header("Position Offsets")]
        public float ZPositionOffset = -30;
        public float YPositionOffset = 20;
        public float Zoom = 30;

        public float DefaultCameraXRotation = 15;
        [Range(0f, 1f)]
        public float CameraXRotationTargetPower = 0.5f;

        public override (Vector3, Quaternion, float) Behave(in float deltaTime, Vector3 targetPosition) {
            return CalculateCameraTransform(targetPosition);
        }

        private (Vector3, Quaternion, float) CalculateCameraTransform(Vector3 targetPosition) {
            var cameraXPosition = targetPosition.x;

            var newCameraPosition = new Vector3(cameraXPosition, YPositionOffset, ZPositionOffset);

            var targetRotation = Quaternion.LookRotation(targetPosition - newCameraPosition).eulerAngles;

            var newCameraRotation = Quaternion.Euler(targetRotation.x, 0, 0);

            var result = Quaternion.Slerp(Quaternion.Euler(DefaultCameraXRotation, 0, 0), newCameraRotation, CameraXRotationTargetPower);

            return (newCameraPosition, result, Zoom);
        }
    }
}
