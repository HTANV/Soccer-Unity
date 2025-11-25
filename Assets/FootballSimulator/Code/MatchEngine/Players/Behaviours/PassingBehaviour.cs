
using FStudio.MatchEngine.Enums;
using UnityEngine;
using System.Linq;
using static FStudio.MatchEngine.Players.PlayerBase;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class PassingBehaviour : BaseBehaviour {
        private PassTarget target;

        private readonly bool onlyIfFrontOfUs;
        private readonly float minBallProgress;
        private readonly float maxBallProgress;
        private readonly float frontXThreshold;
        private readonly bool onlyIfCloserToGoalNet;

        public PassingBehaviour (float maxBallProgress = 1) {
            this.maxBallProgress = maxBallProgress;
        }

        /// <summary>
        /// Pick the targets only if they are closer to target goal net than us.
        /// </summary>
        /// <param name="minBallProgress"></param>
        /// <param name="onlyIfCloserToGoalNet"></param>
        public PassingBehaviour(
            float minBallProgress = 0,
            bool onlyIfCloserToGoalNet = false) {

            this.minBallProgress = minBallProgress;
            this.onlyIfCloserToGoalNet = onlyIfCloserToGoalNet;
        }

        /// <summary>
        /// Construct a passing behaviour with 'front of us' checker.
        /// When you checked 'onlyIfFrontOfUs' the player will pass only if the target is front of us in X Axis (to forward without considering horizontal position). So beware, centre forward can pass to the corner side :-)
        /// </summary>
        /// <param name="minBallProgress">Minimum ball progress to activate. Between 0-1</param>
        /// <param name="onlyIfFrontOfUs">Select if passing point is front of us.</param>
        /// <param name="frontXThreshold">If onlyIfFrontOfUs true, optionally add more X threshold to consider it is 'Front'</param>
        public PassingBehaviour (
            float minBallProgress, 
            bool onlyIfFrontOfUs,
            float frontXThreshold) {
            this.onlyIfFrontOfUs = onlyIfFrontOfUs;
            this.minBallProgress = minBallProgress;
            this.frontXThreshold = frontXThreshold;
        }

        public override bool Behave (bool isAlreadyActive) {
            if (ball.HolderPlayer != Player) {
                return false;
            }

            if (!isAlreadyActive) {
                if (Player.GameTeam.BallProgress < minBallProgress) {
                    return false;
                }

                if (Player.GameTeam.BallProgress > maxBallProgress) {
                    return false;
                }

                var targetGoalNetPosition = targetGoalNet.Position;

                var distanceToTargetGoalNet = Vector3.Distance(Player.Position, targetGoalNetPosition);

                var targets = teammates.Where(x => 
                    (!onlyIfCloserToGoalNet || Vector3.Distance (x.Position, targetGoalNetPosition) < distanceToTargetGoalNet) &&
                    (!onlyIfFrontOfUs || Player.IsFrontOfMe(x.Position, frontXThreshold))).ToArray();

                target = Player.FindPassTarget(in targets, in targetGoalNetPosition);

                if (target.IsValid) {
                    Debug.Log($"[PassingBehaviour] OptionName: {target._OptionName}");
                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                Player.CurrentAct = Acts.PassingToBetterOpportunity;

                Player.GameTeam.KeepPlayerBehavioursForAShortTime();

                Player.Stop(in deltaTime);

                if (Player.LookTo(in deltaTime, target._Position - Player.Position)) {
                    // set pass target. after pass target player will behave with BallChasingBehaviour.
                    Player.PassingTarget = target._ActualTarget;

                    float speedMod = Player.SpeedModForPassing();

                    if (target._PassType == PassType.LongPass) {
                        var dir = target._Position - Player.Position;
                        var add = EngineSettings.Current.CrossTargetAdditionNormalByDistance.Evaluate(dir.magnitude);

                        var crossAddition = dir.normalized * add * target._PassPower;

                        Player.Cross(target._Position + crossAddition);
                    } else {
                        Player.Pass(target._Position, speedMod * target._PassPower);
                    }
                }

                return true;
            }

            return false;
        }
    }
}
