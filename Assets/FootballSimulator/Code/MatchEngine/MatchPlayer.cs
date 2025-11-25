using UnityEngine;

using Shared.Responses;
using FStudio.MatchEngine.FieldPositions;
using FStudio.Data;
using FStudio.Database;

namespace FStudio.MatchEngine {
    public class MatchPlayer {
        public readonly PlayerEntry Player;

        public Positions Position;

        public readonly int Number;

        public MatchPlayer (int number, PlayerEntry player, Positions position) {
            this.Player = player;
            this.Position = position;
            this.Number = number;
            Setup();
        }

        public int ModifySkill (int skill, AnimationCurve curve) {
            var main = (float)skill;
            main = curve.Evaluate(main/100f) * 100f;
            return Mathf.RoundToInt (main);
        }

        public int ActualStrength { get; private set; }
        public int ActualAcceleration { get; private set; }
        public int ActualTopSpeed { get; private set; }
        public int ActualDribbleSpeed { get; private set; }
        public int ActualPassing { get; private set; }
        public int ActualLongBall { get; private set; }
        public int ActualAgility { get; private set; }
        public int ActualShooting { get; private set; }
        public int ActualShootPower { get; private set; }
        public int ActualPositioning { get; private set; }
        public int ActualReaction { get; private set; }
        public int ActualBallControl { get; private set; }
        public int ActualTackling { get; private set; }
        public int ActualBallKeeping { get; private set; }

        public int ActualJump { get; private set; }

        private void Setup () {
            ActualStrength = ModifySkill(Player.strength, PlayerSkillCurves.Current.StrengthCurve);

            ActualAcceleration = ModifySkill(Player.acceleration, PlayerSkillCurves.Current.SpeedCurve);
            ActualTopSpeed = ModifySkill(Player.topSpeed, PlayerSkillCurves.Current.SpeedCurve);
            ActualDribbleSpeed = Mathf.RoundToInt(ModifySkill(Player.dribbleSpeed, PlayerSkillCurves.Current.DribbleSpeedCurve));
            ActualPassing = Mathf.RoundToInt(ModifySkill(Player.passing, PlayerSkillCurves.Current.PassingCurve));
            ActualLongBall = Mathf.RoundToInt(ModifySkill(Player.longBall, PlayerSkillCurves.Current.PassingCurve));
            ActualAgility = Mathf.RoundToInt(ModifySkill(Player.agility, PlayerSkillCurves.Current.AgilityCurve));
            ActualShooting = Mathf.RoundToInt(ModifySkill(Player.shooting, PlayerSkillCurves.Current.ShootingCurve));
            ActualShootPower = ModifySkill(Player.shooting, PlayerSkillCurves.Current.ShootingCurve);

            /// chemistry and wrong positioning affects positioning skill.
            ActualPositioning = Mathf.RoundToInt (ModifySkill(Player.positioning, PlayerSkillCurves.Current.PositioningCurve));
            ActualReaction = Mathf.RoundToInt (ModifySkill(Player.positioning, PlayerSkillCurves.Current.ReactionCurve));
            ///

            ActualBallControl = ModifySkill(Player.ballControl, PlayerSkillCurves.Current.BallControlCurve);
            ActualTackling = Mathf.RoundToInt(ModifySkill(Player.tackling, PlayerSkillCurves.Current.TacklingCurve));
            ActualBallKeeping = ModifySkill(Player.ballKeeping, PlayerSkillCurves.Current.TacklingCurve);

            ActualJump = ModifySkill(Player.jump, PlayerSkillCurves.Current.JumpingCurve);
        }

        #region GET FUNCTIONS with MODIFIERS, for in game match.
        public float GetStrength() => ActualStrength * EngineSettings.Current.StrengthModifier;
        public float GetAcceleration() => ActualAcceleration * EngineSettings.Current.AccerelationModifier;
        public float GetTopSpeed() => ActualTopSpeed * EngineSettings.Current.TopSpeedModifier;
        public float GetDribbleSpeed() => ActualDribbleSpeed * EngineSettings.Current.DribbleModifier;
        public float GetLongBall() => ActualLongBall * EngineSettings.Current.PassingModifier;
        public float GetAgility() => ActualAgility * EngineSettings.Current.AgilityModifier;
        public float GetShooting() => ActualShooting * EngineSettings.Current.ShootingModifier;
        #endregion

        public float GetDribbleSpeedModifier () {
            float dribbleModifier = GetDribbleSpeed() / 100f;
            dribbleModifier = 0.75f + dribbleModifier / 4;

            return dribbleModifier;
        }
    }
}
