using FStudio.MatchEngine.Enums;
using UnityEngine;
using System.Linq;
using FStudio.MatchEngine.Events;
using FStudio.Events;
using FStudio.MatchEngine.EngineOptions;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class ChipShootingBehaviour : AbstractShootingBehaviour {
        private const float MAX_CHIP_ANGLE = 45;

        private const float CHIP_DISTANCE_MOD = 1.4f;
        private const float CHIP_SHOT_POWER_DIVIDER = 2.7f;

        private const float MIN_CHIP_SHO0T_DISTANCE = 18f;

        private readonly AnimationCurve POWER_BY_BALL_PROGRESS_CURVE = new AnimationCurve(new Keyframe[] { 
            new Keyframe(0, 0.8f),
            new Keyframe(0.7f, 0.6f),
            new Keyframe(0.8f, 0.45f),
            new Keyframe(0.9f, 0.25f),
            new Keyframe(1, 0.15f),
        });

        private (Transform point, float angleFree) shootingTarget;

        private Vector3 shootingDir;

        private bool IsGoalNetAvailableForChipShoot () {
            var goalNetPosition = targetGoalNet.Position;
            var myPosition = Player.Position;
            var meToGoal = Vector3.Distance(myPosition, goalNetPosition);

            if (meToGoal < MIN_CHIP_SHO0T_DISTANCE) {
                return false;
            }

            var lastOpponent = opponents.OrderBy(x => x.XPower(fieldEndX, x.Position)).FirstOrDefault();

            var lastOpponentPosition = lastOpponent.Position;

            // 
            var lastOpponentToTargetGoalNet = Vector3.Distance(lastOpponentPosition, goalNetPosition);
            var meToLastOpponent = Vector3.Distance(myPosition, lastOpponentPosition);

            if (meToLastOpponent < lastOpponentToTargetGoalNet * CHIP_DISTANCE_MOD) {
                return true;
            }

            return false;
        }

        public override bool Behave(bool isAlreadyActive) {
            if (ball.HolderPlayer != Player) {
                return false;
            }

            if (!isAlreadyActive) {
                if (!CanShoot ()) {
                    return false;
                }

                var targetGoalNetPosition = targetGoalNet.Position;
                var angleWithGoalNet = Mathf.Abs (Vector3.SignedAngle(targetGoalNetPosition - Player.Position, goalNet.Direction, Vector3.up));

                if (angleWithGoalNet > MAX_CHIP_ANGLE) {
                    return false;
                }

                if (IsGoalNetAvailableForChipShoot()) {
                    shootingTarget = targetGoalNet.GetShootingVector(Player, opponents);
                    isAlreadyActive = true;

                    shootingDir = shootingTarget.point.position - Player.Position;
                } else {
                    return false;
                }
            } 
            
            if (isAlreadyActive) {
                Player.GameTeam.KeepPlayerBehavioursForAShortTime();

                Player.CurrentAct = Acts.Shoot;

                Debug.Log($"Chip Shoot => {shootingTarget}");

                Player.Stop(in deltaTime);

                if (Player.LookTo(in deltaTime, shootingDir)) {
                    var shootPowerByAngleFree = EngineOptions_ShootingSettings.Current.shootPowerModByAngleFree.Evaluate(shootingTarget.angleFree);
                    var target = targetGoalNet.GetShootingVectorFromPoint(Player, shootingTarget.point) * shootPowerByAngleFree;

                    // add chip
                    var power = POWER_BY_BALL_PROGRESS_CURVE.Evaluate(Player.PlayerFieldProgress);
                    target += Vector3.up * power * target.magnitude;
                    //

                    var final = target / CHIP_SHOT_POWER_DIVIDER;

                    Player.Shoot(final);

                    EventManager.Trigger(new PlayerChipShootEvent(Player));
                }

                return true;
            }

            return false;
        }
    }
}
