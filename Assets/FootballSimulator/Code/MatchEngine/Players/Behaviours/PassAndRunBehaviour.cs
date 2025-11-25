
using FStudio.MatchEngine.Enums;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class PassAndRunBehaviour : BallChasingBehaviour {
        private bool isActivated;
        private float runUntil;

        private readonly AnimationCurve joinCurveByFieldProgress;

        private readonly AnimationCurve timeCurveByBallProgress = new AnimationCurve(
                new Keyframe (0, 4),
                new Keyframe (0.5f, 7),
                new Keyframe(1f, 10)
            );

        public PassAndRunBehaviour () {
            joinCurveByFieldProgress = EngineSettings.Current.RunningForwardCurves[2];
        }

        private void Reset () {
            ForceBehaviour = false;
            isActivated = false;
        }

        public override bool Behave(bool isAlreadyActive) {
            if (!isAlreadyActive) { // this is a manual behaviour to trigger.
                Reset();
                return false;
            }

            if (!ShouldIAttackWhenNotHoldingTheBall ()) {
                Reset();
                return false;
            }

            if (isAlreadyActive && !isActivated) {
                isActivated = true;

                ForceBehaviour = true; // force run.

                runUntil = time + timeCurveByBallProgress.Evaluate (Player.GameTeam.BallProgress);
            }
            
            if (runUntil < time || AmITheChaser ().result != BallChasingResult.None) {
                Reset();

                return false;
            }

            Player.CurrentAct = Acts.PassAndRun;

            // curve target position by field progress.
            var targetPosition = Vector3.Lerp (
                Player.Position + goalNet.Direction * 5, 
                targetGoalNet.Position, joinCurveByFieldProgress.Evaluate (Player.PlayerFieldProgress));

            if (Player.IsInOffside) {
                return true;
            }

            Player.AvoidMarkers(teammates, ref targetPosition, 3);
            KeepInField (ref targetPosition);

            if (Player.MoveTo(in deltaTime, targetPosition, true)) {
                Reset();

                return false;
            }

            return true;
        }
    }
}
