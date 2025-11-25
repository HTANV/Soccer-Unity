using UnityEngine;

namespace FStudio.MatchEngine.FieldPositions {
    [System.Serializable]
    public struct FieldPosition {
        public FieldPosition (FStudio.Data.Positions Position) {
            this.Position = Position;
            this.Name = Position.ToString();
            this.HorizontalPlacement = 0;
            this.VerticalPlacement = 0;
        }

        public FieldPosition (FieldPosition from) {
            this.Position = from.Position;
            this.Name = from.Position.ToString();
            this.HorizontalPlacement = from.HorizontalPlacement;
            this.VerticalPlacement = from.VerticalPlacement;
        }

        public string Name;

        public FStudio.Data.Positions Position;

        [Range(0, 1)]
        public float HorizontalPlacement;

        [Range (0,1)]
        public float VerticalPlacement;
    }
}
