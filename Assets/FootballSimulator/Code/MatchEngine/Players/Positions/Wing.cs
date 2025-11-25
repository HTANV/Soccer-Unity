using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;

using System.Collections.Generic;

using FStudio.MatchEngine.Players.Behaviours;
using UnityEngine;

namespace FStudio.MatchEngine.Players {
    public class Wing : PlayerBase {
        public Wing(GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) : base(gameTeam, matchPlayer, kitMaterial)
        {
        }

        protected override IEnumerable<BaseBehaviour> PrivateBehaviours => new BaseBehaviour[] {
            // Try to run to if we are safe.
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            new PassingBehaviour (0.8f, true, -10),

            new PassingBehaviour (0, true, -5), // Try to pass, only if target is closer to goal net.
                
            new ShootingBehaviour (),

            new CrossingBehaviour (),

            // Try to run straight (carefully)
            new RunForwardWithBallBehaviour(0.5f,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.MostlyStraight),

            // Try to run to goal (carefully)
            new RunForwardWithBallBehaviour(1f,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            // Try to run like a wing man (normal)
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Normal,
                RunForwardWithBallBehaviour.ForwardCurve.Wingman),

            // Pass anywhere.
            new PassingBehaviour (0.75f),            
            
            new ShootingBehaviour (0, 2),

            new CrossingBehaviour (0.7f, 2),

            // Send ball safe.
            new SendBallToSafe (),

            // Try to run to goal by ignoring chasing check.
            new RunForwardWithBallBehaviour(0, RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            new PassAndRunBehaviour (),
            new BallChasingBehaviour (),
            new BallChasingWithoutCondition (),
            new TryToTackleBehaviour (),
            new BecomeAPassOptionBehaviour(),

            new JoinTheAttackBehaviour (JoinTheAttackBehaviour.ForwardCurve.GoalFocused, 0.5f, 1.25f),
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
