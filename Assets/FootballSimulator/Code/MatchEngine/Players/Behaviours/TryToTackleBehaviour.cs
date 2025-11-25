using FStudio.MatchEngine.Enums;
using UnityEngine;
using FStudio.Data;
using System.Linq;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class TryToTackleBehaviour : BaseBehaviour {
        private const float POSITIONING_MISTAKE_MOD = 0.05f;
        private const float MAX_BALL_HEIGHT = 0.5f;

        private int index;

        public MovementType MovementType { get; private set; }

        private readonly AnimationCurve dotCurve = new AnimationCurve(new Keyframe[] {
            new Keyframe (-0.8f, 0),
            new Keyframe (-0.5f, 0.05f),
            new Keyframe (0, 0.1f),
            new Keyframe (1, 0.3f)
        });

        public override bool Behave(bool isAlreadyActive) {
            if (ball.HolderPlayer == null || 
                ball.HolderPlayer.GameTeam == Player.GameTeam
                ) {
                return false;
            }

            if (!IsRoughValidated()) {
                return false;
            }

            if (ball.transform.position.y > MAX_BALL_HEIGHT) {
                return false;
            }

            if (!isAlreadyActive) {
                var howManyPlayers = Player.
                    GameTeam.
                    Team.
                    TeamTactics.GetTacticSettings (Player.GameTeam.Team.TacticPresetType, teamBehaviour).
                    HowManyPlayersWillChaseTheBall (Player.GameTeam.BallProgress);

                if (howManyPlayers < 0) {
                    return false;
                }

                var bestOptionsToTarget = PlayerBase.BestOptionsToTargetPlayer(
                    ball.HolderPlayer,
                    teammates,
                    Player.GameTeam.BallProgress,
                    howManyPlayers
                    ).ToList ();

                index = bestOptionsToTarget.FindIndex(x => x == Player);

                if (index != -1) {
                    if (Player.IsGK) {
                        if (index == 0) {
                            isAlreadyActive = true;
                            MovementType = MovementType.BestHeCanDo;
                        } else {
                            isAlreadyActive = false;
                        }
                    } else {
                        isAlreadyActive = true;
                        MovementType = index == 0 ? MovementType.BestHeCanDo : MovementType.Normal;
                    }
                }
            }

            if (isAlreadyActive) {
                var playerPos = Player.Position;

                var holderPlayerPos = ball.HolderPlayer.Position;

                var holderDir = ball.HolderPlayer.Direction.normalized;

                Log(this, "Holder direction: " + holderDir);

                var toGoalDot = Vector3.Dot(holderDir, (goalNet.Position - holderPlayerPos).normalized);

                Log(this, $"Holder player to goal dot curve => {toGoalDot}");

                toGoalDot = dotCurve.Evaluate (toGoalDot);

                Log(this, $"Holder player to goal dot curve evaluated => {toGoalDot}");

                Log(this, $"Holder player position => {holderPlayerPos}");

                Log(this, $"Holder player {ball.HolderPlayer.PlayerController.UnityObject.name}");

                var dot = Vector3.Dot(holderDir, (holderPlayerPos - playerPos).normalized);

                Log(this, $"Go to goal net dot: {dot}");

                float goToGoalNet = dotCurve.Evaluate(dot);

                Log(this, $"Go to goal net dot evaluated: {goToGoalNet}");

                goToGoalNet *= toGoalDot;

                var predicter = PlayerBase.Predicter(Player, ball.HolderPlayer);

                Log(this, $"Predicted: {predicter}");

                predicter += 
                    Player.PositioningMistake * 
                    Vector3.Distance (predicter, playerPos) * 
                    POSITIONING_MISTAKE_MOD;

                Log(this, $"Predicter with mistake: {predicter}");

                var final = Vector3.Lerp(predicter, goalNet.Position, goToGoalNet);

                Log(this, $"Final pos before avoid: {final}");

                Player.CurrentAct = Acts.GoingToTackle;

                if (index > 0) {
                    Player.AvoidMarkers (teammates, ref final, 3);
                }

                Log(this, $"Tackle position with avoided {final}");

                // we need to go for tackling.
                Player.MoveTo(deltaTime, final, true);

                // different distance check for tackling.
                if (dot < -0.25f && Vector3.Distance(holderPlayerPos, playerPos) < PlayerBase.TACKLING_DISTANCE / 1.5f) {
                    Player.Tackle(ball);
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
