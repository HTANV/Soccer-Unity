using UnityEngine;

namespace FStudio.MatchEngine.FieldPositions {
    public class AttackerCornerPositioning : BasePositionsData<AttackerCornerPositioning>, IFieldPositioning {
        FieldPosition[] IFieldPositioning.FieldPositions => FieldPositions;
    }
}
