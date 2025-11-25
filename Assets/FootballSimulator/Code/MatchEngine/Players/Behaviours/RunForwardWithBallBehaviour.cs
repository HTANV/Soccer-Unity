
using FStudio.MatchEngine.Enums;
using System.Linq;
using UnityEngine;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class RunForwardWithBallBehaviour : BaseBehaviour {
        private const float BEWARE_SUPER_CAREFUL = 2.3f;
        private const float BEWARE_CAREFUL = 1.8f;
        private const float BEWARE_NORMAL = 1.3f;
        private const float BEWARE_RISKY = 0.75f;

        private readonly AnimationCurve carefulbyBallprogress = new AnimationCurve (new Keyframe[] {
            new Keyframe (0, 1f),
            new Keyframe (0.25f, 0.9f),
            new Keyframe (0.5f, 0.6f),
            new Keyframe (0.75f, 0.3f),
            new Keyframe (0.9f, 0.2f),
            new Keyframe (1, 0.1f)
        });

        private readonly AnimationCurve carefulByAngleToGoal = new AnimationCurve(new Keyframe[] { 
            new Keyframe (0, 0),
            new Keyframe (20f, 0.01f),
            new Keyframe (30f, 0.05f),
            new Keyframe (50f, 0.1f),
            new Keyframe (90f, 0.4f),
        });

        private const float ALREADY_ACTIVE_LERP_SPEED = 3;

        private readonly float bewareVelocityMod;
        private readonly ForwardCurve curve;
        private readonly float maxBallProgress;
        private readonly bool ignoreChasing;
        private readonly float minBallProgress;
        private readonly float minBallHeight;
        private readonly MovementType movementType;

        public enum BewareMod {
            SuperCareful,
            Careful,
            Normal,
            Risky
        }

        public enum ForwardCurve {
            EarlyToGoal,
            Wingman,
            MostlyStraight
        }

        /// <summary>
        /// Construct a running behaviour with ignoring chasing.
        /// </summary>
        /// <param name="forwardCurve"></param>
        public RunForwardWithBallBehaviour(
            float minBallProgress,
            ForwardCurve forwardCurve,
            BewareMod bewareMod = BewareMod.Normal,
            bool ignoreChasing = true,
            float minBallHeight = 0,
            float maxBallProgress = 1,
            MovementType movementType = MovementType.BestHeCanDo) {

            this.ignoreChasing = ignoreChasing;

            this.minBallHeight = minBallHeight;

            this.minBallProgress = minBallProgress;
            this.curve = forwardCurve;
            this.maxBallProgress = maxBallProgress;

            this.movementType = movementType;

            switch (bewareMod) {
                case BewareMod.SuperCareful: this.bewareVelocityMod = BEWARE_SUPER_CAREFUL; break;
                case BewareMod.Careful: this.bewareVelocityMod = BEWARE_CAREFUL; break;
                case BewareMod.Normal: this.bewareVelocityMod = BEWARE_NORMAL; break;
                case BewareMod.Risky: this.bewareVelocityMod = BEWARE_RISKY; break;
            }
        }

        public RunForwardWithBallBehaviour (
            float maxBallProgress, 
            BewareMod bewareMod,
            ForwardCurve curve,
            float minBallProgress = 0) {

            this.curve = curve;
            this.maxBallProgress = maxBallProgress;
            this.minBallProgress = minBallProgress;
            this.movementType = MovementType.BestHeCanDo;

            switch (bewareMod) {
                case BewareMod.Careful: this.bewareVelocityMod = BEWARE_CAREFUL; break;
                case BewareMod.Normal: this.bewareVelocityMod = BEWARE_NORMAL; break;
                case BewareMod.Risky: this.bewareVelocityMod = BEWARE_RISKY; break;
            }
        }

        public override bool Behave(bool isAlreadyActive) {
            if (ball.HolderPlayer != Player) {
                return false;
            }

            if (Player.GameTeam.BallProgress > maxBallProgress) {
                return false;
            }

            if (Player.GameTeam.BallProgress < minBallProgress) {
                return false;
            }

            if (ball.transform.position.y < minBallHeight) {
                return false;
            }

            var playerPosition = Player.Position;

            var toGoal = targetGoalNet.Position - playerPosition;
            var toForward = goalNet.Direction;

            // to goal rotation by ball progress;
            var ballProgress = Player.GameTeam.BallProgress;

            var lerper = EngineSettings.Current.RunningForwardCurves[(int)curve].Evaluate(ballProgress);

            var runningDir = Vector3.Lerp(toForward, toGoal, lerper);

            if (!ignoreChasing && Player.CanMyMarkersChaseMe(
                bewareVelocityMod + 
                carefulbyBallprogress.Evaluate (Player.PlayerFieldProgress) +
                carefulByAngleToGoal.Evaluate (Mathf.Abs (Vector3.SignedAngle (Player.GoalDirection, targetGoalNet.Position - Player.Position, Vector3.up)))
                )) {

                return false; // check original dir.
            }

            var targetPosition = Player.Position + runningDir * 5;

            var avoided = targetPosition;
            Player.AvoidMarkers(opponents, ref avoided);

            if (Player.BoundCheck (0, avoided, new Vector2 (fieldEndX, fieldEndY))) {
                targetPosition = avoided; // approve.
            }

            Player.CurrentAct = Acts.RunningForward;

            if (!isAlreadyActive) {
                Player.runForwardBehaviourFinalPosition = Player.Position + Player.Direction * 2;
            }

            Player.runForwardBehaviourFinalPosition = Vector3.Lerp(Player.runForwardBehaviourFinalPosition, targetPosition, deltaTime * ALREADY_ACTIVE_LERP_SPEED);

            // available to run forward.
            if (Player.MoveTo(in deltaTime, Player.runForwardBehaviourFinalPosition, true, movementType)) {
                return false;
            }

            return true;
        }
    }
}
