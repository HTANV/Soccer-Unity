
namespace FStudio.MatchEngine.FieldPositions {
    public interface IFieldPositioning {
        public FieldPosition[] FieldPositions { get; }

        public FieldPosition GetPosition (FStudio.Data.Positions position);
    }
}
