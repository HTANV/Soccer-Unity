using FStudio.MatchEngine.Players.Behaviours;
using System;
using System.Collections.Generic;

using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;

using UnityEngine;

using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace FStudio.MatchEngine.Players.Referee {
    public class LinemanReferee : AbstractReferee {
        public bool Side;

        public LinemanReferee(MatchPlayer matchPlayer, GameTeam gameTeam, Material kitMaterial) : base(matchPlayer, gameTeam, kitMaterial)
        {
            PlayerController.SetAsLineReferee();
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

            float minX = 0;
            float maxX;
            float z = -0.5f;
            float targetOffsideLine = !Side ? homeTeamOffSideLine : awayTeamOffSideLine;

            Vector3 lookDirection;

            if (!Side) {
                // this will be second half right side referee.
                minX = fieldEndX / 2;
                maxX = fieldEndX;
                z = -0.5f;
                lookDirection = Vector3.forward;
            } else {
                maxX = fieldEndX / 2;
                z = fieldEndY + 0.5f;
                lookDirection = -Vector3.forward;
            }

            var targetPosition = new Vector3(Mathf.Clamp(targetOffsideLine, minX, maxX), 0, z);

            LookTo(in deltaTime, lookDirection);
            MoveTo(in deltaTime, targetPosition, false);
        }
        // 
    }
}
