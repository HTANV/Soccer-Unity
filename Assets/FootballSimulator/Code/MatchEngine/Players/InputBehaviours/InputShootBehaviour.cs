using FStudio.MatchEngine.Enums;
using UnityEngine;
using FStudio.MatchEngine.EngineOptions;
using FStudio.MatchEngine.Players.InputBehaviours;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class InputShootBehaviour : BaseBehaviour, IInputBehaviour {
        private (Transform point, float angleFree) shootingTarget;

        private Vector3 shootingDir;

        public bool IsTriggered { private get; set; }
        public Vector3 InputDirection { set; private get; }

        public override bool Behave(bool isAlreadyActive) {
            if (!IsTriggered && !isAlreadyActive) {
                return false;
            }

            if (!Player.isInputControlled) {
                IsTriggered = false;
                return false;
            }

            if (Player.IsThrowHolder) {
                IsTriggered = false;
                return false;
            }

            if (Player.IsCornerHolder) {
                IsTriggered = false;
                return false;
            }

            if (ball.HolderPlayer != Player) {
                IsTriggered = false;
                return false;
            }

            if (shootingTarget.point == null) {
                shootingTarget = targetGoalNet.GetShootingVector(
                    Player, opponents);

                shootingDir = shootingTarget.point.position - Player.Position;

                Debug.Log($"[SHOULD SHOOT] {shootingTarget}");

                isAlreadyActive = true;
            }

            if (isAlreadyActive) {
                Player.GameTeam.KeepPlayerBehavioursForAShortTime();

                Player.CurrentAct = Acts.InputShoot;

                Debug.Log($"Shooting => {shootingTarget}");

                Player.Stop(in deltaTime);

                if (Player.LookTo(in deltaTime, shootingDir)) {
                    var shootPowerByAngleFree = EngineOptions_ShootingSettings.Current.shootPowerModByAngleFree.Evaluate(shootingTarget.angleFree);

                    var target = targetGoalNet.
                        GetShootingVectorFromPoint(Player, shootingTarget.point) * shootPowerByAngleFree;

                    Player.Shoot(target);

                    shootingTarget = default;
                }

                return true;
            }

            return false;
        }
    }
}
