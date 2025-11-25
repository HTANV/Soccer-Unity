using UnityEngine;
using System;

namespace FStudio.MatchEngine.Tactics {
    [Serializable]
    public class BehaviourPreset {
        [Header ("Attacking / defense slider. Anchor point is the team goal net. o.5 is the normal. 0 is super defense, 1 is the full attack.")]
        [Range(0, 1)]
        public float DefenceAttackSlider;

        public AnimationCurve DefenceAttackSliderAddByBallProgress;

        [Header("Horizontal team suppress, anchor is the middle point of the field.")]
        [Range(0, 1)]
        public float HorizontalSuppress = 0.5f;

        [Header ("Vertical team suppress, anchor is the middle point of the field.")]
        [Range(0, 1)]
        public float VerticalSuppress = 0.5f;

        [Header ("Stay close to the ball vertically")]
        [Range(0, 1)]
        public float StayCloseToBallVertically = 0.2f;

        [Header ("Move to the ball both horizontally and vertically")]
        public AnimationCurve PlayCloseToBallByBallProgressCurve;

        [SerializeField]
        private AnimationCurve howManyPlayersWillChaseTheBall;
        public int HowManyPlayersWillChaseTheBall(in float ballProgress) => Mathf.RoundToInt(howManyPlayersWillChaseTheBall.Evaluate(ballProgress));

        [Header ("1 is full marking")]
        public AnimationCurve MarkOpponentByBallProgress;

        [Header("When player gets far away from middle point of the field, " +
            "there will be supressing on z position (horizontal).")]
        [Range(0,1)]
        public float HorizontalSupressWhenGoingForward = 1;
        public AnimationCurve HorizontalSupressGoingForwardCurve;

        [Header("When player gets far away from middle point of the field, " +
            "there will be supressing on z position (horizontal).")]
        [Range(0, 1)]
        public float HorizontalSupressWhenGoingDefense = 1;
        public AnimationCurve HorizontalSupressGoingDefenseCurve;
    }
}