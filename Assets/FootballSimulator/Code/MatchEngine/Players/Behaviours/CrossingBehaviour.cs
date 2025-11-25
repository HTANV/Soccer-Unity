
using FStudio.MatchEngine.Enums;
using FStudio.Data;
using UnityEngine;

using System.Linq;
using Shared;

namespace FStudio.MatchEngine.Players.Behaviours {
    /// <summary>
    /// Cross to the penalty area.
    /// </summary>
    public class CrossingBehaviour : BaseBehaviour {
        private PlayerBase target;

        private const float MIN_DISTANCE_TO_TARGET = 16;
        private const float MAX_X_DIFF = 20;
        private const float MIN_Z_DIFF = 14;

        private readonly float minBallProgress;
        private readonly float chanceMultiplier;

        public CrossingBehaviour (float minBallProgress = 0, float chanceMultiplier = 1) {
            this.minBallProgress = minBallProgress;
            this.chanceMultiplier = chanceMultiplier;
        }

        private static readonly Curve MAX_BACKWARDS_DIFF = new Curve(new Curve.Point[] { 
            new Curve.Point(0, 0),
            new Curve.Point(0.85f, 0),
            new Curve.Point(0.9f, 5),
            new Curve.Point(0.93f, 12),
            new Curve.Point(0.96f, 15),
            new Curve.Point(1, 20)
        });

        private static AnimationCurve CROSS_CHANCE_BY_BALL_PROGRESS = new AnimationCurve(new Keyframe[] { 
            new Keyframe (0, 0),
            new Keyframe (0.5f, 0.5f),
            new Keyframe (0.7f, 1f),
            new Keyframe (0.8f, 10),
            new Keyframe (0.9f, 30),
            new Keyframe (0.95f, 60),
            new Keyframe (1, 90)
        });

        private float ZPower (in Vector3 position) {
            return Mathf.Abs(position.z - fieldEndY / 2);
        }

        public override bool Behave(bool isAlreadyActive) {
            if (ball.HolderPlayer != Player) {
                return false;
            }

            if (!isAlreadyActive) {
                if (Player.GameTeam.BallProgress < minBallProgress) {
                    return false;
                }

                var chance = 
                    CROSS_CHANCE_BY_BALL_PROGRESS.Evaluate(Player.PlayerFieldProgress) * 
                    chanceMultiplier;

                var chanceRoll = Random.Range(0, 100) < chance;

                if (!chanceRoll) {
                    return false;
                }

                var myPos = Player.Position;

                float myZPower = ZPower(myPos) - 1; // -1 addition.

                var myX = myPos.x;
                var myZ = myPos.z;
                var myDir = Player.toGoalXDirection;

                var max_backwards = MAX_BACKWARDS_DIFF.Evaluate(Player.PlayerFieldProgress);

                var targetPlayer = teammates.
                    Where(x => 
                    x != Player && 
                    !x.IsInOffside && 
                    ZPower (x.Position) < myZPower &&
                    Mathf.Abs (myX - x.Position.x) < MAX_X_DIFF &&
                    Mathf.Abs(myZ - x.Position.z) > MIN_Z_DIFF &&
                    (myX - x.Position.x) * myDir < max_backwards && 
                    Vector3.Distance (myPos, x.Position) >= MIN_DISTANCE_TO_TARGET).
                    OrderByDescending(x => x.PlayerFieldProgress * 10 + (x.CanMyMarkersChaseMe(1) ? 0 : 10)).
                    FirstOrDefault();

                if (targetPlayer != null) {
                    Player.PassingTarget = targetPlayer;

                    var playerPos = Player.Position;

                    isAlreadyActive = true;

                    //
                    target = targetPlayer;
                }
            }

            if (isAlreadyActive) {
                Player.GameTeam.KeepPlayerBehavioursForAShortTime();

                Player.CurrentAct = Acts.Crossing;

                if (Player.LookTo (in deltaTime, target.Position - Player.Position)) {
                    Player.Cross(target.Position);
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
