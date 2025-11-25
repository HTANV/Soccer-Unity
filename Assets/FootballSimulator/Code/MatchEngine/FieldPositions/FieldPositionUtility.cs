using UnityEngine;

namespace FStudio.MatchEngine.FieldPositions {
    public static class FieldPositionUtility {
        public static Vector3 PositionToVector3(Vector3 netDirection, in int fieldEndX, in int fieldEndY, FieldPosition fieldPosition) {
            var formalPosition = new Vector3(fieldEndX * fieldPosition.VerticalPlacement, 0, fieldEndY * fieldPosition.HorizontalPlacement);
            if (netDirection.x < 0) {
                // team 2. reverse points
                formalPosition.x = fieldEndX - formalPosition.x;
                formalPosition.z = fieldEndY - formalPosition.z;
                return formalPosition;
            }

            return formalPosition;
        }
    }
}
