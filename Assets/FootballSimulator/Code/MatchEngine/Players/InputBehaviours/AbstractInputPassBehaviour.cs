
using FStudio.MatchEngine.Enums;
using UnityEngine;
using System.Linq;
using static FStudio.MatchEngine.Players.PlayerBase;
using FStudio.MatchEngine.Players.Behaviours;

namespace FStudio.MatchEngine.Players.InputBehaviours {
    public abstract class AbstractInputPassBehaviour : BaseBehaviour, IInputBehaviour {
        private readonly Vector2 CORNER_cornerBox = new Vector2(8, 16);

        private const float CORNER_TO_PENALTY_BOX = 9.15f;

        private const float CORNER_TO_TARGET = 0.8f;

        private const float CORNER_DIRECTION_MULTIPLIER = 7;

        private const float START_ANGLE = 5;

        private const float INCLUDE_ANGLE = 5;

        public abstract PassType passType { get; }

        public Vector3 InputDirection { set; private get; }

        private Vector3 cornerTarget;

        private PassTarget passTarget;

        public bool IsTriggered { private get; set; }

        public override bool Behave(bool isAlreadyActive) {
            if (!IsTriggered && !isAlreadyActive) {
                return false;
            }
            
            if (!Player.isInputControlled) {
                IsTriggered = false;
                return false;
            }

            if (ball.HolderPlayer != Player) {
                IsTriggered = false;
                return false;
            }

            IsTriggered = false;

            // check corner.
            if (Player.IsCornerHolder) {
                var penaltyPoint = targetGoalNet.Position;
                penaltyPoint -= Player.GoalDirection * CORNER_TO_PENALTY_BOX;

                cornerTarget = penaltyPoint + new Vector3(
                    Random.Range(-CORNER_cornerBox.x, CORNER_cornerBox.x),
                    0,
                    Random.Range(-CORNER_cornerBox.y, CORNER_cornerBox.y)
                );

                var closest = teammates.OrderBy(x => Vector3.Distance(x.Position, cornerTarget)).FirstOrDefault();
                cornerTarget = Vector3.Lerp(cornerTarget, closest.Position, CORNER_TO_TARGET) + CORNER_DIRECTION_MULTIPLIER * (closest.Position - Player.Position).normalized;

                Player.Cross(cornerTarget);

                // teammates chase the ball directly.
                foreach (var e in Player.GameTeam.GamePlayers) {
                    if (e.PlayerFieldProgress > 0.8f) {
                        e.ActivateBehaviour("BallChasingWithoutCondition");
                    }
                }

                return true;
            }

            if (!passTarget.IsValid) {
                var targetGoalNetPosition = targetGoalNet.Position;

                var distanceToTargetGoalNet = Vector3.Distance(Player.Position, targetGoalNetPosition);

                var activeAngle = START_ANGLE;

                passTarget = default;

                var inputDir = InputDirection;
                var pos = Player.Position;

                while (!passTarget.IsValid && activeAngle < 180) {
                    var target = teammates.Where (x=>x!= Player).Select(x =>(x,
                        Mathf.Abs(
                        Vector3.SignedAngle(inputDir, (x.Position - pos).normalized, Vector3.up)))).
                        Where (x=>x.Item2 <activeAngle).OrderBy(x=>x.Item2).FirstOrDefault();

                    if (target.x == null) {
                        activeAngle += INCLUDE_ANGLE;
                        continue;
                    }

                    var passPositions = Player.FindPassPositions(target.x).Where (x=>x.enableUserInput);
                    var validPasses = passPositions.Where(x => x.passTypes.Contains (passType));

                    if (!validPasses.Any()) {
                        activeAngle += INCLUDE_ANGLE;
                        continue;
                    }

                    var _found = validPasses.OrderByDescending(x => x.priority).FirstOrDefault();

                    var actualTargetPosition = _found.actualTarget.Position;
                    var ourPosition = Player.Position;
                    var usToActualTarget = actualTargetPosition - ourPosition;
                    var usToPoint = _found.position - ourPosition;
                    var angle = Mathf.Abs(Vector3.SignedAngle(usToActualTarget, usToPoint, Vector3.up));

                    var m_angle = EngineSettings.Current.PassPowerByPassAngleCurve.Evaluate(angle);
                    var m_distance = EngineSettings.Current.PassPowerByAngledPassDistanceCurve.Evaluate(usToPoint.magnitude);
                    var passPowerMod = m_angle * m_distance;

                    passTarget = new PassTarget (_found.passTypes.FirstOrDefault(), _found.optionName, _found.position, _found.actualTarget, passPowerMod);
                }

                if (!passTarget.IsValid) {
                    isAlreadyActive = false;
                } else {
                    Debug.Log($"[InputPassBehaviour] OptionName: {passTarget._OptionName}");
                    Debug.Log(passTarget, passTarget._ActualTarget.PlayerController.UnityObject);
                }
            }

            if (isAlreadyActive) {
                Player.CurrentAct = Acts.InputPass;

                Player.GameTeam.KeepPlayerBehavioursForAShortTime();

                Player.Stop(in deltaTime);

                if (Player.LookTo(in deltaTime, passTarget._Position - Player.Position)) {
                    // set pass target. after pass target player will behave with BallChasingBehaviour.
                    Player.PassingTarget = passTarget._ActualTarget;

                    float speedMod = Player.SpeedModForPassing();

                    if (passType == PassType.LongPass) {
                        var dir = passTarget._Position - Player.Position;
                        var add = EngineSettings.Current.CrossTargetAdditionNormalByDistance.Evaluate(dir.magnitude);

                        var crossAddition = dir.normalized * add * passTarget._PassPower;

                        Player.Cross(passTarget._Position + crossAddition);
                    } else {
                        Player.Pass(passTarget._Position, speedMod * passTarget._PassPower);
                    }

                    passTarget = default; // reset target.
                }

                return true;
            }

            return false;
        }
    }
}
