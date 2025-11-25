using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Players.Behaviours;
using FStudio.MatchEngine.Tactics;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players {
    public class Defender : PlayerBase {
        public Defender(GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) : base(gameTeam, matchPlayer, kitMaterial)
        {
        }

        protected override IEnumerable<BaseBehaviour> PrivateBehaviours => new BaseBehaviour[] {
            // Try to run to if we are safe.
            new RunForwardWithBallBehaviour(1,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),
            
            // Try to shoot.
            new ShootingBehaviour (),

            // Try to pass, only if target is closer to goal net.
            new PassingBehaviour (0.7f, true),

            new CrossingBehaviour (),

            // Try to run to straight (carefully)
            new RunForwardWithBallBehaviour(0.5f,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.MostlyStraight),

            // We should not run anymore. Pass a target in front of us.
            new PassingBehaviour (0, true, 0),

            // Try to run to goal (carefully)
            new RunForwardWithBallBehaviour(1f,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            // Pass anywhere.
            new PassingBehaviour (1),

            new ShootingBehaviour (0, 2),

            // cross with higher chance
            new CrossingBehaviour (0.7f, 2),

            // Send ball safe.
            new SendBallToSafe (),

            // Try to run to goal by ignoring chasing check.
            new RunForwardWithBallBehaviour(0, RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            new PassAndRunBehaviour (),
            new BallChasingBehaviour(),
            new BallChasingWithoutCondition (),
            new TryToTackleBehaviour (),
            new JoinTheAttackBehaviour (JoinTheAttackBehaviour.ForwardCurve.KeepX, 0.25f, 0.65f),
            new MarkTheLastGuy (),
            new DefensiveTacticalPositioningBehaviour ()
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
