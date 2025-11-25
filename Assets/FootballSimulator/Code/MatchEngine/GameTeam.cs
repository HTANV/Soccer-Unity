using UnityEngine;

using FStudio.MatchEngine.Players;
using FStudio.MatchEngine.Players.Positions;
using FStudio.MatchEngine.Graphics;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Input;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;

using System.Threading.Tasks;
using FStudio.Input;

using System.Linq;
using static FStudio.MatchEngine.Players.PlayerBase;
using FStudio.Data;
using FStudio.MatchEngine.Events;
using FStudio.Events;
using FStudio.Graphics.RenderTextureCreators;
using Shared.Responses;
using FStudio.MatchEngine.Utilities;
using FStudio.MatchEngine.AIManager;

namespace FStudio.MatchEngine {
    public class GameTeam : MonoBehaviour {
        private const float NAME_TIMER_DELAYER = 1f;

        public PlayerBase[] GamePlayers;

        private IManager tacticManager;

        public MatchTeam Team;

        private TeamInputListener[] inputListeners = new TeamInputListener[InputListener.MAX_CONTROLLER];

        public float TeamDensity { private set; get; }

        [Tooltip("0 home team, 1 away team.")]
        public ushort TeamId;

        /// <summary>
        /// Current progress of the ball. Between 0-1, ball position from our goalnet to target goalnet in X axis.
        /// </summary>
        public float BallProgress { get; private set; }

        private float m_nextName;

        private Material mainKit, gkKit;

        public bool IsPlayerInputControlled(PlayerBase playerBase) {
            for (int i = 0; i < InputListener.MAX_CONTROLLER; i++) {
                if (inputListeners[i] != null) {
                    if (inputListeners[i].ActivePlayer == playerBase) {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnEnable() {
            EventManager.Subscribe<PlayerControlBallEvent>(OnBallHold);
        }

        private void OnDisable() {
            EventManager.UnSubscribe<PlayerControlBallEvent>(OnBallHold);
        }

        private void OnBallHold(PlayerControlBallEvent _) {
            if (Ball.Current.HolderPlayer.IsGK && Ball.Current.HolderPlayer.IsGKUntouchable) {
                // reset all behaviours;
                foreach (var player in GamePlayers) {
                    player.ResetBehaviours();
                }
            }
        }

        /// <summary>
        /// Add new input listener to the team.
        /// </summary>
        /// <param name="playerIndex"></param>
        public void AddInputListener(int playerIndex) {
            if (playerIndex < 0 || playerIndex > InputListener.MAX_CONTROLLER) {
                Debug.LogFormat("[GameTeam] AddInputListener failed. Player Index cannot be bigger than {0}, but given is {1}", InputListener.MAX_CONTROLLER, playerIndex);
                return;
            }

            // if exist.
            RemoveInputListener(playerIndex);

            var listener = new TeamInputListener(playerIndex, this);

            inputListeners[playerIndex] = listener;

            Debug.Log("[GameTeam] Input listener added to the team.");
        }

        public void RemoveInputListener(int playerIndex) {
            if (playerIndex < 0 || playerIndex > InputListener.MAX_CONTROLLER) {
                Debug.LogFormat("[GameTeam] RemoveInputListener failed. Player Index cannot be bigger than {0}, but given is {1}", InputListener.MAX_CONTROLLER, playerIndex);
                return;
            }

            var listener = inputListeners[playerIndex];
            if (listener != null) {
                listener.Clear();
            }

            inputListeners[playerIndex] = null;
        }

        public void ClearAllInputListeners() {
            for (int i = 0; i < InputListener.MAX_CONTROLLER; i++) {
                RemoveInputListener(i);
            }
        }

        private PlayerBase CreateBasePlayer(MatchPlayer matchPlayer) {
            PlayerBase basePlayer = null;

            switch (matchPlayer.Position) {
                case Positions.GK:
                    basePlayer = new GK(this, matchPlayer, gkKit);
                    break;

                case Positions.CB:
                    basePlayer = new CB(this, matchPlayer, mainKit);
                    break;

                case Positions.CB_R:
                    basePlayer = new CB_R(this, matchPlayer, mainKit);
                    break;

                case Positions.CB_L:
                    basePlayer = new CB_L(this, matchPlayer, mainKit);
                    break;

                case Positions.LB:
                    basePlayer = new LB(this, matchPlayer, mainKit);
                    break;

                case Positions.RB:
                    basePlayer = new RB(this, matchPlayer, mainKit);
                    break;

                case Positions.DMF:
                    basePlayer = new DMF(this, matchPlayer, mainKit);
                    break;

                case Positions.DMF_L:
                    basePlayer = new DMF_L(this, matchPlayer, mainKit);
                    break;

                case Positions.DMF_R:
                    basePlayer = new DMF_R(this, matchPlayer, mainKit);
                    break;

                case Positions.CM:
                    basePlayer = new CMF(this, matchPlayer, mainKit);
                    break;

                case Positions.CM_L:
                    basePlayer = new CMF_L(this, matchPlayer, mainKit);
                    break;

                case Positions.CM_R:
                    basePlayer = new CMF_R(this, matchPlayer, mainKit);
                    break;

                case Positions.LMF:
                    basePlayer = new LMF(this, matchPlayer, mainKit);
                    break;

                case Positions.RMF:
                    basePlayer = new RMF(this, matchPlayer, mainKit);
                    break;

                case Positions.AMF:
                    basePlayer = new AMF(this, matchPlayer, mainKit);
                    break;

                case Positions.AMF_L:
                    basePlayer = new AMF_L(this, matchPlayer, mainKit);
                    break;

                case Positions.AMF_R:
                    basePlayer = new AMF_R(this, matchPlayer, mainKit);
                    break;

                case Positions.LW:
                    basePlayer = new LW(this, matchPlayer, mainKit);
                    break;

                case Positions.RW:
                    basePlayer = new RW(this, matchPlayer, mainKit);
                    break;

                case Positions.ST:
                    basePlayer = new ST(this, matchPlayer, mainKit);
                    break;

                case Positions.ST_R:
                    basePlayer = new ST_R(this, matchPlayer, mainKit);
                    break;

                case Positions.ST_L:
                    basePlayer = new ST_L(this, matchPlayer, mainKit);
                    break;
            }

            return basePlayer;
        }

        public void SetTeam(MatchTeam team) {
            Debug.LogFormat("[GameTeam] Set Team to {0}", team.Team.TeamName);
            Team = team;

            tacticManager = MatchManager.Current.UserTeam == this ? 
                new UserManager() : 
                new AIManagerHandler(TeamId == 1, this, 
                TeamId == 1 ? MatchManager.Current.GameTeam1 : MatchManager.Current.GameTeam2);

            Clear();

            // create kits.
            var targetKit = !team.Kit ? team.Team.HomeKit : team.Team.AwayKit;

            mainKit = InGameKitMaterial.Current.GetMaterial(targetKit.KitMaterial, targetKit.Color1, targetKit.Color2);
            gkKit = InGameKitMaterial.Current.GetMaterial(targetKit.GKKitMaterial, targetKit.GKColor1, targetKit.GKColor2);

            var length = Mathf.Min(team.Players.Length, 11);

            GamePlayers = new PlayerBase[length];

            var positions = FormationRules.GetTeamFormation(team.Formation).Positions;

            for (int i = 0; i < length; i++) {
                var basePlayer = CreateBasePlayer(team.Players[i]);

                Debug.Log("Created player " + i);

                if (basePlayer != null) {
                    GamePlayers[i] = basePlayer;
                } else {
                    Debug.LogErrorFormat("Position {0} is not implemented.", positions[i]);
                }
            }
        }

        public void Clear() {
            if (GamePlayers != null) {
                foreach (PlayerBase player in GamePlayers) {
                    player.Dispose();
                }
            }

            GamePlayers = new PlayerBase[0];
        }

        public void Behave(
                in float time,
                in float deltaTime,
                in int fieldEndX,
                in int fieldEndY,
                in MatchStatus matchStatus,
                in TeamBehaviour tactics,
                in float teamDensityX,
                in float offsideLine,
                in float ourOffsideLine,
                Ball ball,
                GoalNet goalNet,
                GoalNet targetGoalNet,
                in PlayerBase[] opponents) {

            #region Input Listeners
            ///control free input listeners. give them a player to manage.
            for (int i = 0; i < InputListener.MAX_CONTROLLER; i++) {
                if (inputListeners[i] != null) {
                    // update input listener.
                    inputListeners[i].Update(in deltaTime);
                }
            }
            //
            #endregion

            TeamDensity = teamDensityX;

            tacticManager.Run();

            BallProgress = Mathf.Abs(goalNet.Position.x - ball.transform.position.x) / fieldEndX;

            // show & hide name.
            if (m_nextName < time) {
                m_nextName = time + NAME_TIMER_DELAYER;

                var orderByCloser = GamePlayers.
                    OrderBy(x => Vector3.Distance(x.Position, ball.BallPosition (x)));

                var showName = orderByCloser.Take(2);

                foreach (var player in showName) {
                    player.PlayerController.UI.ShowName(true);
                }

                var hideName = orderByCloser.Skip(2);

                foreach (var player in hideName) {
                    player.PlayerController.UI.ShowName (false);
                }

                UpdateMarkings(opponents);
            }

            for (int i=0; i < 11; i++) {
                if (GamePlayers[i] != null) {

                    var isInputControlled = IsPlayerInputControlled(GamePlayers[i]);

                    if (!GamePlayers[i].PlayerController.IsPhysicsEnabled) {
                        // player is not enabled atm.
                        GamePlayers[i].Stop(deltaTime / 4f); // stop slowly.
                    }

                    foreach (var behaviour in GamePlayers[i].Behaviours) {
                        behaviour.SetBehaviour(
                            GamePlayers[i],
                            in isInputControlled,
                            in time,
                            in deltaTime,
                            in fieldEndX,
                            in fieldEndY,
                            in matchStatus,
                            in tactics,
                            in offsideLine,
                            in ourOffsideLine,
                            ball,
                            goalNet,
                            targetGoalNet,
                            in GamePlayers,
                            in opponents);
                    };

                    GamePlayers[i].Behave(
                        in isInputControlled,
                        in time,
                        in deltaTime,
                        in fieldEndX,
                        in fieldEndY,
                        in matchStatus,
                        in tactics,
                        in offsideLine,
                        ball,
                        goalNet,
                        targetGoalNet,
                        in GamePlayers,
                        in opponents);

                    if (!GamePlayers[i].PlayerController.IsPhysicsEnabled || GamePlayers[i].ballHitAnimationEvent != BallHitAnimationEvent.None) {
                        continue; // ignore cases.
                    }

                    if (MatchManager.Current.MatchFlags == MatchStatus.Freeze) {
                        GamePlayers[i].Stop(in deltaTime);
                        continue;
                    }

                    if (!GamePlayers[i].IsHoldingBall && !MatchManager.CanIMove(in tactics)) {
                        GamePlayers[i].Stop(deltaTime);
                        continue;
                    }

                    GamePlayers[i].ProcessBehaviours(in time);
                } else {
                    Debug.LogError("Player is null.");
                }
            }
        }

        private void lookToBall(in Vector3 ballPosition, PlayerBase player) {
            var lookToBall = Quaternion.LookRotation(ballPosition - player.Position);
            lookToBall.eulerAngles = new Vector3(0, lookToBall.eulerAngles.y, 0);
            player.PlayerController.SetInstantRotation(lookToBall);
        }

        private void PutPlayerToFieldPosition (
            PlayerBase player, 
            FieldPosition fieldPosition, 
            GoalNet goalNet, 
            in int fieldEndX, 
            in int fieldEndY,
            in Vector3 ballPosition) {

            player.InstantStop();

            var worldPosition = FieldPositionUtility.PositionToVector3(goalNet.Direction, in fieldEndX, in fieldEndY, fieldPosition);
            player.PlayerController.SetInstantPosition (worldPosition);

            Debug.Log($"Player prepositioned: {player.MatchPlayer.Position} to {worldPosition}");

            lookToBall(ballPosition, player);
        }


        public void CornerPrePosition (
            PlayerBase cornerKicker,
            IFieldPositioning basePositioning,
            int fieldEndX,
            int fieldEndY,
            GoalNet goalNet,
            Ball ball){

            float distanceToEnd(FieldPosition fieldPosition) {
                var worldPosition = FieldPositionUtility.PositionToVector3(goalNet.Direction, fieldEndX, fieldEndY, fieldPosition);

                return goalNet.Direction.x > 0 ? worldPosition.x : fieldEndY - worldPosition.x;
            }

            const int cornerKeyPlayerCount = 5;

            // max 9 player.
            var orderPlayersByHeight = GamePlayers.Where (x=> x!= cornerKicker && !x.IsGK).OrderByDescending (x => x.MatchPlayer.Player.height);

            var orderPositionsToGoalNetDistance = 
                basePositioning.FieldPositions.Where (x=>x.Position != Positions.GK).
                OrderByDescending(x => distanceToEnd(x));

            // longest player is comes for header.

            var scorerPositions = orderPositionsToGoalNetDistance.Take(cornerKeyPlayerCount).OrderBy (x=>System.Guid.NewGuid ()); // 5 scorers.

            var ballPosition = ball.transform.position;

            int playerIndex = 0;

            // draw scorers.
            foreach (var scorerPosition in scorerPositions) {
                var player = orderPlayersByHeight.ElementAt(playerIndex++);
                PutPlayerToFieldPosition(player, scorerPosition, goalNet, fieldEndX, fieldEndY, ballPosition);
            }

            // draw rest.
            var restPositions = orderPositionsToGoalNetDistance.Skip(cornerKeyPlayerCount).OrderBy(x => System.Guid.NewGuid());
            foreach (var restPosition in restPositions) {
                if (playerIndex >= orderPlayersByHeight.Count ()) {
                    break;
                }

                var player = orderPlayersByHeight.ElementAt(playerIndex++);
                PutPlayerToFieldPosition(player, restPosition, goalNet, fieldEndX, fieldEndY, ballPosition);
            }

            // reorder GK.
            var gk = GamePlayers.Where(x => x.IsGK).FirstOrDefault();
            var gkFieldPos = basePositioning.FieldPositions.FirstOrDefault(x => x.Position == Positions.GK);
            PutPlayerToFieldPosition(gk, gkFieldPos, goalNet, fieldEndX, fieldEndY, ballPosition);

            if (cornerKicker != null) {
                // reorder cornerKicker
                var dir = (goalNet.Position - ballPosition).normalized;

                cornerKicker.InstantStop();
                cornerKicker.PlayerController.SetInstantPosition (ballPosition - dir);
                cornerKicker.PlayerController.SetInstantRotation (Quaternion.LookRotation (dir));
            }
        }

        // Reposition the team players for the given positioning details.
        public void PrePositionDefensiveKickOff(
            in int fieldEndX,
            in int fieldEndY,
            GoalNet goalNet) {

            Debug.Log("[GameTeam] Preposition for non kickoff");

            int skip = 0;

            var orderedBasePositions = KickOffPositioning.Current.FieldPositions.OrderBy(x => x.VerticalPlacement).ToArray();

            var playersWithPositions = GamePlayers.Select(x => (x, FormalPositioning.Current.GetPosition(x.MatchPlayer.Position))).OrderBy(x => x.Item2.VerticalPlacement).ToArray();

            var ballPosition = Ball.Current.transform.position;

            foreach (var player in playersWithPositions) {
                var targetPosition = orderedBasePositions[skip++];
                PutPlayerToFieldPosition(player.x, targetPosition, goalNet, fieldEndX, fieldEndY, ballPosition);
            }
        }

        public void PrePositionForOffensiveKickOff(
            int fieldEndX,
            int fieldEndY,
            GoalNet goalNet,
            Vector3 kickerPositioningOffset = default,
            Positions excludePosition = Positions.ParametersCount) {

            Debug.Log("[GameTeam] Preposition for offensive kickoff");

            int skip = 0;

            var orderedBasePositions = KickOffPositioning_Starter.Current.FieldPositions.OrderBy(x => x.VerticalPlacement).ToArray();

            var playersWithPositions = GamePlayers.Select(x => (x, FormalPositioning.Current.GetPosition(x.MatchPlayer.Position))).OrderBy (x=>x.Item2.VerticalPlacement).ToArray();

            var ballPosition = Ball.Current.transform.position;

            foreach (var player in playersWithPositions) {
                var targetPosition = orderedBasePositions[skip++];
                PutPlayerToFieldPosition(player.x, targetPosition, goalNet, fieldEndX, fieldEndY, ballPosition);
            }

            KickOffPlayer(ballPosition, kickerPositioningOffset, excludePosition);
        }

        private void KickOffPlayer(
            Vector3 ballPosition,
            Vector3 kickerPositioningOffset = default,
            Positions excludePosition = Positions.ParametersCount) {

            // put our closest player to the center.
            var sortedByDistanceToBall = GamePlayers.
            Where(x => x.MatchPlayer.Position != excludePosition).
            OrderBy(x => Vector3.Distance(x.Position, ballPosition));

            if (sortedByDistanceToBall.Count() > 0) {
                var closest = sortedByDistanceToBall.First();

                closest.PlayerController.SetInstantPosition(ballPosition + kickerPositioningOffset);
                lookToBall(ballPosition, closest);

                // hold the ball.
                Ball.Current.Hold(closest);
            }
        }

        /// <summary>
        /// Keep the current behaviours for x seconds.
        /// </summary>
        /// <param name="seconds"></param>
        public void KeepPlayerBehavioursForAShortTime () {
            float time = Time.time + 0.1f; 
            
            foreach (var player in GamePlayers) {
                player.NextBehaviour = time;
            }
        }

        private void UpdateMarkings(in PlayerBase[] opponents) {
            foreach (var player in GamePlayers) {
                player.MarkingTarget = null; // reset first.
            }

            foreach (var player in opponents) {
                player.Markers.Clear(); // reset first.
            }

            var markableOpponents = opponents.Where (x=>!x.IsGK && x.PlayerController.IsPhysicsEnabled);
            var markers = GamePlayers.Where (x=>x.PlayerController.IsPhysicsEnabled);

            var orderedOpponenets = markableOpponents.OrderByDescending(x => x.PlayerFieldProgress);

            foreach (var opponent in orderedOpponenets) {
                var myPosition = opponent.Position;
                var bestMarkers = BestOptionsToTargetPlayer (
                     opponent,
                     markers,
                     BallProgress,
                4); // max 4.

                foreach (var marker in bestMarkers) {
                    opponent.Markers.Add(marker);
                }

                foreach (var marker in bestMarkers) {
                    if (marker.MarkingTarget == null) {
                        marker.MarkingTarget = opponent;
                        break;
                    }
                }
            }

            // include GK.
            var opponentGK = opponents.FirstOrDefault (x=>x.IsGK);
            foreach (var marker in markers) {
                marker.Markers.Add (opponentGK);
            }
        }
    }
}
