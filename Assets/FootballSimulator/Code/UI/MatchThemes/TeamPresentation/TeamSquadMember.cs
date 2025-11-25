using FStudio.Database;
using Shared;
using Shared.Responses;
using FStudio.Data;

namespace FStudio.UI.MatchThemes.TeamPresentation {
    public class TeamSquadMember : IUnique<TeamSquadMember> {
        public readonly PlayerEntry player;
        public readonly KitEntry kit;
        public readonly Positions position;
        
        public TeamSquadMember(PlayerEntry player, Positions position, KitEntry kit = null) {
            this.player = player;
            this.position = position;
            this.kit = kit;
        }

        public bool IsSame(TeamSquadMember other) {
            return this == other;
        }
    }
}
