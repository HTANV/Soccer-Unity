using UnityEngine;

using System;

using FStudio.Scriptables;

namespace FStudio.MatchEngine {
    [Serializable]
    public class EngineSettings : SerializedSingletonScriptable<EngineSettings> {
        [Header("Ball Control")]
        public AnimationCurve BallControlDifficultyCurveByImpactPulseCurve;
        public AnimationCurve BallControlHeightMultiplierCurve;
        public float BallControlMaxBallImpulse = 2f;

        [Tooltip("Passing height multiplier")]
        [SerializeField] private AnimationCurve passHeightDistanceCurve;
        public float PassHeight(float distance) => passHeightDistanceCurve.Evaluate (distance);

        [Tooltip("Crossing height multiplier")]
        [SerializeField] private AnimationCurve crossHeightDistanceCurve;
        public float CrossHeight(float distance) => crossHeightDistanceCurve.Evaluate(distance);

        public AnimationCurve ShootHeightByBallHeightCurve;

        public AnimationCurve CrossTargetAdditionNormalByDistance;

        public AnimationCurve CrossingBehindDistanceByFieldProgress;

        [Range (0, 1)]
        [Header("Positioning error modifier. Best to keep between 0-1, it is powerful.")]
        public float PositioningMistakeModifier = 0.1f;

        [Header ("Positioning update counter")]
        [Range (0, 20)]
        public float PositioningMistakeUpdateInPerSeconds = 3;
        
        [Header ("After min distance of long ball, long ball skill will be used both with passing skill. At max, %100 long ball skill will be applied.")]
        public float LongBallSkillActivationDistanceMin = 15;
        public float LongBallSkillActivationDistanceMax = 25;

        public float LongBallSkillPercentageAtDistance (float distance) {
            var toMin = distance - LongBallSkillActivationDistanceMin;
            var size = LongBallSkillActivationDistanceMax - LongBallSkillActivationDistanceMin;

            return Mathf.Clamp (toMin, 0, size) / size;
        }

        [Range(0, 0.25f)]
        [Header("Player Skill Multipliers")]
        [Tooltip("(PlayerSkill) Accerelation modifier")]
        public float StrengthModifier = 1;

        [Range(0, 0.25f)]
        [Header ("Player Skill Multipliers")]
        [Tooltip("(PlayerSkill) Accerelation modifier")]
        public float AccerelationModifier = 0.05f;

        [Range(0f, 0.25f)]
        [Tooltip("Top speed modifier")]
        public float TopSpeedModifier = 0.05f;

        [Tooltip("(PlayerSkill) Agility modifier")]
        [Range(0, 10f)]
        public float AgilityModifier = 1f;

        [Tooltip("(PlayerSkill) Shooting modifier")]
        [Range(0, 10f)]
        public float ShootingModifier = 1f;

        [Tooltip("(PlayerSkill) Passing modifier")]
        [Range(0, 10f)]
        public float PassingModifier = 1f;

        [Tooltip("(PlayerSkill) Passing modifier")]
        [Range(0, 10f)]
        public float DribbleModifier = 1f;

        [Header("AI Shoot Tolerance")]
        [Range(0f, 100f)]
        [Tooltip("AI will roll numbers between 0 and 100. If AI_ShootTolerance is bigger than it, it will decide to shoot.")]
        [SerializeField] private float AI_ShootTolerance = 25f;
        [SerializeField] private AnimationCurve AI_ShootToleranceDistanceCurveMod;
        [SerializeField] private AnimationCurve AI_ShootToleranceDividerAngleCurveMod;
        [SerializeField] private AnimationCurve AI_DistanceToAngleCurveMod;
        
        /// <summary>
        /// Roll values to get if shoot is preferred.
        /// </summary>
        /// <param name="angle">Angle with the goal net</param>
        /// <param name="distance">Distance to the goal net</param>
        /// <returns></returns>
        public bool ShootRoll (in float angle, in float distance, in float toleranceMod = 1) {
            var distanceMod = AI_ShootToleranceDistanceCurveMod.Evaluate(distance);

            var angleModdedByDistance = angle * AI_DistanceToAngleCurveMod.Evaluate(distance);

            var angleDivider = AI_ShootToleranceDividerAngleCurveMod.Evaluate(angleModdedByDistance);

            var roller = AI_ShootTolerance * toleranceMod * distanceMod / angleDivider;

            return UnityEngine.Random.Range(0f, 100f) < roller;
        }

        [Header ("Min Ball Velocity")]
        [Range (1f, 100f)]
        public float Ball_MinHitVelocity = 5f;


        [Header ("Tackling diffculty, more less, more success tackling")]
        [Range(1f, 25f)]
        public float Tackling_Difficulty = 2f;

        [Header("Player Run Angle Mod")]
        public float Angle_Power = 1.25f;
        public float Angle_Multi = 0.002f;
        public float Angle_Distance_Power = 0.8f;
        public AnimationCurve Angle_PlayerProgress;

        [Header ("Passing (throught)")]
        public AnimationCurve PassingAngleCurve;
        public float PassingPlayerVelocityModifier = 0.25f;
        public float PassingDistancePlayerVelocityModifier = 0.01f;
        public float PassingBackwardMaxDistance = 20f;
        public float PassingOptionPriorityPower = 5f;
        public float PassingOptionDistanceToPriority = 0.1f;
        public float PassingCrossPriority = 10f;
        public float MinimumPassDistance = 3f;
        public float PassingFieldBoundCheck = 5f;
        public float PassingMiddlePriority = 0.2f;
        public float XPowerPow = 1;

        public ThroughtPassOption[] ThroughtPassOptions;

        [Header ("Pass Power")]
        /// <summary>
        /// When a player want to pass another one, the angle to the target player is important
        /// <para>The angle will be calculated by TargetPlayer.Pos - PasserPos & TargetPlayer.Direction</para>
        /// </summary>
        public AnimationCurve PassPowerDistanceCurve;
        public AnimationCurve PassPowerReceiverSpeedCurve;
        public float PassPowerCrossMod = 1.5f;

        [Header ("Pass Power Angled")]
        public AnimationCurve PassPowerByPassAngleCurve;
        public AnimationCurve PassPowerByAngledPassDistanceCurve;

        [Header("CanSee")]
        public AnimationCurve CanSeeSecureAngleModifierByBallProgressCurve;
        public float CanSeeSecureAngleBetweenPasserAndThread = 20;
        public float CanSeePredictPositionVelocityMod = 3;
        public AnimationCurve CanSeeThreadDistanceAdditionByBallProgress;
        public AnimationCurve CanSeeAngleModByDistanceCurve;
        public AnimationCurve PassingCrossBlockApproveDistanceToTargetByDistance;
        public AnimationCurve PassingCrossBlockApproveDistanceToPasserByDistance;

        [Header("Curved ball settings")]
        public float CurvedBallHitPositionVectorModifier = 0.25f;
        public float CurvedBallHitDirectionLerper = 0.25f;

        [Header("Direction Error settings")]
        public bool IsDirectionErrorEnabled;
        public AnimationCurve DirectionErrorModByVelocityCurve;
        public AnimationCurve DirectionErrorSkillModCurve;

        [Header("Shooting velocity")]
        public float ShootingForwardAxisMultiplier = 2f;
        public float ShootingUpAxisDistanceMultiplier = 4f;
        public AnimationCurve ShootPowerByDistanceCurve;
        public AnimationCurve ShootPowerBySkillCurve;
        public float ShootingBlockAngle = 10;
        public AnimationCurve ShootErrorRemoveByDistance;

        [Header("PlayerSettings")]
        public float MinimumDistanceToTeammates = 3.5f;

        [Header("RunForwardBehaviour")]
        public AnimationCurve[] RunningForwardCurves;
        public float RunForwardLerpSpeed = 1;
        public AnimationCurve RunForwardLerpCurve;
        public float RunForwardAngleLerpMod = 20f;

        public AnimationCurve AvoidanceCurve;

        [Header("BallChasingBehaviour")]
        [Tooltip ("When a AI player wants to chase the ball, we will make it harder by distance")]
        public AnimationCurve BallChasingChaserToBallDistanceAdditionCurve;

        [Header("Slight movement by angle difference")]
        public float BallMagnetRadius = 2f;
        public float BallMagnetPower = 1f;

        public float AgileToDirectionWhenHoldingBallModifier = 2;

        [Header ("Agile to direction")]
        public AnimationCurve AgileToDirectionAngleDifferencyHardness;
        public AnimationCurve AgileToDirectionMoveSpeedHardness;

        [Header("BestOptionToTargetPoint (When someone have the ball)")]
        public AnimationCurve BestOptionToTargetMaxDistanceByBallProgressCurve;
        public float BestOptionToTargetGKAddition = -10;

        [Header("BallChasing GK addition")]
        public float BallChasingDistanceGKAddition = 15;
        
        [Header("JoinTheAttackCurves")]
        public AnimationCurve[] JoinTheAttackCurves; 
    }
}
