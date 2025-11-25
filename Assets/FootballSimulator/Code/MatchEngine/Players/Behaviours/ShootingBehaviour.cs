
using FStudio.MatchEngine.Enums;
using UnityEngine;
using System.Linq;
using Shared;
using System;
using FStudio.MatchEngine.EngineOptions;
using static UnityEngine.GraphicsBuffer;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class ShootingBehaviour : AbstractShootingBehaviour {
        private const float ONEONONE_GK_TO_GOALNET_X_DIST = 5;
        private const float TO_GK_DIST = 10;
        private const float ONEONEONE_TOLERANCE_BONUS = 20;

        private (Transform point, float angleFree) shootingTarget;

        private Vector3 shootingDir;

        private readonly float minBallProgress;

        private readonly float toleranceMulti;

        private PlayerBase opponentGK;

        private float adminMulti = 1;

        private readonly Curve shootingSkillToleranceMultiplierCurve = new Curve(new Curve.Point[] { 
            new Curve.Point (0, 0.5f),
            new Curve.Point (0.5f, 0.9f),
            new Curve.Point (0.6f, 1f),
            new Curve.Point (0.7f, 1.3f),
            new Curve.Point (0.75f, 1.5f),
            new Curve.Point (0.8f, 1.6f),
            new Curve.Point (0.85f, 1.8f),
            new Curve.Point (0.9f, 1.9f),
            new Curve.Point (1f, 2f),
        });

        private readonly Curve keepingToleranceCurve = new Curve(new Curve.Point[] {
            new Curve.Point (0, 0f),
            new Curve.Point (0.5f, 0.25f),
            new Curve.Point (0.6f, 0.5f),
            new Curve.Point (0.7f, 0.75f),
            new Curve.Point (0.75f, 1),
            new Curve.Point (0.8f, 1.25f),
            new Curve.Point (0.85f, 1.5f),
            new Curve.Point (0.9f, 1.75f),
            new Curve.Point (1f, 2f),
        });

        private readonly Curve ballHeightToToleranceCurve = new Curve(new Curve.Point[] {
            new Curve.Point (-10, 0f),
            new Curve.Point (0, 0f),
            new Curve.Point (0.4f, 15f),
            new Curve.Point (1f, 40f),
            new Curve.Point (2f, 80f),
            new Curve.Point (20, 100)
        });
		
        private readonly Curve ballHeightToToleranceByBallProgressCurve = new Curve(new Curve.Point[] {
            new Curve.Point (0, 0f),
            new Curve.Point (0.75f, 0.2f),
            new Curve.Point (0.85f, 0.7f),
			new Curve.Point (0.9f, 1f),
			new Curve.Point (1f, 100f)
        });

        public ShootingBehaviour (float minBallProgress, float toleranceMulti = 1) {
            this.minBallProgress = minBallProgress;
            this.toleranceMulti = toleranceMulti;
        }

        public ShootingBehaviour () {
            this.minBallProgress = 0;
            this.toleranceMulti = 1;
        }

        private float toleranceMod {
            get {
                if (opponentGK == null) {
                    opponentGK = opponents.FirstOrDefault(x => x.IsGK);
                }

                var ballHeight = ball.transform.position.y;

                // calculate by shooting.
                var shooting = Player.MatchPlayer.ActualShooting * 0.75f + Player.MatchPlayer.ActualShootPower * 0.25f;

                var keeping = opponentGK.MatchPlayer.ActualPositioning * 0.25f +
                    opponentGK.MatchPlayer.ActualAcceleration * 0.125f +
                    opponentGK.MatchPlayer.ActualBallControl * 0.125f +
                    opponentGK.MatchPlayer.ActualReaction * 0.25f +
                    opponentGK.MatchPlayer.ActualTopSpeed * 0.25f;

                var gkTol = keepingToleranceCurve.Evaluate(keeping / 100);

                var tol = shootingSkillToleranceMultiplierCurve.Evaluate(shooting / 100f) * toleranceMulti;

				var byBallHeight = ballHeightToToleranceCurve.Evaluate (ballHeight);
				var byBallProgressByHeight = ballHeightToToleranceByBallProgressCurve.Evaluate (Player.GameTeam.BallProgress);
				
                return Mathf.Max (1, 1 + tol - gkTol + (ballHeightToToleranceCurve.Evaluate (ballHeight) * byBallProgressByHeight));
            }
        }

        public bool IsOneOnOneWithTheGoalKeeper () {
            var opponentGK = opponents.FirstOrDefault(x => x.IsGK);
            if (opponentGK == null) {
                return false; // NO GK? ok...
            }

            var gkPos = opponentGK.Position;
            var targetGoalNetPos = targetGoalNet.Position;

            if (Mathf.Abs (targetGoalNetPos.x - gkPos.x) > ONEONONE_GK_TO_GOALNET_X_DIST &&
                Vector3.Distance (gkPos, Player.Position) < TO_GK_DIST) {
                return true;
            }

            return false;
        }

        public override bool Behave(bool isAlreadyActive) {
            if (ball.HolderPlayer != Player) {
                return false;
            }

            if (!isAlreadyActive) {
                if (Player.GameTeam.BallProgress < minBallProgress) {
                    return false;
                }

                if (!CanShoot()) {
                    return false;
                }

                var forward = new Vector3(Player.toGoalXDirection, 0, 0);
                var toGoal = targetGoalNet.Position - Player.Position;

                var angleToGoal = AngleToGoal(targetGoalNet);

                var oneOnOne = IsOneOnOneWithTheGoalKeeper();

                float tolerance = adminMulti * (toleranceMod + (oneOnOne ? ONEONEONE_TOLERANCE_BONUS : 0));

                var shouldShoot = EngineSettings.Current.ShootRoll(
                    angleToGoal, 
                    toGoal.magnitude, 
                    tolerance);

                if (shouldShoot) {
                    adminMulti = 1;

                    shootingTarget = targetGoalNet.GetShootingVector(
                        Player, opponents);

                    var target = shootingTarget.point.position;

                    shootingDir = target - Player.Position;

                    Debug.Log($"[SHOULD SHOOT] {shootingTarget}");
                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                Player.GameTeam.KeepPlayerBehavioursForAShortTime();
                
                Player.CurrentAct = Acts.Shoot;

                Debug.Log($"Shooting => {shootingTarget}");

                Player.Stop(in deltaTime);

                if (Player.LookTo(in deltaTime, shootingDir)) {
                    var shootPowerByAngleFree = EngineOptions_ShootingSettings.Current.shootPowerModByAngleFree.Evaluate(shootingTarget.angleFree);

                    var target = targetGoalNet.
                        GetShootingVectorFromPoint(Player, shootingTarget.point) * shootPowerByAngleFree;

                    Player.Shoot(target);
                }

                return true;
            }

            return false;
        }
    }
}
