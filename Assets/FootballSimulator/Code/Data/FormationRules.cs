using FStudio.Data;

using System.Collections.Generic;
using System.Linq;

namespace FStudio.Data {
    public static class FormationRules {
        private static Dictionary<Formations, TeamFormation> Forms = new Dictionary<Formations, TeamFormation>() {
            { Formations._4_4_2, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.LMF,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.RMF,
                 Positions.ST_L,
                 Positions.ST_R
            } } },

           { Formations._3_5_2, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.LMF,
                 Positions.CM_L,
                 Positions.CM,
                 Positions.CM_R,
                 Positions.RMF,
                 Positions.ST_L,
                 Positions.ST_R
            } } },

           { Formations._4_1_4_1, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.DMF,
                 Positions.LMF,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.RMF,
                 Positions.ST
            } } },

           { Formations._4_2_3_1_A, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.DMF_L,
                 Positions.DMF_R,
                 Positions.LMF,
                 Positions.RMF,
                 Positions.AMF,
                 Positions.ST
            } } },

           { Formations._4_2_3_1_B, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.DMF_L,
                 Positions.DMF_R,
                 Positions.LMF,
                 Positions.RMF,
                 Positions.CM,
                 Positions.ST
            } } },

           { Formations._4_3_2_1, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.DMF,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.AMF_L,
                 Positions.AMF_R,
                 Positions.ST
            } } },

           { Formations._4_3_3, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.LMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.LW,
                 Positions.RW,
                 Positions.ST
            } } },

           { Formations._4_1_3_2, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.DMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.LMF,
                 Positions.ST_L,
                 Positions.ST_R
            } } },

           { Formations._3_1_2_3_1, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.DMF,
                 Positions.LMF,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.RMF,
                 Positions.AMF,
                 Positions.ST
            } } },

           { Formations._3_2_1_3_1, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.DMF_L,
                 Positions.DMF_R,
                 Positions.LMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.AMF,
                 Positions.ST
            } } },

           { Formations._3_2_3_2, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.DMF_L,
                 Positions.DMF_R,
                 Positions.LMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.ST_L,
                 Positions.ST_R
            } } },

           { Formations._3_4_3, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.LMF,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.RMF,
                 Positions.LW,
                 Positions.ST,
                 Positions.RW
            } } },

           { Formations._5_3_2, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.LMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.ST_L,
                 Positions.ST_R
            } } },

           { Formations._5_4_1, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.LMF,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.RMF,
                 Positions.ST
            } } },

           { Formations._5_1_1_2_1_A, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.DMF,
                 Positions.LMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.ST
            } } },

           { Formations._5_1_1_2_1_B, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.CM,
                 Positions.AMF,
                 Positions.LW,
                 Positions.RW,
                 Positions.ST
            } } },

           { Formations._2_3_5, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.CB_L,
                 Positions.CB_R,
                 Positions.LMF,
                 Positions.CM,
                 Positions.RMF,
                 Positions.LW,
                 Positions.ST_L,
                 Positions.ST,
                 Positions.ST_R,
                 Positions.RW
            } } },

           { Formations._5_2_3, new TeamFormation() { Positions = new Positions[] {
                 Positions.GK,
                 Positions.LB,
                 Positions.CB_L,
                 Positions.CB,
                 Positions.CB_R,
                 Positions.RB,
                 Positions.CM_L,
                 Positions.CM_R,
                 Positions.ST_L,
                 Positions.ST,
                 Positions.ST_R
            } } }
        };

        public static TeamFormation GetTeamFormation(Formations formation) {
            if (Forms.ContainsKey(formation)) {
                return Forms[formation];
            }

            throw new System.NotImplementedException($"Formation is not implemented: {formation}");
        }

        public static Formations GetRandomFormation() {
            return Forms.Keys.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
        }
    }
}
