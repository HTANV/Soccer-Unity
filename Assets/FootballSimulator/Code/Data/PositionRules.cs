using System.Collections.Generic;
using System.Linq;

namespace FStudio.Data {
    public class PositionRules {

        private static Dictionary<Positions, Positions> rules = new Dictionary<Positions, Positions>() {
            { Positions.GK, Positions.GK },
            { Positions.LB, Positions.LB },
            { Positions.RB, Positions.RB },
            { Positions.LMF, Positions.LMF },
            { Positions.RMF, Positions.RMF },
            { Positions.LW, Positions.LW },
            { Positions.RW, Positions.RW },
            { Positions.CB, Positions.CB | Positions.CB_L | Positions.CB_R },
            { Positions.CM, Positions.CM | Positions.CM_L | Positions.CM_R },
            { Positions.DMF, Positions.DMF | Positions.DMF_L | Positions.DMF_R },
            { Positions.AMF, Positions.AMF | Positions.AMF_L | Positions.AMF_R },
            { Positions.ST, Positions.ST | Positions.ST_L | Positions.ST_R }
        };

        public static Positions GetRandomPosition() {
            var randomPick = rules.Keys.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
            return randomPick;
        }

        public static Positions GetBasePosition(Positions position) {
            var result = rules.Where(x => x.Value.HasFlag(position)).FirstOrDefault();
            if (result.Equals(default(KeyValuePair<Positions, Positions>))) {
                // position doesnt have a rule.
                return position;
            }

            return result.Key;
        }

        /// <summary>
        /// Get all playable positions for the given base position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Positions GetPositions(Positions position) {
            return rules.Where(x => x.Key.HasFlag(position)).FirstOrDefault().Value;
        }

        public static IEnumerable<Positions> GetAllPositions() {
            return rules.Keys;
        }
    }
}