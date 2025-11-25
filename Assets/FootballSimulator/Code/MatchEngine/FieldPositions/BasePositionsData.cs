using UnityEngine;
using System.Collections.Generic;

using FStudio.Scriptables;

namespace FStudio.MatchEngine.FieldPositions {
    [System.Serializable]
    public abstract class BasePositionsData<T> : SerializedSingletonScriptable<T> where T : Object {

        [SerializeField] private Dictionary<Data.Positions, FieldPosition> Indexed;

        public bool FixedLength = true;

        [SerializeField]
        public FieldPosition[] FieldPositions = new FieldPosition[] {
            new FieldPosition ( Data.Positions.GK),
            new FieldPosition ( Data.Positions.CB),
            new FieldPosition ( Data.Positions.CB_R),
            new FieldPosition ( Data.Positions.CB_L),
            new FieldPosition ( Data.Positions.LB),
            new FieldPosition ( Data.Positions.RB),
            new FieldPosition ( Data.Positions.CM),
            new FieldPosition ( Data.Positions.CM_R),
            new FieldPosition ( Data.Positions.CM_L),
            new FieldPosition ( Data.Positions.RMF),
            new FieldPosition ( Data.Positions.LMF),
            new FieldPosition ( Data.Positions.AMF),
            new FieldPosition ( Data.Positions.AMF_R),
            new FieldPosition ( Data.Positions.AMF_L),
            new FieldPosition ( Data.Positions.ST),
            new FieldPosition ( Data.Positions.ST_R),
            new FieldPosition ( Data.Positions.ST_L)
        };

        public FieldPosition GetPosition (Data.Positions position) {
            if (Indexed == null) {
                Indexed = new Dictionary<Data.Positions, FieldPosition>();
                foreach (var fieldPos in FieldPositions) {
                    Indexed.Add(fieldPos.Position, fieldPos);
                }
            }

            return Indexed[position];
        }
    }
}
