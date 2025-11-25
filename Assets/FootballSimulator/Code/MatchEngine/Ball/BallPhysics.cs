using FStudio.MatchEngine.Players;
using UnityEngine;
using FStudio.MatchEngine.EngineOptions;

namespace FStudio.MatchEngine.Balls {
    public partial class Ball {
        private static EngineOptions_BallDropPrediction predictionSettings;

        private Vector3 calcBallisticVelocityVector(Vector3 source, Vector3 target, float angle) {
            Vector3 direction = target - source;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;
            float a = angle * Mathf.Deg2Rad;

            direction.y = distance * Mathf.Tan(a);

            distance += 0.5f * h / Mathf.Tan(a);

            float velocity = distance * Physics.gravity.magnitude / Mathf.Sin(2 * a);

            velocity = Mathf.Sqrt(velocity);

            return velocity * direction.normalized;
        }

        private Vector3 CrossModePredicter (PlayerBase requester) {
            var crossSettings = EngineOptions_BallDropPredictionForCrosses.Current;

            var crossTarget = CrossTarget;

            var toRequester = requester.Position - crossTarget;

            var distance = toRequester.magnitude;

            var normalizedVel = rigidbody.velocity.normalized;

            var angle = Mathf.Abs(Vector3.SignedAngle(normalizedVel, toRequester, Vector3.up));
            angle *= Mathf.Pow (distance, crossSettings.DistToAngle_Pow);

            // apply pow to angle.
            angle = Mathf.Pow(angle, crossSettings.Angle_Pow);
            // apply mod to angle.
            angle *= crossSettings.Angle_Mod;

            var playerVelocityDiv = crossSettings.PlayerVelocityDivider / (requester.Velocity.magnitude + 1);
            var playerVelocityMul = playerVelocityDiv * crossSettings.PlayerVelocityMul * angle;

            float requestersHeight = requester.PlayerController.UnityCollider.height;
            float reduceMod = requestersHeight * crossSettings.Velocity_Height_Mod_As_Minus;
             
            return crossTarget + 
            angle * normalizedVel 
            - reduceMod * normalizedVel + 
            Mathf.Pow (crossHeight, crossSettings.CrossHeight_Pow) * crossSettings.CrossHeight_Mod *
            normalizedVel + 
            Mathf.Pow (distance, crossSettings.DistanceToBallPow) * crossSettings.DistanceToBallMod * 
            normalizedVel +
            playerVelocityMul * normalizedVel;
        }

        private Vector3 Predicter (PlayerBase requester, float relaxation) {
            if (IsOnCrossMode) {
                return CrossModePredicter(requester);
            }

            if (predictionSettings == null) {
                predictionSettings = EngineOptions_BallDropPrediction.Current;
            }

            var ballPosition = transform.position;
            ballPosition.y = 0;

            var toRequester = requester.Position - ballPosition;
            
            relaxation = predictionSettings.RelaxationToVelocityCurve.Evaluate(relaxation);

            var velocity = rigidbody.velocity * relaxation;

            var absY = Mathf.Abs(velocity.y);

            var distance = toRequester.magnitude;

            var angle = Mathf.Abs (Vector3.SignedAngle(velocity, toRequester, Vector3.up));
            angle = velocity.magnitude * angle * distance;

            // apply pow to angle.
            angle = Mathf.Pow(angle, predictionSettings.BallPrediction_Angle_Pow);
            // apply mod to angle.
            angle *= predictionSettings.BallPrediction_Angle_Mod;
            angle *= predictionSettings.BallPrediction_Angle_Effect_DistanceCurve.Evaluate(distance);

            // apply pow to velo.
            var velPow = Mathf.Pow(velocity.magnitude, predictionSettings.BallPrediction_Velocity_Pow) + 
                distance * predictionSettings.BallPrediction_Velocity_Per_Distance_Mod * velocity.magnitude;

            // apply mod to velo.
            velPow *= predictionSettings.BallPrediction_Velocity_Mod;

            // complete.
            velocity = new Vector3 (velocity.x, 0, velocity.z).normalized * 
                velPow * 
                angle;

            float requestersHeight = requester.PlayerController.UnityCollider.height;
            float reduceMod = requestersHeight * predictionSettings.BallPrediction_Velocity_Height_Mod_As_Minus;

            // add height mod.
            velocity += velocity.normalized * 
                Mathf.Pow (absY * velocity.magnitude, predictionSettings.BallPrediction_Velocity_YPow) * 
                Mathf.Max (0, (predictionSettings.BallPrediction_Velocity_YMod - reduceMod));

            return ballPosition + velocity;
        }
    }
}
