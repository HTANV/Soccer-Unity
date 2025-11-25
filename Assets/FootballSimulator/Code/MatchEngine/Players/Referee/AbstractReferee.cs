using FStudio.MatchEngine.Players.Behaviours;
using System;
using System.Collections.Generic;

using FStudio.MatchEngine.Graphics;

using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Referee {
    public abstract class AbstractReferee : PlayerBase {
        public AbstractReferee (MatchPlayer matchPlayer, GameTeam gameTeam, Material kitMaterial) : base (gameTeam, matchPlayer, kitMaterial) {
            // disable collision for referees.
            PlayerController.IsPhysicsEnabled = false;
        }

        public abstract void RefereeBehave(
            in int fieldEndX,
            in int fieldEndY,
            in MatchStatus matchStatus,
            in float deltaTime,
            in float homeTeamOffSideLine,
            in float awayTeamOffSideLine,
            Ball ball);

        public override void Behave(
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
                in PlayerBase[] opponents
            ) { }

        public override IEnumerable<BaseBehaviour> Behaviours {
            get => throw new NotImplementedException();
        } 
    }
}
