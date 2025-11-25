
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Players.PlayerController;
using FStudio.Players.Behaviours;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class BallChasingBehaviour : BaseBehaviour, IBallChasing {
        private const float MAX_CHASERS_DISTANCE_TOEACHOTHER = 10;
        private const float FOCUS_TO_BALL_AFTER_HEIGHT = 3f;
        private const float MAX_BALL_HEIGHT_FOR_CAUTION = 0.3f;

        private const float MAX_PROGRESS_FOR_CAUTION = 0.5f;

        private static readonly AnimationCurve DISTANCE_TO_EACHOTHER_SUBTRACT_BY_BALLPROGRESS = new AnimationCurve (
                new Keyframe(0, 7f),
                new Keyframe(0.25f, 3f),
                new Keyframe(0.5f, 1f),
                new Keyframe(1, 0f)
            );

        private static readonly AnimationCurve CAUTION_CURVE_FIELD_PROGRESS = new AnimationCurve(
                new Keyframe(0, 0.05f),
                new Keyframe(0.4f, 0.09f),
                new Keyframe(0.6f, 0.06f),
                new Keyframe(1, 0.03f)
            );

        private const int LIMIT_CHASERS_BY_DISTANCE = 10;

        public enum BallChasingResult {
            None,
            SecondaryChaser,
            ChaserTrue,
            /// <summary>
            /// We are the chaser but one of our opponents are closer than us to chase.
            /// </summary>
            ChaserTrueButCaution,
            /// <summary>
            /// We know we will get the ball.
            /// </summary>
        }

        protected (BallChasingResult result, float relaxation) chasingAct;

        public float ChasingDistance { get; protected set; }

        public PlayerBase ActivePlayer => Player;

        public override bool Behave (bool isAlreadyActive) {
            if (ball.HolderPlayer != null) {
                return false;
            }

            if (!IsRoughValidated ()) {
                return false;
            }

            if (Player.CaughtInOffside) {
                return false;
            }

            if (!isAlreadyActive) {
                // check if we are the good option.
                chasingAct = AmITheChaser();

                if (chasingAct.result != BallChasingResult.None) {
                    isAlreadyActive = true; // activate.
                }
            }

            if (isAlreadyActive) {
                Log(this, $"Chasing active with {chasingAct}");

                // check chasing environment.
                if (!EnvironmentCheckForChasing()) {
                    Log(this, $"Environment is not suitable for chasing.");
                    return false;
                }

                if (IsTheBallGoingOutside ()) {
                    Log(this, $"Ball is going outside, no need for chasing.");
                    return false;
                }

                var actualBallPos = ball.transform.position;
                bool lookPoint = actualBallPos.y > FOCUS_TO_BALL_AFTER_HEIGHT;

                if (chasingAct.result == BallChasingResult.ChaserTrue) {
                    // Chasing ball directly.
                    Player.CurrentAct = Acts.GoingToGetTheBall_BallChasing;

                    var actualPoint = ball.BallPosition(Player, chasingAct.relaxation);

                    Player.MoveTo(in deltaTime, actualPoint, !lookPoint);

                    if (lookPoint) {
                        Player.LookTo(in deltaTime, actualBallPos - Player.Position);
                    }
                } else {
                    // With Caution, relax, or secondary.
                    // go to our goal net.

                    Player.CurrentAct = Acts.GoingToGetTheBall_WithCaution;

                    var actualPoint = ball.BallPosition(Player, chasingAct.relaxation);

                    var caution = CAUTION_CURVE_FIELD_PROGRESS.Evaluate(Player.PlayerFieldProgress);

                    actualPoint = Vector3.Lerp (actualPoint, goalNet.Position, caution);

                    Player.MoveTo(in deltaTime, actualPoint, !lookPoint, 
                        chasingAct.result == BallChasingResult.ChaserTrueButCaution ? MovementType.BestHeCanDo : MovementType.Normal);

                    if (lookPoint) {
                        Player.LookTo(in deltaTime, actualBallPos - Player.Position);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the type of ball chasing.
        /// If it's relax mode, it will also return relaxation power.
        /// </summary>
        /// <returns></returns>
        protected (BallChasingResult result, float relaxation) AmITheChaser () {
            var debugger = Player.PlayerController.IsDebuggerEnabled;

            float relaxation = 0;

            var chaserCount = Player.
                GameTeam.
                Team.
                TeamTactics.
                GetTacticSettings (Player.GameTeam.Team.TacticPresetType, teamBehaviour).
                HowManyPlayersWillChaseTheBall(Player.GameTeam.BallProgress);

            Log(this, $"Chasing count: {chaserCount}");

            Log(this, $"Team ball progress: " + Player.GameTeam.BallProgress);

            var teamChasers = teammates.
                    Where(x =>
                    x.PlayerController.IsPhysicsEnabled &&
                    !x.CaughtInOffside).
                    Select (x=> (x, BallChasingDistance (x))).
                    OrderBy(x=> x.Item2).
                    Take(chaserCount + 1);

            var chasersAsArray = teamChasers.ToArray();

            var requiredChasers = 1;

            for (int i=1, length= chasersAsArray.Length; i<length; i++) {
                if (chasersAsArray[i].Item2 / LIMIT_CHASERS_BY_DISTANCE <= i+1) {
                    requiredChasers++;
                } else {
                    break;
                }
            }

            Log(this, $"Required chasers: {requiredChasers}");

            // fix by required chasers.
            teamChasers = teamChasers.Take(requiredChasers);

            IEnumerable <(PlayerBase x, float distance)> chasers;
            (PlayerBase x, float distance) myChaser;

            if (Player.MatchPlayer.Position != FStudio.Data.Positions.GK) {
                var bests = teamChasers.Where(x => !x.x.IsGK).Take(chaserCount);

                chasers = bests.Where (x=>x.x == Player).Select (x=>(x.x, x.Item2));
            } else {
                // only if we are the closest for GK.
                chasers = teamChasers.Take (1).Select(x => (x.x, x.Item2));
            }

            myChaser = chasers.FirstOrDefault(x => x.x == Player);

            if (myChaser.x != null) {
                // we are chaser.
                ChasingDistance = myChaser.distance;

                Log(this, $"We should chase, our chasing distance is {ChasingDistance}");
                
                // check opponent chasers.
                var opponentChasers = opponents.
                    Where(x => x.ActiveBehaviour is BallChasingBehaviour || x.ActiveBehaviour is BallChasingWithoutCondition).
                    Select (x=>x.ActiveBehaviour as IBallChasing);

                if (opponentChasers.Any ()) {
                    // check for relax mode.
                    var closestOpponentChaser = opponentChasers.OrderBy(x => x.ChasingDistance).FirstOrDefault ();
                    var diff = closestOpponentChaser.ChasingDistance - myChaser.distance;
                    relaxation = Mathf.Max (0, diff);
                }

                if (Player.PlayerFieldProgress < MAX_PROGRESS_FOR_CAUTION && !ball.IsOnCrossMode && ball.transform.position.y < MAX_BALL_HEIGHT_FOR_CAUTION) {
                    foreach (var opChaser in opponentChasers) {
                        if (opChaser.ChasingDistance > 0 && opChaser.ChasingDistance < ChasingDistance) {
                            // he is behind of us. Closer to our goal net.
                            Log(this, $"We will chase with caution.");

                            return (BallChasingResult.ChaserTrueButCaution, relaxation);
                        }
                    }
                }

                var result = chasers.ToList().FindIndex(x => x.x == Player) == 0 ? BallChasingResult.ChaserTrue : BallChasingResult.SecondaryChaser;

                Log(this, result == BallChasingResult.ChaserTrue ? "We will chase as we can." : $"We will chase with relax mode.");

                return (result,relaxation);
            }

            ChasingDistance = -1;

            return (BallChasingResult.None, relaxation);
        }

        public static float BallChasingDistance (PlayerBase target) {
            var ball = Ball.Current;

            var myPrediction = ball.BallPosition(target);

            var ballPosition = ball.transform.position;
            ballPosition.y = 0;

            var gkMod = target.MatchPlayer.Position == FStudio.Data.Positions.GK ? EngineSettings.Current.BallChasingDistanceGKAddition : 0;

            return 
                Vector3.Magnitude(myPrediction - ballPosition) +
                Vector3.Magnitude(myPrediction - target.Position) + 
                EngineSettings.Current.BallChasingChaserToBallDistanceAdditionCurve.Evaluate
                (Vector3.Distance(ballPosition, target.Position)) +
                gkMod;
        }

        protected bool EnvironmentCheckForChasing () {
            var ourTeamChasers = teammates.Where(x => x != Player &&
            (x.ActiveBehaviour is BallChasingBehaviour || x.ActiveBehaviour is BallChasingWithoutCondition));

            var myChasingDistance = BallChasingDistance(Player);

            var subtractor = DISTANCE_TO_EACHOTHER_SUBTRACT_BY_BALLPROGRESS.Evaluate(Player.GameTeam.BallProgress);

            foreach (var chaser in ourTeamChasers) {
                var chasingDistance = BallChasingDistance(chaser);

                if (chasingDistance < myChasingDistance) {
                    // he is closer.
                    if (Vector3.Distance (chaser.Position, Player.Position) < MAX_CHASERS_DISTANCE_TOEACHOTHER - subtractor) {
                        // we are close to him, ignore chasing.
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
