using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;

using System.Collections.Generic;

using FStudio.MatchEngine.Players.Behaviours;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players {
    public class Midfielder : PlayerBase {
        public Midfielder(GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) : base(gameTeam, matchPlayer, kitMaterial)
        {
        }

        protected override IEnumerable<BaseBehaviour> PrivateBehaviours => new BaseBehaviour[] {
            // We should not run anymore. Pass a target in front of us.
            new PassingBehaviour (0.5f, true, 0),

            // Try to shoot.
            new ShootingBehaviour (),
            
            // Try to pass, only if target is closer to goal net.
            new PassingBehaviour (0.7f, true),

            new CrossingBehaviour (),

            // Try to run to straight (carefully)
            new RunForwardWithBallBehaviour(0.7f,
                RunForwardWithBallBehaviour.BewareMod.Careful,
                RunForwardWithBallBehaviour.ForwardCurve.MostlyStraight),
            
            // Pass anywhere.
            new PassingBehaviour (1),
            
            // Try to run to goal by ignoring chasing check.
            new RunForwardWithBallBehaviour(0, RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal),

            // try to shoot wit a high roller
            new ShootingBehaviour (0, 2),

            new CrossingBehaviour (0.7f, 2),

            // Send ball safe.
            new SendBallToSafe (),

            new BallChasingBehaviour (),
            new BallChasingWithoutCondition (),
            new TryToTackleBehaviour (),
            new BecomeAPassOptionBehaviour(),
            new JoinTheAttackBehaviour (JoinTheAttackBehaviour.ForwardCurve.KeepX, 0.75f, 1.25f),
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
