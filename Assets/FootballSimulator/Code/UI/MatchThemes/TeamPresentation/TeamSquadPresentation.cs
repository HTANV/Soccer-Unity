using FStudio.Database;
using FStudio.Loaders;
using Shared.Responses;
using FStudio.Data;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FStudio.UI.MatchThemes.TeamPresentation {
    public class TeamSquadPresentation : Panel {
        public StaticPool<TeamPresentationSquadPlayer, TeamSquadMember> pool { private set; get; }
        [SerializeField] private AssetReference squadPlayer;
        [SerializeField] private Transform holder;

        public async Task SetSquadMembers(TeamSquadMember[] players) {
            if (pool == null) {
                pool = new StaticPool<TeamPresentationSquadPlayer, TeamSquadMember>(squadPlayer, holder);
            }

            await pool.SetMembers(players);
        }

        public static TeamSquadMember[] PlayersToSquadMembers(PlayerEntry[] players, Formations formation, TeamEntry team, bool kitSelection = false) {
            var formationRules = FormationRules.GetTeamFormation(formation).Positions;

            KitEntry getKit (PlayerEntry x) {
                if (team == null) {
                    return null;
                }

                return kitSelection ? team.AwayKit : team.HomeKit;
            }

            return players.Select((x, y) =>

                new TeamSquadMember(x,

                (formationRules.Length > y) ? formationRules[y] : PositionRules.GetBasePosition(x.Position), getKit (x)

                )).ToArray();
        }

        public static TeamSquadMember[] PlayersToSquadMembers(PlayerEntry[] players, TeamEntry team, bool kitSelection = false) {
            KitEntry getKit() {
                if (team == null) {
                    return null;
                }

                return kitSelection ? team.AwayKit : team.HomeKit;
            }

            return players.Select((x, y) =>

                new TeamSquadMember(x,

                PositionRules.GetBasePosition(x.Position), getKit()

                )).ToArray();
        }
    }
}