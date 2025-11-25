using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;

using System.Collections.Generic;

using FStudio.MatchEngine.Players.Behaviours;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players {
    public class AttackingWing : PlayerBase {
        public AttackingWing(GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) : base(gameTeam, matchPlayer, kitMaterial)
        {
        }

        protected override IEnumerable<BaseBehaviour> PrivateBehaviours => new BaseBehaviour[] {
            // Try to run straight (carefully)
            new RunForwardWithBallBehaviour(0.5f,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.MostlyStraight),

            // Try to run to if we are safe.
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            new PassingBehaviour (0f, true, 5), // Try to pass to front of us
            
            // try to shoot.
            new ShootingBehaviour (),

            new PassingBehaviour (0.8f, true), // Try to pass, only if target is closer to goal net.
            
            new CrossingBehaviour (),

            // Try to run to goal (carefully)
            new RunForwardWithBallBehaviour(1f,
                RunForwardWithBallBehaviour.BewareMod.Normal,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            // Pass anywhere.
            new PassingBehaviour (1),
            
            // Try to run like a wing man (normal)
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Risky,
                RunForwardWithBallBehaviour.ForwardCurve.Wingman),

            // try to shoot wit a high roller
            new ShootingBehaviour (0, 2),           
            
            // cross with higher chance
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
