using UnityEngine;

using System.Linq;

using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Graphics;
using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.Utilities;
using FStudio.MatchEngine.Players;
using FStudio.Animation;
using FStudio.MatchEngine.Cameras;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;

using Random = UnityEngine.Random;

using System;
using FStudio.MatchEngine.Players.Referee;
using System.Threading.Tasks;
using Shared.Responses;
using System.Collections.Generic;
using FStudio.Data;

using FStudio.MatchEngine.Statistics;
using FStudio.MatchEngine.Players.PlayerController;
using MatchEngine.MatchScenes;
using UnityEngine.Scripting;
using FStudio.UI.Events;
using FStudio.Database;
using FStudio.UI.MatchThemes.MatchEvents;
using FStudio.MatchEngine.Input;

namespace FStudio.MatchEngine {
    public class MatchManager : SceneObjectSingleton<MatchManager> {

        [Header("Field Settings")]
        public int fieldEndX = default;
        public int fieldEndY = default;

        public class MatchDetails {
            public TeamEntry homeTeam;
            public TeamEntry awayTeam;
            public bool homeKitSelection;
            public bool awayKitSelection;

            public UpcomingMatchEvent matchEvent;

            public KitEntry HomeKit => !homeKitSelection ? homeTeam.HomeKit : homeTeam.AwayKit;
            public KitEntry AwayKit => !awayKitSelection ? awayTeam.HomeKit : awayTeam.AwayKit;

            public MatchDetails(
                UpcomingMatchEvent matchEvent,
                bool homeKit,
                bool awayKit) {
                this.matchEvent = matchEvent;
                this.homeTeam = matchEvent.details.homeTeam;
                this.awayTeam = matchEvent.details.awayTeam;
                this.homeKitSelection = homeKit;
                this.awayKitSelection = awayKit;
            }
        }

        #region current match detials
        public static MatchDetails CurrentMatchDetails { private set; get; }
        #endregion

        public static StatisticsManager Statistics { private set; get; }

        /// <summary>
        /// All footballers in the match.
        /// </summary>
        public static IEnumerable<PlayerBase> AllPlayers;

        public int homeTeamScore;
        public int awayTeamScore;
        public Vector2 SizeOfField => new Vector2(fieldEndX, fieldEndY);

        private int refereeWhistleForKickOffDelay = 1000;
        private int kickOffAfterMilliSecs = 1000;

        [SerializeField] private float matchSpeed = 0.5f;

        private float m_minutes;
        public float minutes {
            get {
                return m_minutes;
            }

            private set {
                m_minutes = value;
                EventManager.Trigger(new GameTimeEvent(value));
            }
        }

        public GoalNet goalNet1 = default;
        public GoalNet goalNet2 = default;

        private MatchSceneManager matchSceneManager;

        [SerializeField] private Transform offsideLine1 = default;
        [SerializeField] private Transform offsideLine2 = default;
        [SerializeField] private Transform densityPoint1 = default;
        [SerializeField] private Transform densityPoint2 = default;

        [SerializeField] private Transform[] cornerSpots = default;
        [SerializeField] private Transform[] goalKickSpots = default;

        [SerializeField] private BoxCollider goal1SpotCollider = default;
        [SerializeField] private BoxCollider goal2SpotCollider = default;

        public Transform[] celebrationPoints;

        public PhysicMaterial PlayerColliderMaterial;

        public GameTeam GameTeam1 = default;
        public GameTeam GameTeam2 = default;
        public GameTeam UserTeam { private set; get; }

        private Rect goalNet1Rect;
        private Rect goalNet2Rect;

        public AbstractReferee[] Referees { private set; get; }

        [SerializeField] private Ball ball = default;

        public MatchStatus MatchFlags = default;

        private GeneralUserInput generalInput;

        /// <summary>
        /// Which team started, team1 or team2?
        /// </summary>
        public int whichTeamStarted = default;

        /// <summary>
        /// First half, second half, extra time etc.
        /// </summary>
        private int halfIndex = default;

        protected override void OnEnable() {
            base.OnEnable();

            EventManager.Subscribe<KickOffEvent>(KickOff);
            EventManager.Subscribe<ThrowInEvent>(OnThrowIn);
            EventManager.Subscribe<OutEvent>(OnCorner);
        }

        private void OnDisable() {
            EventManager.UnSubscribe<KickOffEvent>(KickOff);
            EventManager.UnSubscribe<ThrowInEvent>(OnThrowIn);
            EventManager.UnSubscribe<OutEvent>(OnCorner);
        }

        [Preserve]
        private void OnDestroy() {
            if (matchSceneManager != null) {
                matchSceneManager.Dispose();
                matchSceneManager = null;
            }

            if (Statistics != null) {
                Statistics.Dispose();
                Statistics = null;
            }
        }

        /// <summary>
        /// Can player move?
        /// </summary>
        /// <param name="tactics"></param>
        /// <returns></returns>
        public static bool CanIMove(in TeamBehaviour tactics) {
            if (!Current.MatchFlags.HasFlag(MatchStatus.Playing)) {
                return false;
            }

            switch (tactics) {
                case TeamBehaviour.OpponentThrowIn:
                case TeamBehaviour.TeamThrowIn:
                    return false;
            }

            return true;
        }

        private void OnThrowIn(ThrowInEvent throwInEvent) {
            var ballPosition = throwInEvent.Position;

            // clamp.
            ballPosition.z = Mathf.Clamp(ballPosition.z, 0, fieldEndY);

            Foul(FoulType.ThrowIn, ballPosition, ball.LastTouchedPlayer.GameTeam == GameTeam1 ? GameTeam2 : GameTeam1, FStudio.Data.Positions.GK);
        }

        private void OnCorner(OutEvent outEvent) {
            int outIndex = outEvent.Part;

            if (outIndex < 2) {
                if (ball.LastTouchedPlayer.GameTeam == GameTeam2) {
                    GoalKick(outIndex);
                } else {
                    Corner(outIndex);
                }

                return;
            } else if (outIndex >= 2) {
                if (ball.LastTouchedPlayer.GameTeam == GameTeam1) {
                    GoalKick(outIndex);
                } else {
                    Corner(outIndex);
                }

                return;
            } else {
                Debug.LogErrorFormat("[MatchManager] Invalid out index {0}", outIndex);
            }
        }

        void OnDrawGizmos() {
            Gizmos.DrawWireCube(
                new Vector3(goalNet1Rect.position.x, goalNet1Rect.height / 2f, goalNet1Rect.position.y),
                new Vector3(1, goalNet1Rect.height, goalNet1Rect.width));

            Gizmos.DrawWireCube(
                new Vector3(goalNet2Rect.position.x, goalNet2Rect.height / 2f, goalNet2Rect.position.y),
                new Vector3(1, goalNet2Rect.height, goalNet2Rect.width));
        }

        public bool IsInGoalSpot(Vector3 position, int teamId) {
            BoxCollider goalNet;
            if (teamId == 0) {
                goalNet = goal1SpotCollider;
            } else {
                goalNet = goal2SpotCollider;
            }

            return goalNet.bounds.Contains(position);
        }

        public static void ClearMatchAssets() {
            if (Current.Referees != null) {
                foreach (var referee in Current.Referees) {
                    Destroy(referee.PlayerController.UnityObject);
                }

                Current.Referees = null;
            }
        }

        /// <summary>
        /// Loads a 3D Match with the given teams.
        /// <para>Parameters should be directly received 
        /// from the backend as this function can only takes shared responses.</para>
        /// <para>This function will load the match and start the UI events like squad presentations.</para>
        /// </summary>
        /// <param name="homeTeam"></param>
        /// <param name="awayTeam"></param>
        /// <param name="homePlayers"></param>
        /// <param name="awayPlayers"></param>
        /// <param name="homeSquad"></param>
        /// <param name="awaySquad"></param>
        /// <param name="homeKit"></param>
        /// <param name="awayKit"></param>
        /// <param name="stadiumAssetIndex">Index of the stadium asset on StadiumCollection asset loader</param>
        /// <param name="ballAssetIndex">Index of the ball asset on BallCollection asset loader</param>
        /// <returns></returns>
        public static async Task CreateMatch(
                MatchDetails matchDetails
            ) {

            CurrentMatchDetails = matchDetails;

            var homeFormation = FormationRules.GetTeamFormation(matchDetails.homeTeam.Formation);

            var homeTeamMatchPlayers = new MatchPlayer[11];
            for (int i = 0; i < 11; i++) {
                homeTeamMatchPlayers[i] = new MatchPlayer(
                    i + 1,
                    matchDetails.homeTeam.Players[i],
                    11 > i ? homeFormation.Positions[i] : matchDetails.homeTeam.Players[i].Position);
            }

            var homeTactics = await PlayableFormations.
                Current.
                Formations.
                FindAsync(matchDetails.homeTeam.Formation);

            var details = matchDetails.matchEvent.details;

            var homeMatchTeam = new MatchTeam() {
                Players = homeTeamMatchPlayers,
                Team = matchDetails.homeTeam,
                Formation = matchDetails.homeTeam.Formation,
                TeamTactics = homeTactics,
                Kit = matchDetails.homeKitSelection,
                AILevel = details.userTeam == MatchCreateRequest.UserTeam.Home ? AILevel.Legendary : details.aiLevel
            };

            var awayFormation = FormationRules.GetTeamFormation(matchDetails.awayTeam.Formation);

            var awayTeamMatchPlayers = new MatchPlayer[11];

            for (int i = 0; i < 11; i++) {
                awayTeamMatchPlayers[i] = new MatchPlayer(
                    i + 1,
                    matchDetails.awayTeam.Players[i],
                    11 > i ? awayFormation.Positions[i] : matchDetails.awayTeam.Players[i].Position);
            }

            var awayTactics = await PlayableFormations.Current.Formations.FindAsync(matchDetails.awayTeam.Formation);

            var awayMatchTeam = new MatchTeam() {
                Players = awayTeamMatchPlayers,
                Team = matchDetails.awayTeam,
                Formation = matchDetails.awayTeam.Formation,
                TeamTactics = awayTactics,
                Kit = matchDetails.awayKitSelection,
                AILevel = details.userTeam == MatchCreateRequest.UserTeam.Away ? AILevel.Legendary : details.aiLevel
            };
            //

            EventManager.Trigger(new MatchDetailsEvent(matchDetails));

            Current.generalInput = new GeneralUserInput("MatchEngine" , 0);

            switch (details.userTeam) {
                case MatchCreateRequest.UserTeam.Home:
                    Current.UserTeam = Current.GameTeam1;
                    Current.GameTeam1.AddInputListener(0);
                    break;

                case MatchCreateRequest.UserTeam.Away:
                    Current.UserTeam = Current.GameTeam2;
                    Current.GameTeam2.AddInputListener(0);
                    break;

                default: Current.UserTeam = null; break;
            }

            await Current.CreateMatch(homeMatchTeam, awayMatchTeam);

            // set default tactic.
            if (Current.UserTeam != null) {
                var defaultUserTactic = TacticPresetTypes.Balanced;
                Current.UserTeam.Team.TacticPresetType = defaultUserTactic;
                EventManager.Trigger(new TeamChangedTactic(Current.UserTeam, defaultUserTactic));
            }
            //

            GameInput.SwitchToMatchEngine();
        }

        private async void StartKickoffCounter() {
            await Task.Delay(refereeWhistleForKickOffDelay);

            EventManager.Trigger(new RefereeShortWhistleEvent());
            EventManager.Trigger(new ShowScoreboardEvent());

            await Task.Delay(kickOffAfterMilliSecs);

            if (minutes == 0) {
                EventManager.Trigger(new FirstWhistleEvent());
            }

            EventManager.Trigger(new KickOffEvent());
        }

        private async Task CreateMatch (
            MatchTeam team1, 
            MatchTeam team2) {

            minutes = 0;

            GameTeam1.SetTeam(team1);
            GameTeam2.SetTeam(team2);

            // set all players.
            AllPlayers = GameTeam1.GamePlayers.Concat(GameTeam2.GamePlayers);

            // create referee.
            await CreateReferees();

            RandomizeKickOff();

            OriginateAll();

            var midPoint = new Vector3(fieldEndX / 2f, 0, fieldEndY / 2f);

            ball.ResetBall(midPoint);

            // build statistics.
            Statistics = new StatisticsManager(team1.Players.Concat (team2.Players).Select (x=>x.Player.id).ToArray (), ball);

            matchSceneManager = new MatchSceneManager();

            EventManager.Trigger(new MatchInitializationCompletedEvent());

            PrePositionTeamsForKickOff(); // preposition.

            CameraSystem.Current.FocusToBall();
        }

        public PlayerBase GoalScorer(int scorerTeamId) {
            var ball = Ball.Current;
            var scorer = ball.HolderPlayer != null ? ball.HolderPlayer : ball.LastHolder;
            if (scorer == null || scorer.GameTeam.TeamId != scorerTeamId) {
                scorer = ball.LastTouchedPlayer;

                if (scorer == null || scorer.GameTeam.TeamId != scorerTeamId) {
                    var ballPos = ball.transform.position;
                    var gameTeam = scorerTeamId == GameTeam1.TeamId ? GameTeam1 : GameTeam2;

                    scorer = gameTeam.GamePlayers.OrderBy(x => Vector3.Distance(x.Position, ballPos)).FirstOrDefault();
                }
            }

            return scorer;
        }

        private void OriginateAll() {
            var all = AllPlayers;

            const float betweenPlayersDistance = 0.75f;
            const float yDivider = 3;

            var placementPos = new Vector3(fieldEndX / 2 - betweenPlayersDistance * all.Count() / 2f + betweenPlayersDistance, 0, fieldEndY / yDivider);
            var placementRot = Quaternion.LookRotation(-Vector3.forward);

            foreach (var player in all.Concat(Referees)) {
                player.PlayerController.SetInstantPosition(placementPos);
                player.PlayerController.SetInstantRotation(placementRot);

                player.InstantStop();

                placementPos += new Vector3(betweenPlayersDistance, 0, 0);
            }
        }

        public void SetGoalColliders(bool value) {
            goalNet1.GoalColliders.SetActive(value);
            goalNet2.GoalColliders.SetActive(value);
        }

        public void SetOutColliders(bool value) {
            goalNet1.OutColliders.SetActive(value);
            goalNet2.OutColliders.SetActive(value);
        }

        public void ClearMatch () {
            MatchFlags &= ~MatchFlags;

            Current.generalInput.Clear();

            ClearMatchAssets();

            ResetMatchState();
        }

        public void ResetMatchState () {
            Debug.Log("[MatchManager] ResetMatchState ()");
            GameTeam1.Clear();
            GameTeam2.Clear();

            var midPoint = new Vector3(fieldEndX / 2f, 0, fieldEndY / 2f);

            ball.ResetBall(midPoint);

            MatchFlags = MatchStatus.NotPlaying;
        }

        private async Task CreateReferees () {
            Referees = new AbstractReferee[3];

            var kitMaterial = await InGameKitMaterial.Current.GetRefereeMaterial();

            MatchPlayer createRef () {
                var player = ScriptableObject.CreateInstance<PlayerEntry>();

                player.topSpeed = 70;
                player.acceleration = 70;
                player.agility = 50;

                player.height = Random.Range (170, 185);
                player.weight = Random.Range (65, 95);

                return new MatchPlayer(0, player, Positions.ST);
            }

            Referees[0] = new LinemanReferee(createRef (), null, kitMaterial);
            Referees[1] = new MiddleReferee(createRef (), null, kitMaterial);
            Referees[2] = new LinemanReferee(createRef (), null, kitMaterial) { Side = true };
        }

        /// <summary>
        /// Start the current period.
        /// </summary>
        private void KickOff (KickOffEvent _) {
            MatchFlags = MatchStatus.WaitingForKickOff;
        }

        private void RandomizeKickOff () {
            whichTeamStarted = Random.Range(1, 3);
            halfIndex = 1; // 1 first half, 2 second half.
        }

        public void PrePositionTeamsForKickOff () {
            Debug.Log("[MatchManager] PrePositionTeamsForKickOff");

            foreach (var referee in Referees) {
                referee.InstantStop();
            }

            // put referee to middle.
            var midPoint = new Vector3(fieldEndX / 2f, 0, fieldEndY / 2f + 10);
            Referees[1].PlayerController.SetInstantPosition(midPoint);
            Referees[1].PlayerController.SetInstantRotation (Quaternion.LookRotation(ball.transform.position - midPoint));
            //

            // put linesman referees
            Referees[0].PlayerController.SetInstantPosition (new Vector3(fieldEndX/2, 0, 0));
            Referees[0].PlayerController.SetInstantRotation (Quaternion.LookRotation(Vector3.forward));

            Referees[2].PlayerController.SetInstantPosition(new Vector3(fieldEndX / 2, 0, fieldEndY));
            Referees[2].PlayerController.SetInstantRotation (Quaternion.LookRotation(-Vector3.forward));
            //

            var homeTeamStart = halfIndex % 2 == whichTeamStarted;

            if (homeTeamStart) {
                GameTeam1.PrePositionForOffensiveKickOff(
                    fieldEndX,
                    fieldEndY,
                    goalNet1,
                    Vector3.forward,
                    Positions.GK);

                GameTeam2.PrePositionDefensiveKickOff(
                     in fieldEndX,
                     in fieldEndY,
                     goalNet2);
            } else {
                GameTeam1.PrePositionDefensiveKickOff(
                    in fieldEndX,
                    in fieldEndY,
                    goalNet1);

                GameTeam2.PrePositionForOffensiveKickOff (
                     fieldEndX,
                     fieldEndY,
                     goalNet2,
                     Vector3.forward,
                     Positions.GK);
            }
            
            foreach (var player in AllPlayers) {
                player.PlayerController.IsPhysicsEnabled = true;
            }

            StartKickoffCounter();
        }

        public static TeamBehaviour GetTeamEvent (GameTeam gameTeam) {
            TeamBehaviour tactics;

            var ball = Current.ball;

#region GK Condition
            if (ball.HolderPlayer?.IsGKUntouchable == true) {
                if (gameTeam == ball.HolderTeam) {
                    tactics = TeamBehaviour.WaitingForTeamGK;
                } else {
                    tactics = TeamBehaviour.WaitingForOpponentGK;
                }
            }
#endregion
            else if (ball.HolderTeam == gameTeam) {
                tactics = TeamBehaviour.Attacking;
            } else if (ball.HolderTeam == null) {
                tactics = TeamBehaviour.BallChasing;
            } else {
                tactics = TeamBehaviour.Defending;
            }

            if (tactics == TeamBehaviour.BallChasing) {
                if (ball.LastHolder != null) {
                    if (ball.LastHolder.GameTeam == gameTeam) {
                        tactics = TeamBehaviour.Attacking;
                    } else {
                        tactics = TeamBehaviour.Defending;
                    }
                }
            }

            return tactics;
        }

        private void LateUpdate() {
            if (matchSceneManager != null) {
                var result = matchSceneManager.UpdateScenes();
                if (result == ESceneResult.BlockLogic) {
                    // ignore rest
                    return;
                }
            }

            if (MatchFlags.HasFlag (MatchStatus.NotPlaying)) {
                return;
            }
            
            if (!MatchFlags.HasFlag(MatchStatus.Freeze)) {
                var midPoint = fieldEndX / 2f;
                var ballPosX = ball.transform.position.x;
                // calculate offside lines for both teams.
                CalculateOffsideLine(midPoint, ballPosX, offsideLine1, goalNet1, GameTeam1.GamePlayers);
                CalculateOffsideLine(midPoint, ballPosX, offsideLine2, goalNet2, GameTeam2.GamePlayers);
                //
            }
            
            float deltaTime = Time.deltaTime;
            float time = Time.time;

            if (MatchFlags == MatchStatus.Playing) {
                bool matchShouldContinue = minutes < 90;

                if (!matchShouldContinue) {
                    var ballPosition = Ball.Current.transform.position;
                    float size = Current.fieldEndX / 2.25f;
                    if (ballPosition.x > size && ballPosition.x < Current.fieldEndX - size) {
                        minutes = 0;

                        Debug.Log("[MatchManager] FinalWhistle");

                        EventManager.Trigger(new RefereeLastWhistleEvent());

                        EventManager.Trigger(new FinalWhistleEvent(GameTeam1.Team.Team, GameTeam2.Team.Team));

                        // game over.
                        MatchFlags = MatchStatus.Special;

                        // stop all players.
                        foreach (var player in GameTeam1.GamePlayers.Concat (GameTeam2.GamePlayers).Concat (Referees)) {
                            player.PlayerController.Animator.SetFloat(PlayerAnimatorVariable.Horizontal, 0);
                            player.PlayerController.Animator.SetFloat(PlayerAnimatorVariable.Vertical, 0);
                        }
                        //

                        // Stop following.
                        CameraSystem.Current.target = null;

                        return;
                    }
                } else {
                    minutes = Mathf.Min (90, minutes + deltaTime * matchSpeed);
                }
            }

            var team1Tactic = GetTeamEvent(GameTeam1);
            var team2Tactic = GetTeamEvent(GameTeam2);

            //
            // Calculate density points.
            //
            float calculateDensity (GameTeam gameTeam) {
                float totalX = 0;

                var allPlayers = gameTeam.GamePlayers.Where(x => !x.IsGK);

                foreach (var player in allPlayers) {
                    totalX += player.Position.x;
                }

                return totalX / gameTeam.GamePlayers.Length;
            }

            var ballPositionX = ball.transform.position.x;

            var team1Density = Mathf.Lerp (calculateDensity(GameTeam1), ballPositionX, 0.5f);
            var team2Density = Mathf.Lerp (calculateDensity(GameTeam2), ballPositionX, 0.5f);

            densityPoint1.position = new Vector3(team1Density, 0, SizeOfField.y / 2);
            densityPoint2.position = new Vector3(team2Density, 0, SizeOfField.y / 2);
            //

            GameTeam1.Behave(
                in time,
                in deltaTime,
                in fieldEndX,
                in fieldEndY,
                in MatchFlags,
                in team1Tactic,
                in team1Density,
                offsideLine2.position.x,
                offsideLine1.position.x,
                ball,
                goalNet1,
                goalNet2,
                in GameTeam2.GamePlayers);

            GameTeam2.Behave(
                in time,
                in deltaTime,
                in fieldEndX,
                in fieldEndY,
                in MatchFlags,
                in team2Tactic,
                in team2Density,
                offsideLine1.position.x,
                offsideLine2.position.x,
                ball,
                goalNet2,
                goalNet1,
                in GameTeam1.GamePlayers);

            if (Referees != null) {
                foreach (var referee in Referees) {
                    referee.RefereeBehave(
                        in fieldEndX, 
                        in fieldEndY, 
                        in MatchFlags, 
                        in deltaTime, 
                        offsideLine2.position.x,
                        offsideLine1.position.x, 
                        ball);
                }
            }
        }

        private void FixedUpdate() {
            if (!MatchFlags.HasFlag (MatchStatus.Playing)) {
                return;
            }

            if (Statistics != null) {

                Statistics.Update();
            } else {
                Debug.LogError("Statistics is null");
            }
        }

        /// <summary>
        /// Find the last second guy to reposition offside line.
        /// TODO MIDDLE CHECK
        /// </summary>
        /// <param name="offsideLine"></param>
        /// <param name="players"></param>
        private void CalculateOffsideLine (float midPointX, float ballPosX, Transform offsideLine, GoalNet goalNet, in PlayerBase[] players) {
            var goalNetX = goalNet.Position.x;

            if (players.Length < 2) {
                return;
            }

            var lineDecider = players.OrderBy(x => Mathf.Abs(x.Position.x - goalNetX)).Take (2).ElementAt (1);

            var position = offsideLine.position;
            position.x = lineDecider.Position.x;

            var lineDeciderDistance = Mathf.Abs(lineDecider.Position.x - goalNetX);
            var ballDistance = Mathf.Abs(ballPosX - goalNetX);
            var midPointDistance = Mathf.Abs(midPointX - goalNetX);

            if (ballDistance > midPointDistance) {
                ballDistance = midPointDistance;
                ballPosX = midPointX;
            }

            // check if the ball is more closer to the goalNet.
            if (ballDistance < lineDeciderDistance) {
                position.x = ballPosX;
            }

            position.x = Mathf.Clamp(position.x, 0, fieldEndX);

            offsideLine.position = position;
        }

        public void AssignOffsides(GameTeam gameTeam, PlayerBase except) {
            foreach (var player in gameTeam.GamePlayers) {
                if (player != except && player.IsInOffside) {
                    player.CaughtInOffside = true;
                }
            }
        }

        public void WaitForMoment(float seconds, Action onCompleted) {
            new TimerAction(seconds).GetQuery().Start(this, onCompleted);
        }

        /// <summary>
        /// Reset offside caughts.
        /// </summary>
        public void ResetOffsides () {
            void checkTeam(GameTeam gameTeam) {
                foreach (var player in gameTeam.GamePlayers) {
                    player.CaughtInOffside = false;
                }
            }

            checkTeam(GameTeam1);
            checkTeam(GameTeam2);
        }

        /// <summary>
        /// Foul. Corner is different. This is for offside, normal foul, goalkick etc.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameTeam">which team will use it.</param>
        public void Foul(
            FoulType foulType,
            Vector3 position,
            GameTeam gameTeam,
            Positions excludeKickerPosition,
            Action onCameraTransition = null) {

            Debug.Log($"[Foul] {foulType}");
            MatchFlags = MatchStatus.Freeze;

            if (foulType == FoulType.GoalKick) {
                SetOutColliders(true);
            }

            EventManager.Trigger(new RefereeShortWhistleEvent());

            ResetOffsides();

            WaitForMoment(1, runFunction);

            void runFunction () {
                // start transition.
                CameraTransition.Current.StartTransition(() => {
                    new TimerAction(1).GetQuery().Start(this, () => {
                        MatchFlags = MatchStatus.WaitingForKickOff;
                    });
                });

                onCameraTransition?.Invoke();

                if (foulType == FoulType.GoalKick) {
                    SetOutColliders(false);
                }

                ball.ResetBall(position);


                var holderTeamIsTheFirstTeam = gameTeam == GameTeam1;

                if (holderTeamIsTheFirstTeam) {
                    reOrderTeam(GameTeam1, goalNet1, goalNet2, offsideLine2.position.x, true);
                    reOrderTeam(GameTeam2, goalNet2, goalNet1, offsideLine1.position.x, false);
                } else {
                    reOrderTeam(GameTeam2, goalNet2, goalNet1, offsideLine1.position.x, true);
                    reOrderTeam(GameTeam1, goalNet1, goalNet2, offsideLine2.position.x, false);
                }

                Statistics.ResetPositions();

                // reorder players.
                void reOrderTeam(
                    GameTeam team,
                    GoalNet goalNet,
                    GoalNet targetGoalNet,
                    in float offsideLine,
                    bool hasTheBall) {
                    TeamBehaviour teamBehaviour;

                    switch (foulType) {
                        case FoulType.ThrowIn: 
                            teamBehaviour = !hasTheBall ? TeamBehaviour.OpponentThrowIn : TeamBehaviour.TeamThrowIn; 
                            break;
                        case FoulType.GoalKick: 
                            teamBehaviour = !hasTheBall ? TeamBehaviour.WaitingForOpponentGK : TeamBehaviour.WaitingForTeamGK; 
                            break;
                        default: 
                            teamBehaviour = default;
                            break;
                    }

                    Debug.Log("Reorder team with behaviour: " + teamBehaviour);

                    var fieldSize = Current.SizeOfField;

                    void keepInField (ref Vector3 pos, float fielder) {
                        pos.x = Mathf.Clamp(pos.x, fielder, fieldSize.x - fielder);
                        pos.z = Mathf.Clamp(pos.z, fielder, fieldSize.y - fielder);
                    }

                    foreach (var player in team.GamePlayers) {
                        var tPos = player.GetFieldPosition(
                            ball.HolderPlayer?.GameTeam == player.GameTeam,
                            teamBehaviour,
                            in fieldEndX,
                            in fieldEndY,
                            position,
                            null,
                            offsideLine,
                            goalNet,
                            targetGoalNet);

                        // clamp tPos
                        keepInField(ref tPos, 2);

                        var rotation = Quaternion.LookRotation(position - tPos);
                        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);

                        player.PlayerController.SetInstantRotation(rotation);
                        player.PlayerController.SetInstantPosition(tPos);

                        player.InstantStop();
                    }

                    PlayerBase closest = null;

                    if (hasTheBall) {
                        // closest one to the ball.
                        closest = team.GamePlayers.Where(x => !x.MatchPlayer.Position.HasFlag(excludeKickerPosition)).
                            OrderBy(x => Vector3.Distance(x.Position, position)).First();

                        keepInField(ref position, 0.25f); // clamp ball pos.

                        switch (foulType) {
                            case FoulType.ThrowIn:
                                closest.IsThrowHolder = true;
                                closest.PlayerController.Animator.SetBool (
                                    PlayerAnimatorVariable.ThrowInIdle, true);

                                closest.PlayerController.SetInstantPosition(position);

                                break;

                            case FoulType.GoalKick:
                                closest.IsGoalKickHolder = true;

                                closest.PlayerController.SetInstantPosition(position - (targetGoalNet.Position - position).normalized);
                                break;

                            default:
                                closest.PlayerController.SetInstantPosition(position);
                                break;
                        }

                        closest.PlayerController.SetInstantRotation (Quaternion.LookRotation(targetGoalNet.Position - position));
                        ball.Hold(closest);
                    }
                }
            }
        }

        /// <summary>
        /// Reset all behaviour selection timers for teams.
        /// If you put a non null ballHitter, the other team will use their positioning skill as reset delayer.
        /// </summary>
        public void DelayBehaviourSelectionByReactionSkill () {
            float time = Time.time;

            foreach (var player in AllPlayers) {
                player.NextBehaviour = time + player.MatchPlayer.ActualReaction / 1000;
            }
        }

        public void ResetBehaviours () {
            foreach (var player in AllPlayers) {
                player.NextBehaviour = 0;
            }
        }

        public void Corner (int cornerIndex) {
            EventManager.Trigger (new CornerEvent (cornerIndex));

            Debug.Log(cornerIndex);

            MatchFlags = MatchStatus.Freeze;
            ResetOffsides();

            SetOutColliders(true);

            WaitForMoment(1, runFunction);

            async void runFunction () {
                EventManager.Trigger(new RefereeShortWhistleEvent());

                var currentCameraType = CameraSystem.Current.CurrentCameraType;
                // start transition.
                CameraTransition.Current.StartTransition(() => {
                    new TimerAction(1).GetQuery().Start(this, async () => {
                        MatchFlags = MatchStatus.WaitingForKickOff;

                        await CameraSystem.Current.SwitchCamera(currentCameraType);
                    });
                });

                var ballPosition = cornerSpots[cornerIndex].position;

                SetOutColliders(false);

                bool team1KickOff = cornerIndex > 1;

                ball.ResetBall(ballPosition);

                // choose kicker.
                var possibleKickers = team1KickOff ? GameTeam1.GamePlayers : GameTeam2.GamePlayers;

                var kicker = possibleKickers.
                    Where (x=> !x.IsGK).
                    OrderByDescending(x => x.MatchPlayer.GetLongBall()).First();

                kicker.PlayerController.SetInstantPosition(ballPosition + (cornerIndex % 2 == 0 ? -1 : 1) * Vector3.forward);
                var lookToBall = Quaternion.LookRotation(ballPosition - kicker.Position);
                lookToBall.eulerAngles = new Vector3(0, lookToBall.eulerAngles.y, 0);
                kicker.PlayerController.SetInstantRotation(lookToBall);

                kicker.IsCornerHolder = true;

                // hold the ball.
                ball.Hold(kicker);

                IFieldPositioning team1Positioning = DefenderCornerPositioning.Current;
                if (team1KickOff) {
                    team1Positioning = AttackerCornerPositioning.Current;
                }

                IFieldPositioning team2Positioning = DefenderCornerPositioning.Current;
                if (!team1KickOff) {
                    team2Positioning = AttackerCornerPositioning.Current;
                }

                GameTeam1.CornerPrePosition(
                    team1KickOff ? kicker : null,
                    team1Positioning,
                    fieldEndX,
                    fieldEndY,
                    goalNet1,
                    ball
                 );

                GameTeam2.CornerPrePosition(
                    !team1KickOff ? kicker : null,
                    team2Positioning,
                    fieldEndX,
                    fieldEndY,
                    goalNet2,
                    ball
                );

                await CameraSystem.Current.SwitchCamera("Corner");
                CameraSystem.Current.FocusToBall();
            }
        }

        public void GoalKick (int goalKickIndex) {
            EventManager.Trigger(new ShootWentOutEvent(ball.Velocity.magnitude));

            Foul(FoulType.GoalKick, goalKickSpots[goalKickIndex].position, goalKickIndex < 2 ? GameTeam1 : GameTeam2, FStudio.Data.Positions.ParametersCount);
        }
    }
}