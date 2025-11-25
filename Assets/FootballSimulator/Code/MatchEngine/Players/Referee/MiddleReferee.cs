using FStudio.MatchEngine.Players.Behaviours;
using System;
using System.Collections.Generic;

using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using UnityEngine;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Players.Referee {
    public class MiddleReferee : AbstractReferee {
        private const float REFEREE_BALL_DISTANCE = 20;
        private const float HORIZONTAL_DISTANCE_BY_BALL_Z_POSITION = 10;

        public MiddleReferee(MatchPlayer matchPlayer, GameTeam gameTeam, Material kitMaterial) : base(matchPlayer, gameTeam, kitMaterial)
        {
            
        }

        public override IEnumerable<BaseBehaviour> Behaviours {
            get => throw new NotImplementedException();
        }

        public override void RefereeBehave(
            in int fieldEndX,
            in int fieldEndY,
            in MatchStatus matchStatus,
            in float deltaTime,
            in float homeTeamOffSideLine,
            in float awayTeamOffSideLine,
            Ball ball) {

            ProcessMovement(0, in deltaTime);

            if (!matchStatus.HasFlag(MatchStatus.Playing)) {
                Stop(in deltaTime);
                return;
            }

            // 
            var ballPosition = ball.transform.position;
            var middlePoint = new Vector3(fieldEndX / 2, 0, fieldEndY / 2);
             
            var horizontalAddition = Vector3.zero; 
                horizontalAddition.z = ballPosition.z < middlePoint.z ? 
                -HORIZONTAL_DISTANCE_BY_BALL_Z_POSITION : 
                HORIZONTAL_DISTANCE_BY_BALL_Z_POSITION;

            var targetPosition = horizontalAddition + ballPosition + (middlePoint - ballPosition).normalized * 
                REFEREE_BALL_DISTANCE;

            MoveTo(in deltaTime, targetPosition, false, MovementType.Normal);
            FocusToBall(deltaTime, ball);
        }
        // 
    }
}
