
using FStudio.MatchEngine.Players.PlayerController;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    internal class DribblingBehaviour : RunForwardWithBallBehaviour {
        private readonly AnimationCurve chanceCurveByFieldProgress = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.1f),
            new Keyframe(0.75f, 0.2f),
            new Keyframe(1, 0.3f)
            );

        private readonly AnimationCurve chanceMultiplyCurveBySkill = new AnimationCurve(
                new Keyframe(50, 0),
                new Keyframe(70f, 0.1f),
                new Keyframe(80f, 0.2f),
                new Keyframe(90, 0.3f),
                new Keyframe(100, 0.4f)
            );

        public DribblingBehaviour(ForwardCurve forwardCurve
            ) : base(0,
                forwardCurve,
                BewareMod.Risky,
                true,
                0,
                1,
                MovementType.BestHeCanDo) {
        }

        public override bool Behave (bool isAlreadyActive) {
            if (!Player.IsHoldingBall) {
                return false;
            }

            if (!isAlreadyActive) {
                // chance calculation.
                var skill =
                    Player.MatchPlayer.ActualTopSpeed * 0.2f +
                    Player.MatchPlayer.ActualDribbleSpeed * 0.2f +
                    Player.MatchPlayer.ActualBallKeeping * 0.6f;

                var chance = chanceMultiplyCurveBySkill.Evaluate(skill) * chanceCurveByFieldProgress.Evaluate(Player.PlayerFieldProgress);

                if (Random.Range(0f, 1f) < chance) {
                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                Player.CurrentAct = Enums.Acts.Dribbling;
                return base.Behave(isAlreadyActive);
            } else {
                return false;
            }
        }
    }
}
