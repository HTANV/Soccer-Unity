using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Players.Behaviours;
using FStudio.MatchEngine.Tactics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players {
    public class Scorer : PlayerBase {
        public Scorer(GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) : base(gameTeam, matchPlayer, kitMaterial)
        {
        }

        protected override IEnumerable<BaseBehaviour> PrivateBehaviours => new BaseBehaviour[] {
            // Try to run with a risk after some ball progress.
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Normal,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal,
                0.75f),

            new ShootingBehaviour (),

            new PassingBehaviour (0.7f, true, -5),

            new PassingBehaviour (0, true, -10),

            new CrossingBehaviour (),

            // Try to run to if we are safe.
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Risky,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            /// Safe pass until 3/4
            new PassingBehaviour (1),
            
             // nowhere to go try to shoot with more tolerance
            new ShootingBehaviour (0, 2),

            new CrossingBehaviour (0.7f, 2),

            // Send ball safe.
            new SendBallToSafe (),

            // Try to run to goal by ignoring chasing check.
            new RunForwardWithBallBehaviour(0, RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            new PassAndRunBehaviour (),
            new BallChasingBehaviour(),
            new BallChasingWithoutCondition (),
            new TryToTackleBehaviour (),
            new RunBehindDefenseLineBehaviour(),
            new StrikerTacticalPositioningBehaviour (),
            new MarkTheLastGuy (),
            new TacticalPositioningBehaviour ()
        };

        public override sealed void Behave(
            in bool isInputControlled,
            in float time,
            in float deltaTime,
            in int fieldEndX,
            in int fieldEndY,
            in MatchStatus matchStatus,
            in TeamBehaviour tactics,
            in float offsideLine,
            Ball ball,
            GoalNet goalNet,
            GoalNet targetGoalNet,
            in PlayerBase[] teammates,
            in PlayerBase[] opponents) {

            ProcessMovement(in time, in deltaTime);

            base.Behave(in isInputControlled,
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
                in teammates,
                in opponents);
        }
    }
}
