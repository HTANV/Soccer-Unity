using System;

using UnityEngine;

using FStudio.MatchEngine.FieldPositions;
using FStudio.Data;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Tactics {
    [Serializable]
    public class TeamTactics : ScriptableObject {
        [SerializeField] private ManagerTactics managerTactics;

        [SerializeField]
        private AnimationCurve sliderPowerCurveByDistanceToTarget;

        [Header ("BaseSettings")]
        [Range(0, 1)]
        [SerializeField]
        private float verticalDispersion = 0.5f;

        [Range(0, 1)]
        [SerializeField]
        private float
            defenseHorizontalDispersion = 0.5f,
            midfieldHorizontalDispersion = 0.5f,
            attackHorizontalDispersion = 0.5f;

        [Range (0, 1f)]
        [SerializeField]
        private float fullBacksPositioningAddition = 0;

        [Header("In meters type, distance to keeping close to the marking target")]
        public AnimationCurve MarkingDistanceByPlayerFieldProgress;

        [Range(0, 1f)]
        [SerializeField]
        private float wingPositioningAddition = 0;

        [Range (0, 100)]
        [SerializeField]
        private int passAndRunChance = 20;

        [SerializeField] private AnimationCurve passAndRunChanceModifierByBallProgress;

        [Header("Increasing this will increase the power of JoinTheAttackBehaviour")]
        public AnimationCurve JoinAttackPowerByFieldProgressCurve;

        public bool RollPassAndRunChance (in float ballProgress) {
            return UnityEngine.Random.Range(0, 100) > 
                passAndRunChanceModifierByBallProgress.Evaluate(ballProgress) * passAndRunChance;
        }

        public BehaviourPreset GetTacticSettings(TacticPresetTypes tacticPresetType, TeamBehaviour behaviour) {
            return managerTactics.Presets.Find (tacticPresetType).behaviours.Find (behaviour);
        }

        /// <summary>
        /// Get tactical field positioning
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <returns></returns>
        public FieldPosition GetPosition (FormalPositioning fieldPositioning, in Positions playerPosition) {
            var formalPosition = fieldPositioning.GetPosition(playerPosition);
            var basePosition = PositionRules.GetBasePosition(playerPosition);

            #region positional horizontal dispersion
            float positionHorizontalDispersion = 0.5f;

            switch (basePosition) {
                case Positions.CB: 
                case Positions.RB: 
                case Positions.LB:
                    positionHorizontalDispersion = defenseHorizontalDispersion;
                    break;

                case Positions.DMF:
                    positionHorizontalDispersion = (defenseHorizontalDispersion + midfieldHorizontalDispersion) / 2;
                    break;

                case Positions.CM: 
                case Positions.RMF: 
                case Positions.LMF:
                    positionHorizontalDispersion = midfieldHorizontalDispersion;
                    break;


                case Positions.AMF:
                    positionHorizontalDispersion = (midfieldHorizontalDispersion + attackHorizontalDispersion) / 2;
                    break;

                case Positions.LW:
                case Positions.RW:
                case Positions.ST:
                    positionHorizontalDispersion = attackHorizontalDispersion;
                    break;
            }

            var distanceToMiddle = formalPosition.HorizontalPlacement - 0.5f;

            formalPosition.HorizontalPlacement += positionHorizontalDispersion * distanceToMiddle;

            formalPosition.HorizontalPlacement = Mathf.Clamp(formalPosition.HorizontalPlacement, 0, 1);
            #endregion

            #region vertical dispersion
            float toEnd = 1 - formalPosition.VerticalPlacement;
            formalPosition.VerticalPlacement += toEnd * verticalDispersion;
            #endregion

            switch (playerPosition) {
                case Positions.LB: 
                case Positions.RB:
                    formalPosition.VerticalPlacement += toEnd * fullBacksPositioningAddition;
                    break;

                case Positions.LMF: 
                case Positions.RMF:
                case Positions.LW:
                case Positions.RW:
                    formalPosition.VerticalPlacement += toEnd * wingPositioningAddition;
                    break;
            }

            return formalPosition;
        }

        /// <summary>
        /// Get in-game dynamic field position.
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <param name="teamBehaviour"></param>
        /// <param name="fieldXLength"></param>
        /// <param name="fieldYLength"></param>
        /// <param name="ballPosition"></param>
        /// <param name="markingTarget"></param>
        /// <param name="goalNet"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public Vector3 GetFieldPosition(
            in Positions playerPosition,
            in TacticPresetTypes tacticPresetType,
            in TeamBehaviour teamBehaviour,
            in float ballProgress,
            in int fieldXLength,
            in int fieldYLength,
            in Vector3 ballPosition,
            in PlayerBase markingTarget,
            GoalNet goalNet, 
            bool debug = false) {

            var tactic = GetTacticSettings(tacticPresetType, teamBehaviour);

            var formalPosition = GetPosition (FormalPositioning.Current, playerPosition);
            var playerVectorPosition = FieldPositionUtility.PositionToVector3(goalNet.Direction, in fieldXLength, in fieldYLength, formalPosition);
            Vector3 midPoint = new Vector3(fieldXLength / 2, 0, fieldYLength / 2);

            #region calculate spreading
            var verticalDistanceToMidPoint = midPoint.x - playerVectorPosition.x;
            playerVectorPosition.x += verticalDistanceToMidPoint * tactic.VerticalSuppress;

            var horizontalDistanceToMidPoint = midPoint.z - playerVectorPosition.z;
            playerVectorPosition.z += horizontalDistanceToMidPoint * tactic.HorizontalSuppress;
            #endregion

            #region calculate frontline / attacking force.
            float attackSliderValue = tactic.DefenceAttackSlider + tactic.DefenceAttackSliderAddByBallProgress.Evaluate (ballProgress);
            attackSliderValue = Mathf.Clamp(attackSliderValue, 0, 1);

            var distanceToEnd =
                goalNet.Direction.x > 0 ?
                fieldXLength - playerVectorPosition.x :
                playerVectorPosition.x;

            var distanceToEndPercentage = distanceToEnd / fieldXLength;

            var distanceToStart = goalNet.Direction.x > 0 ? 
                playerVectorPosition.x : 
                fieldXLength - playerVectorPosition.x;

            var distanceToStartPercentage = distanceToStart / fieldXLength;

            if (debug) {
                Debug.Log($"Distance to start : {distanceToStart}");
                Debug.Log($"Distance to end : {distanceToEnd}");
                Debug.Log($"Slider value  : {attackSliderValue}");
            }

            if (attackSliderValue >= 0.5f) {
                // 
                var more = attackSliderValue - 0.5f;

                var add = distanceToEnd * more * sliderPowerCurveByDistanceToTarget.Evaluate (1- distanceToEndPercentage);

                var normalAttack = formalPosition.VerticalPlacement;

                var finalMod = 0.5f + normalAttack / 2;

                playerVectorPosition.x += add * goalNet.Direction.x * finalMod;

                if (debug) {
                    Debug.Log($"More : {more}");
                    Debug.Log($"Add: {add}");
                    Debug.Log($"Result: {add * goalNet.Direction.x}");
                }

            } else {
                var more = (0.5f - attackSliderValue) * 2;

                var add = distanceToStart * more * sliderPowerCurveByDistanceToTarget.Evaluate(1- distanceToStartPercentage);

                var normalDefense = 1 - formalPosition.VerticalPlacement;

                var finalMod = normalDefense / 2f + 0.5f;

                playerVectorPosition.x -= add * goalNet.Direction.x * finalMod;

                if (debug) {
                    Debug.Log($"More : {more}");
                    Debug.Log($"Add: {add}");
                    Debug.Log($"Result: {-add * goalNet.Direction.x}");
                }
            }
            #endregion

            #region stay close to ball vertically.
            var ballX = ballPosition.x;
            playerVectorPosition.x = Mathf.Lerp(
                playerVectorPosition.x,
                ballX,
                tactic.StayCloseToBallVertically);
            #endregion

            #region closeToBall
            var toBall = ballPosition - playerVectorPosition;

            playerVectorPosition += 
                toBall * 
                tactic.PlayCloseToBallByBallProgressCurve.Evaluate (ballProgress);
            #endregion

            #region horizontal suppress by field position
            // update distance to end.
            distanceToEnd =
                goalNet.Direction.x > 0 ?
                fieldXLength - playerVectorPosition.x :
                playerVectorPosition.x;

            distanceToEndPercentage = distanceToEnd / fieldXLength;
            //

            var horizontalSuppressForward =
                tactic.HorizontalSupressGoingForwardCurve.Evaluate(1 - distanceToEndPercentage) *
                tactic.HorizontalSupressWhenGoingForward;

            var horizontalSuppressDefense =
                tactic.HorizontalSupressGoingDefenseCurve.Evaluate(distanceToEndPercentage) *
                tactic.HorizontalSupressWhenGoingDefense;

            playerVectorPosition.z += horizontalDistanceToMidPoint * horizontalSuppressForward;
            playerVectorPosition.z += horizontalDistanceToMidPoint * horizontalSuppressDefense;
            #endregion

            #region Marking
            if (markingTarget != null && markingTarget != null) {
                var markingDistance = MarkingDistanceByPlayerFieldProgress.Evaluate (1- markingTarget.PlayerFieldProgress);

                var targetPos = markingTarget.Position;

                // plus goal net dir.
                var target = targetPos + (goalNet.Position - targetPos).normalized * markingDistance;

                var markTheOpponent = tactic.MarkOpponentByBallProgress.Evaluate(1- markingTarget.PlayerFieldProgress);

                playerVectorPosition = Vector3.Lerp(playerVectorPosition, target, markTheOpponent);

                if (debug) {
                    Debug.Log (playerVectorPosition);
                }
            }
            #endregion

            return playerVectorPosition;
        }
    }
}
