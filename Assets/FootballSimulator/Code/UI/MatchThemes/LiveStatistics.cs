using FStudio.Events;
using FStudio.MatchEngine;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.UI;
using FStudio.MatchEngine.Utilities;
using FStudio.UI.Graphics;
using Shared;
using FStudio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FStudio.Utilities;

namespace FStudio.UI.MatchThemes {
    public class LiveStatistics : Panel {
        protected enum StatisticsType {
            PlayerToWatch,
            TeamPossession, // %78
            TeamAttempts, // 1/4
            TeamPassing, // 345/452 %78
            TeamRunDistance, // 16km
            PlayerBestRunDistance, // 2km
            PlayerBestDribblingDistance, // 0.5km
            PlayerAttemps, // 0/2
            PlayerPassing, // 4/9 %45
            PlayerRating,
            PlayerBallWinning,
            TeamBallWinning,
            TeamBallLosing
        }

        [Serializable]
        public class PlayerFace {
            public GameObject[] playerHolders;
            public TextMeshProUGUI playerName;
            public SquadPosition squadPosition;
            public TextMeshProUGUI overallText;
        }


        [Serializable]
        protected class Team {
            public Transform faceHolder;

            public PlayerFace generalFace;

            public TextMeshProUGUI stats;
        }

        [Serializable]
        protected class StatisticsOption : IDroppable {
            public int Rate => PickRate;

            public StatisticsType SType;
            public int PickRate;
        }

        private readonly LimitedCollection<StatisticsType> showedStatistics = new LimitedCollection<StatisticsType> (5);

        private const float timeOffsetBetweenStatisticsAsSeconds = 12;
        private const float statisticsLifeTime = 5.5f;
        private const float rollChancePercentage = 20;

        [SerializeField] private Team[] sides;
        [SerializeField] private StatisticsOption[] options;
        [SerializeField] private Image teamLogoImage;
        [SerializeField] private GameObject[] regularStatistics;
        [SerializeField] private TextMeshProUGUI infoText;

        private float nextCheck;

        private bool isBusy = true;

        protected override void OnEnable () {
            base.OnEnable();

            EventManager.Subscribe<FirstWhistleEvent>(FirstWhistle);
            EventManager.Subscribe<FinalWhistleEvent>(FinalWhistle);
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<FirstWhistleEvent>(FirstWhistle);
            EventManager.UnSubscribe<FinalWhistleEvent>(FinalWhistle);
        }

        private void FinalWhistle (FinalWhistleEvent _) {
            Disappear();
        }

        private void FirstWhistle (FirstWhistleEvent _) {
            try {
                OpenStatistics(StatisticsType.PlayerToWatch);
                Appear();
            } catch (Exception exception) {
                Debug.Log("[LiveStatistics] Could not run: " + exception);
            }
        }

        protected override void Update() {
            base.Update();

            float time = Time.time;

            if (isBusy ||
                time < nextCheck) {
                return;
            }

            try {
                var result = CheckStatistics();
                if (result) {
                    Appear();
                }
            } catch { }
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            isBusy = true;
        }

        protected override async void OnAppeared() {
            base.OnAppeared();

            await Task.Delay (Mathf.RoundToInt (statisticsLifeTime * 1000));

            Disappear();
        }

        protected override void OnDisappeared() {
            base.OnDisappeared();

            isBusy = false;

            nextCheck = Time.time + timeOffsetBetweenStatisticsAsSeconds;
        }

        private bool CheckStatistics () {
            if (!MatchManager.Current.MatchFlags.HasFlag(MatchEngine.Enums.MatchStatus.Playing)) {
                return false;
            }

            if (UnityEngine.Random.Range(0, 100) > rollChancePercentage) {
                return false;
            }

            // create dropper.

            StatisticsType sType = default;
            var dropper = new Dropper<StatisticsOption>(options);
            while (true) {
                sType = dropper.Select().SType;

                if (!showedStatistics.Members.Any (x=>x == sType)) {
                    break;
                }
            }

            showedStatistics.Add(sType);

            return OpenStatistics(sType);
        }

        private bool OpenStatistics (StatisticsType sType) {
            regularStatistics.SetActive(true);

            var statistics = MatchManager.Statistics;
            var homePlayerIds = MatchManager.Current.GameTeam1.GamePlayers.Select(x => x.MatchPlayer.Player.id).ToList();
            var awayPlayerIds = MatchManager.Current.GameTeam2.GamePlayers.Select(x => x.MatchPlayer.Player.id).ToList();

            /// Find the best value in dictionary.
            IOrderedEnumerable <KeyValuePair<int, T>> bestInDictionary<T>(List<int> playerIds, Dictionary<int, T> dic) where T : IComparable {
                return dic.Where (x=>playerIds.Contains(x.Key)).OrderByDescending(x => x.Value);
            }

            int bestHomeTeamer, bestAwayTeamer;

            switch (sType) {
                case StatisticsType.PlayerPassing:
                    int bestTeamPasser(List<int> teamPlayerIds) {
                        var mostPassers = bestInDictionary(teamPlayerIds, statistics.passing.PlayerPassing);
                        var successPassers = bestInDictionary(teamPlayerIds, statistics.passing.PlayerSuccesfulPasses);

                        var bestPassers = teamPlayerIds.OrderByDescending(x =>
                        mostPassers.FirstOrDefault(e => e.Key == x).Value +
                        successPassers.FirstOrDefault(e => e.Key == x).Value).
                        ThenBy(x => mostPassers.FirstOrDefault(e => e.Key == x).Value);

                        var bestPasser = bestPassers.FirstOrDefault();

                        return bestPasser;
                    }

                    string parsePlayerPassingStats(int playerId) {
                        var pass = statistics.passing.PlayerPassing[playerId];
                        var succ = statistics.passing.PlayerSuccesfulPasses[playerId];
                        return $"{succ}/{pass}";
                    }

                    bestHomeTeamer = bestTeamPasser(homePlayerIds);
                    bestAwayTeamer = bestTeamPasser(awayPlayerIds);

                    if (statistics.passing.PlayerPassing[bestHomeTeamer] == 0 || 
                        statistics.passing.PlayerPassing[bestAwayTeamer] == 0) {
                        return false;
                    }

                    EnableStatistics("Best Passers",
                        parsePlayerPassingStats(bestHomeTeamer), 
                        parsePlayerPassingStats(bestAwayTeamer), 
                        bestHomeTeamer, 
                        bestAwayTeamer);

                    break;

                case StatisticsType.PlayerRating:
                    var bestHomeTeamerRating = homePlayerIds.Select(x => (x, statistics.PlayerRating(x))).OrderByDescending(x => x.Item2).FirstOrDefault();
                    var bestAwayTeamerRating = awayPlayerIds.Select(x => (x, statistics.PlayerRating(x))).OrderByDescending(x => x.Item2).FirstOrDefault();

                    EnableStatistics("Rating",
                        bestHomeTeamerRating.Item2.ToString(),
                        bestAwayTeamerRating.Item2.ToString(),
                        bestHomeTeamerRating.x,
                        bestAwayTeamerRating.x);
                    break;

                case StatisticsType.TeamPassing:
                    if (statistics.passing.Passes[0] == 0 || 
                        statistics.passing.Passes[1] == 0) {
                        return false;
                    }

                    string parseTeamPassingStats (int teamId) {
                        var pass = statistics.passing.Passes [teamId];
                        var succ = statistics.passing.SuccesfulPasses[teamId];
                        var perc = statistics.passing.PassingPercentage[teamId];
                        return $"{succ}/{pass}";
                    }

                    EnableStatistics("Passes",
                        parseTeamPassingStats(0),
                        parseTeamPassingStats(1));
                    break;

                case StatisticsType.PlayerToWatch:
                    (int id, int height) BestOverall (GameTeam gameTeam) {
                        return gameTeam.GamePlayers.OrderByDescending(x => 
                        x.MatchPlayer.Player.Overall).Take (4).
                        OrderBy (x=>Guid.NewGuid ())
                        .Select(x =>
                        (x.MatchPlayer.Player.id, x.MatchPlayer.Player.height)).FirstOrDefault ();
                    }

                    var homeTeamPlayerWatch = BestOverall(MatchManager.Current.GameTeam1);
                    var awayTeamPlayerWatch = BestOverall(MatchManager.Current.GameTeam2);

                    Debug.Log("[PlayerToWatch] HomeTeam : " + homeTeamPlayerWatch.id);
                    Debug.Log("[PlayerToWatch] AwayTeam : " + awayTeamPlayerWatch.id);

                    EnableStatistics("Player to Watch", homeTeamPlayerWatch.height + " cm", awayTeamPlayerWatch.height + " cm",
                        homeTeamPlayerWatch.id, awayTeamPlayerWatch.id);
                    break;

                case StatisticsType.TeamPossession:
                    EnableStatistics("Possession",
                            $"%{statistics.possesioning.TeamPositioning[0]}",
                            $"%{statistics.possesioning.TeamPositioning[1]}");

                    break;

                case StatisticsType.TeamBallWinning:
                    if (statistics.ballWinning.Winnings[0] == 0 && statistics.ballWinning.Winnings[1] == 0) {
                        return false;
                    }

                    EnableStatistics("Ball Winning",
                            $"{statistics.ballWinning.Winnings[0]}",
                            $"{statistics.ballWinning.Winnings[1]}");

                    break;

                case StatisticsType.TeamBallLosing:
                    if (statistics.ballLosing.Losing[0] == 0 && statistics.ballLosing.Losing[1] == 0) {
                        return false;
                    }

                    EnableStatistics("Ball Loss",
                            $"{statistics.ballWinning.Winnings[0]}",
                            $"{statistics.ballWinning.Winnings[1]}");

                    break;

                case StatisticsType.PlayerAttemps:
                    int bestTeamShooter (List<int> teamPlayerIds) {
                        var mostShooters = bestInDictionary(teamPlayerIds, statistics.shooting.PlayerAttempt);
                        var successShooters = bestInDictionary(teamPlayerIds, statistics.shooting.PlayerAttemptOnTarget);

                        var bestShooters = teamPlayerIds.OrderByDescending(x =>
                        mostShooters.FirstOrDefault(e => e.Key == x).Value +
                        successShooters.FirstOrDefault(e => e.Key == x).Value).
                        ThenByDescending (x=> successShooters.FirstOrDefault(e => e.Key == x).Value);

                        var bestShooter = bestShooters.FirstOrDefault();

                        return bestShooter;
                    }

                    string parsePlayerShootingStats(int playerId) {
                        var shoot = statistics.shooting.PlayerAttempt[playerId];
                        var succ = statistics.shooting.PlayerAttemptOnTarget[playerId];
                        return $"{succ}/{shoot}";
                    }

                    bestHomeTeamer = bestTeamShooter(homePlayerIds);
                    bestAwayTeamer = bestTeamShooter(awayPlayerIds);

                    if (statistics.shooting.PlayerAttempt[bestHomeTeamer] == 0 ||
                        statistics.shooting.PlayerAttempt[bestAwayTeamer] == 0) {
                        return false;
                    }

                    EnableStatistics("Player Attemps",
                        parsePlayerShootingStats(bestHomeTeamer),
                        parsePlayerShootingStats(bestAwayTeamer),
                        bestHomeTeamer,
                        bestAwayTeamer);
                    break;

                case StatisticsType.TeamAttempts:
                    if (statistics.shooting.Attempts[0] == 0 || statistics.shooting.Attempts[1] == 0) {
                        return false;
                    }

                    EnableStatistics("Attempts",
                        $"{statistics.shooting.Attempts[0]} / {statistics.shooting.AttemptsOnTarget[0]}",
                        $"{statistics.shooting.Attempts[1]} / {statistics.shooting.AttemptsOnTarget[1]}");
                    break;

                case StatisticsType.PlayerBallWinning:
                    var homeTeamBestWinner = bestInDictionary(homePlayerIds, statistics.ballWinning.PlayerBallWinning).FirstOrDefault ();
                    var awayTeamBestWinner = bestInDictionary(awayPlayerIds, statistics.ballWinning.PlayerBallWinning).FirstOrDefault ();

                    if (homeTeamBestWinner.Value == 0 || 
                        awayTeamBestWinner.Value == 0) {
                        return false; // no need for this info.
                    }

                    EnableStatistics("Best Ball Winners",
                        homeTeamBestWinner.Value.ToString(),
                        awayTeamBestWinner.Value.ToString (),
                        homeTeamBestWinner.Key,
                        awayTeamBestWinner.Key);
                    break;

                case StatisticsType.PlayerBestRunDistance:
                    var homeTeamBestRunner = bestInDictionary(homePlayerIds, statistics.runningDistance.PlayerDistances).FirstOrDefault();
                    var awayTeamBestRunner = bestInDictionary(awayPlayerIds, statistics.runningDistance.PlayerDistances).FirstOrDefault();

                    if (homeTeamBestRunner.Value < 100 || awayTeamBestRunner.Value < 100) {
                        return false;
                    }

                    EnableStatistics("Distance",
                        MatchStatistics.LocalizedDistance(homeTeamBestRunner.Value),
                        MatchStatistics.LocalizedDistance(awayTeamBestRunner.Value),
                        homeTeamBestRunner.Key,
                        awayTeamBestRunner.Key);

                    break;

                case StatisticsType.TeamRunDistance:
                    EnableStatistics("Dribbling",
                        MatchStatistics.LocalizedDistance(statistics.runningDistance.TeamDistances[0]),
                        MatchStatistics.LocalizedDistance(statistics.runningDistance.TeamDistances[1]));

                    break;

                case StatisticsType.PlayerBestDribblingDistance:
                    var homeTeamBestDribbler = bestInDictionary(homePlayerIds, statistics.dribbling.PlayerDribbling).FirstOrDefault();
                    var awayTeamBestDribbler = bestInDictionary(awayPlayerIds, statistics.dribbling.PlayerDribbling).FirstOrDefault();

                    if (homeTeamBestDribbler.Value < 10 || 
                        awayTeamBestDribbler.Value < 10) {
                        return false;
                    }

                    EnableStatistics("Best Dribblers",
                        MatchStatistics.LocalizedDistance(homeTeamBestDribbler.Value),
                        MatchStatistics.LocalizedDistance(awayTeamBestDribbler.Value),
                        homeTeamBestDribbler.Key,
                        awayTeamBestDribbler.Key);
                    break;
            }

            return true;
        }

        private void SetPlayer (PlayerFace face, GameTeam team, MatchPlayer player) {
            face.overallText.text = player.Player.Overall.ToString();
            face.playerName.text = $"{player.Number} {player.Player.Name}";

            foreach (var holder in face.playerHolders)
                holder.SetActive(true);

            face.squadPosition.SetPosition(PositionRules.GetBasePosition(player.Position));
        }

        private void EnableStatistics (
            string infoLocale,
            string homeStats,
            string awayStats, 
            int homePlayerId = -1, 
            int awayPlayerId = -1) {

            Debug.Log("Enable Statistics " + infoLocale);

            infoText.text = infoLocale;

            var allPlayers = MatchManager.AllPlayers;

            void setSide (int side, int playerId, string stat) {
                if (playerId != -1) {
                    var player = allPlayers.FirstOrDefault(x => x.MatchPlayer.Player.id == playerId);
                    SetPlayer(
                        sides[side].generalFace,
                        player.GameTeam,
                        player.MatchPlayer);
                }

                sides[side].faceHolder.gameObject.SetActive(playerId != -1);

                sides[side].stats.text = stat;
            }

            setSide(0, homePlayerId, homeStats);
            setSide(1, awayPlayerId, awayStats);
        }
    }
}
