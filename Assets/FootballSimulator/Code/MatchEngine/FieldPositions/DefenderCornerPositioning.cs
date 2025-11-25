using UnityEngine;

namespace FStudio.MatchEngine.FieldPositions {
    public class DefenderCornerPositioning : BasePositionsData<DefenderCornerPositioning>, IFieldPositioning {
        FieldPosition[] IFieldPositioning.FieldPositions => FieldPositions;
    }
}
