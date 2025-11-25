using FStudio.Database.Rules;
using Shared;
using FStudio.Data;
using System;
using System.Linq;
using UnityEngine;

namespace FStudio.Database {
    [Serializable]
    public class PlayerEntry : ScriptableObject, IUnique<PlayerEntry> {
        public int id { get; set; }

        [HideInInspector] public TeamEntry team;

        public string Name = "Unnamed";

        public virtual Positions Position {
            get {
                var formation = team.Formation;

                var rule = FormationRules.GetTeamFormation(formation);

                var myIndex = team.Players.ToList().FindIndex(x => x == this);

                return rule.Positions[myIndex];
            }
        }

        public SkinColor SkinColor;

        public HairStyles HairStyles;
        public HairColors HairColor;

        public FacialHairStyles FacialHairStyles;
        public HairColors FacialHairColor;

        public BootColor BootColor;
        public SockAccessoryColor SockAccessoryColor;

        [Range (150, 210)]
        public int height = 180;
        [Range (45, 100)]
        public int weight = 80;
        
        [Range(0, 100)] public int strength         = 50;
        [Range(0, 100)] public int acceleration     = 50;
        [Range(0, 100)] public int topSpeed         = 50;
        [Range(0, 100)] public int dribbleSpeed     = 50;
        [Range(0, 100)] public int jump             = 50;
        [Range(0, 100)] public int tackling         = 50;
        [Range(0, 100)] public int ballKeeping      = 50;
        [Range(0, 100)] public int passing          = 50;
        [Range(0, 100)] public int longBall         = 50;
        [Range(0, 100)] public int agility          = 50;
        [Range(0, 100)] public int shooting         = 50;
        [Range(0, 100)] public int shootPower       = 50;
        [Range(0, 100)] public int positioning      = 50;
        [Range(0, 100)] public int reaction         = 50;
        [Range(0, 100)] public int ballControl      = 50;

        private float MaxBaseSkill (in int baseSkill) => Mathf.Pow(baseSkill, 2.25f);

        private float CalculateOverallEffect (int mySkill, int baseSkill) {
            var power = MaxBaseSkill(in baseSkill);
            return power * mySkill;
        }

        public int Overall {
            get {
                var basePosition = PositionRules.GetBasePosition(Position);

                var baseSchema = PositionSkillRules.Current.GetDefaultSkillSchema(basePosition);

                var totalBase = MaxBaseSkill(baseSchema.strength        ) +
                                MaxBaseSkill(baseSchema.acceleration    ) +
                                MaxBaseSkill(baseSchema.topSpeed        ) +
                                MaxBaseSkill(baseSchema.jump            ) +
                                MaxBaseSkill(baseSchema.dribbleSpeed    ) +
                                MaxBaseSkill(baseSchema.tackling        ) +
                                MaxBaseSkill(baseSchema.ballKeeping     ) +
                                MaxBaseSkill(baseSchema.passing         ) +
                                MaxBaseSkill(baseSchema.longBall        ) +
                                MaxBaseSkill(baseSchema.agility         ) +
                                MaxBaseSkill(baseSchema.shooting        ) +
                                MaxBaseSkill(baseSchema.shootPower      ) +
                                MaxBaseSkill(baseSchema.positioning     ) +
                                MaxBaseSkill(baseSchema.reaction        ) +
                                MaxBaseSkill(baseSchema.ballControl     );



                var totalPower =
                CalculateOverallEffect(strength       ,baseSchema.strength       ) +
                CalculateOverallEffect(acceleration   ,baseSchema.acceleration   ) +
                CalculateOverallEffect(topSpeed       ,baseSchema.topSpeed       ) +
                CalculateOverallEffect(jump           ,baseSchema.jump           ) +
                CalculateOverallEffect(dribbleSpeed   ,baseSchema.dribbleSpeed   ) +
                CalculateOverallEffect(tackling       ,baseSchema.tackling       ) +
                CalculateOverallEffect(ballKeeping    ,baseSchema.ballKeeping    ) +
                CalculateOverallEffect(passing        ,baseSchema.passing        ) +
                CalculateOverallEffect(longBall       ,baseSchema.longBall       ) +
                CalculateOverallEffect(agility        ,baseSchema.agility        ) +
                CalculateOverallEffect(shooting       ,baseSchema.shooting       ) +
                CalculateOverallEffect(shootPower     ,baseSchema.shootPower     ) +
                CalculateOverallEffect(positioning    ,baseSchema.positioning    ) +
                CalculateOverallEffect(reaction       ,baseSchema.reaction       ) +
                CalculateOverallEffect(ballControl    ,baseSchema.ballControl    );

                var quality = (int)Mathf.Round(totalPower / totalBase);

                return quality;
            }
        }

        public bool IsSame(PlayerEntry other) {
            return other == this;
        }

        public PlayerEntry Clone(TeamEntry teamEntry) {
            var clone = CreateInstance<PlayerEntry>();

            clone.team = teamEntry;

            clone.weight = weight;
            clone.BootColor = BootColor;
            clone.FacialHairColor = FacialHairColor;
            clone.FacialHairStyles = FacialHairStyles;
            clone.HairColor = HairColor;
            clone.HairStyles = HairStyles;
            clone.height = height;
            clone.SkinColor = SkinColor;
            clone.SockAccessoryColor = SockAccessoryColor;
            clone.Name = Name;

            clone.tackling = tackling;
            clone.positioning = positioning;
            clone.reaction = reaction;
            clone.ballControl = ballControl;
            clone.acceleration = acceleration;
            clone.agility = agility;
            clone.shooting = shooting;
            clone.shootPower = shootPower; 
            clone.ballKeeping = ballKeeping;
            clone.dribbleSpeed = dribbleSpeed;
            clone.topSpeed = topSpeed;
            clone.jump = jump;
            clone.strength= strength;
            clone.longBall = longBall;
            clone.passing = passing;

            return clone;
        }
    }
}

