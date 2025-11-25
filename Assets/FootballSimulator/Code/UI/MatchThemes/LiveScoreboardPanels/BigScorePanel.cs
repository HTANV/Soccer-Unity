using FStudio.MatchEngine.Events;
using FStudio.UI.Events;
using UnityEngine;
using FStudio.Loaders;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using FStudio.Events;
using Shared.Responses;
using FStudio.UI.Graphics;
using UnityEngine.UI;
using FStudio.UI.MatchThemes.MatchEvents;
using FStudio.Database;

namespace FStudio.UI.MatchThemes {
    public class BigScorePanel : ScoreboardPanel {
        private const int MAX_GOAL_PER_TEAM = 50;
        
        private bool isInitialized;
        //InfoboardEvent
        private StaticPool<ScoreboardScorerMember, PlayerEntry> homeScorersPool, awayScorersPool;

        [SerializeField] private Transform homeScorerHolder, awayScorerHolder;

        [SerializeField] private AssetReference scorerElementAsset;

        [SerializeField] private Image homeTeamLogo;
        [SerializeField] private Image awayTeamLogo;

        private async Task<StaticPool<ScoreboardScorerMember, PlayerEntry>> InitScorerBoard (Transform scorerHolder) {
            var scorerPool = new StaticPool<ScoreboardScorerMember, PlayerEntry>(scorerElementAsset, scorerHolder);

            for (int i = 0; i < MAX_GOAL_PER_TEAM; i++) {
                var asset = scorerElementAsset.InstantiateAsync(scorerHolder).Task;
                await asset;

                asset.Result.SetActive(false); // hide.

                scorerPool.Add(asset.Result.GetComponent<ScoreboardScorerMember>());
            }

            return scorerPool;
        }

        protected override void OnEnable() {
            base.OnEnable();

            EventManager.Subscribe<GoalScoredEvent>(OnGoalScored);
            EventManager.Subscribe<InfoboardEvent>(OnInfoBoard);
            EventManager.Subscribe<UpcomingMatchEvent>(UpcomingMatch);
            EventManager.Subscribe<KickOffEvent>(Kickoff);
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<GoalScoredEvent>(OnGoalScored);
            EventManager.UnSubscribe<InfoboardEvent>(OnInfoBoard);
            EventManager.UnSubscribe<UpcomingMatchEvent>(UpcomingMatch);
            EventManager.UnSubscribe<KickOffEvent>(Kickoff);
        }

        protected override void OnEventCalled (UpcomingMatchEvent upcomingMatchEvent) {
            if (upcomingMatchEvent == null) {
                return;
            }

            base.OnEventCalled(upcomingMatchEvent);

            var logoMaterial = GetLogo(upcomingMatchEvent.details.homeTeam.TeamLogo);
            homeTeamLogo.material = logoMaterial;

            logoMaterial = GetLogo(upcomingMatchEvent.details.awayTeam.TeamLogo);
            awayTeamLogo.material = logoMaterial;

            // clear scores.
            void clear(StaticPool<ScoreboardScorerMember, PlayerEntry> pool) {
                foreach (var e in pool.Members) {
                    e.IsActive = false;
                }
            }

            if (!isInitialized) {
                return;
            }

            clear(homeScorersPool);
            clear(awayScorersPool);
        }

        private async void OnGoalScored (GoalScoredEvent goalScored) {
            var targetPool = !goalScored.Side ? homeScorersPool : awayScorersPool;

            var member = targetPool.Get();

            await member.SetMember(goalScored.Scorer);
            member.SetMinute(goalScored.Minute);

            member.IsActive = true;
        }

        private async Task Initialize () {
            if (isInitialized) {
                return;
            }

            homeScorersPool = await InitScorerBoard(homeScorerHolder);
            awayScorersPool = await InitScorerBoard(awayScorerHolder);

            isInitialized = true;
        }

        private void Clear () {
            void clear(StaticPool<ScoreboardScorerMember, PlayerEntry> pool) {
                foreach (var e in pool.Members) {
                    e.MarkAsDeactive();
                }
            }

            if (isInitialized) {
                clear(homeScorersPool);
                clear(awayScorersPool);
            }
        }

        private async void UpcomingMatch (UpcomingMatchEvent eventObject) {
            await Initialize();
        }

        private async void Kickoff (KickOffEvent eventObject) {
            Appear();

            await Task.Delay(3000);

            Disappear();
        }

        private void OnInfoBoard (InfoboardEvent eventObject) {
            if (eventObject == null) {
                Clear();
                Disappear();
            } else {
                async void enabler(StaticPool<ScoreboardScorerMember, PlayerEntry> pool) {
                    foreach (var e in pool.Members) {
                        if (e.IsActive) {
                            await Task.Delay(200);
                            e.MarkAsActive();
                        }
                    }
                }

                enabler(homeScorersPool);
                enabler(awayScorersPool);

                Appear();
            }
        }

        private Material GetLogo(LogoEntry logo) {
            var result = TeamLogoMaterial.Current.GetColoredMaterial(logo);
            return result;
        }
    }
}
