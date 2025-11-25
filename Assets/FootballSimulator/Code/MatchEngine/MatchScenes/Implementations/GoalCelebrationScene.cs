using FStudio.Animation;
using FStudio.Events;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players.PlayerController;
using FStudio.MatchEngine.Players;
using FStudio.MatchEngine;
using FStudio.UI.Events;
using FStudio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using MM = FStudio.MatchEngine.MatchManager;
using FStudio.MatchEngine.Cameras;

namespace MatchEngine.MatchScenes.Implementations {
    public class GoalCelebrationScene : IMatchScene {
        private const float CELEBRATION_ZOOM_MULTIPLIER = 0.1f;

        private const float CELEBRATION_TIME = 6;

        private IEnumerable<PlayerBase> celebrators;
        private Transform celebrationPoint;
        private GameTeam scorerTeam, concedeTeam;
        private PlayerBase celebrator;

        private bool cameraSwitch = false;

        private bool isCelebrating = false;

        private bool taskDone = false;

        private bool goalSide;
        private int minute;

        private float celebrationStartTime;

        public GoalCelebrationScene() {
            EventManager.Subscribe<GoalEvent>(OnGoal);
        }

        public void Dispose () {
            EventManager.UnSubscribe<GoalEvent>(OnGoal);
        }

        public ESceneResult Update() {
            if (isCelebrating) {
                UpdateCelebration();
                return ESceneResult.FreeLogic;
            }

            return ESceneResult.Failed;
        }

        private void OnGoal(GoalEvent callback) {
            isCelebrating = true;
            taskDone = false;

            scorerTeam = callback.HomeOrAway ? MM.Current.GameTeam2 : MM.Current.GameTeam1;
            concedeTeam = callback.HomeOrAway ? MM.Current.GameTeam1 : MM.Current.GameTeam2;

            if (!callback.HomeOrAway) {
                MM.Current.homeTeamScore++;
            } else {
                MM.Current.awayTeamScore++;
            }

            var scorer = MM.Current.GoalScorer(scorerTeam.TeamId);

            if (scorer == null) {
                Debug.LogError("[GoalCelebration] No scorer.");
                return;
            }

            MM.Current.SetGoalColliders(true);

            if (cameraSwitch) {
                CameraSystem.Current.SetTarget(celebrator.PlayerController.UnityObject.transform);
            }

            celebrator = scorer;

            if (scorer.GameTeam != scorerTeam) {
                // Own Goal.
                // find the closest player for celebration.
                celebrator = null;
            }

            if (celebrator == null) {
                // pick the closest player to ball.
                celebrator = scorerTeam.GamePlayers.OrderBy(x => Vector3.Distance(x.Position, Ball.Current.transform.position)).FirstOrDefault();
            }

            // freeze match.
            MM.Current.MatchFlags = MatchStatus.Freeze | MatchStatus.Special;

            Debug.Log("Match freezed.");

            // celebration // sadness.
            // find a point to run.
            celebrationPoint = MM.Current.celebrationPoints.OrderBy(x => Vector3.Distance(x.position, celebrator.Position)).FirstOrDefault();

            // pick closest 3-6.
            celebrators = scorerTeam.GamePlayers.
                Where(x => x != celebrator).
                OrderBy(x => Vector3.Distance(x.Position, celebrationPoint.position)).
                Take(Random.Range(3, 6));
            //

            var toBeDisabled = scorerTeam.GamePlayers.Where(x => x != celebrator).Except(celebrators).Concat(concedeTeam.GamePlayers);
            foreach (var player in toBeDisabled) {
                player.PlayerController.IsPhysicsEnabled = false;
            }

            celebrator.PlayerController.Animator.SetBool(PlayerAnimatorVariable.IsHappy, true);
            celebrator.PlayerController.Animator.SetLayerWeight(1, 1);

            foreach (var player in concedeTeam.GamePlayers) {
                player.PlayerController.Animator.SetLayerWeight(1, 1);
                player.PlayerController.Animator.SetBool(PlayerAnimatorVariable.IsHappy, false);
            }

            // wait 1 second and switch camera.
            new TimerAction(1).GetQuery().Start(MM.Current, async () => {
                CameraSystem.Current.ZoomMultiplier = CELEBRATION_ZOOM_MULTIPLIER;
                cameraSwitch = true;

                await Task.Delay(1000);
            });

            Debug.Log($"[MatchManager] OnGoal {callback.HomeOrAway}");

            goalSide = callback.HomeOrAway;

            Debug.Log($"GoalSide: {goalSide}");

            minute = Mathf.CeilToInt(MM.Current.minutes);

            celebrationStartTime = Time.time;

            // goal scored event for other purposes!
            EventManager.Trigger(new GoalScoredEvent(goalSide, scorer.MatchPlayer.Player, minute));
        }

        private void UpdateCelebration() {
            if (!isCelebrating || taskDone) {
                return;
            }

            var deltaTime = Time.deltaTime;
            float time = Time.time;

            // celebrator will run to point.
            if (celebrator.MoveTo(in deltaTime, celebrationPoint.position, true) || celebrationStartTime + CELEBRATION_TIME < time) {
                EndCelebration();
                return;
            }

            // others will follow him.
            foreach (var player in celebrators) {
                player.MoveTo(in deltaTime, celebrator.Position, true);
            }
        }

        private void EndCelebration() {
            taskDone = true;

            MM.Current.WaitForMoment(1, RestoreCelebration);

            void RestoreCelebration() {
                CameraSystem.Current.FocusToBall(); // focus to ball again.

                CameraTransition.Current.StartTransition(() => {
                    new TimerAction(1).GetQuery().Start(MM.Current, async () => {
                        EventManager.Trigger(new RefereeShortWhistleEvent());

                        EventManager.Trigger(new InfoboardEvent());

                        await Task.Delay(4000);

                        EventManager.Trigger<InfoboardEvent>(null);
                    });
                });

                MM.Current.MatchFlags &= ~MatchStatus.Special;

                MM.Current.whichTeamStarted = (concedeTeam == MM.Current.GameTeam1) ? 1 : 2;

                CameraSystem.Current.ZoomMultiplier = 0;
                cameraSwitch = false;
                celebrator.PlayerController.Animator.SetLayerWeight(1, 0);
                foreach (var player in concedeTeam.GamePlayers) {
                    player.PlayerController.Animator.SetLayerWeight(1, 0);
                }

                var midPoint = new Vector3(MM.Current.fieldEndX / 2f, 0, MM.Current.fieldEndY / 2f);

                MM.Current.SetGoalColliders(false);

                foreach (var player in scorerTeam.GamePlayers.Concat(concedeTeam.GamePlayers)) {
                    player.PlayerController.IsPhysicsEnabled = true;
                }

                Ball.Current.ResetBall(midPoint);

                MM.Current.PrePositionTeamsForKickOff();

                MM.Statistics.ResetPositions(); 
                
                isCelebrating = false;
            }
        }

    }
}
