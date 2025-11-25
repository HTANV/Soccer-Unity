using UnityEngine;
using System;
using FStudio.Animation;

using System.Collections.Generic;

namespace FStudio.UI.Animators {
    [Serializable]
    public class UIAnimation {
        public string Name = "AppearAnimation";

        public bool PlayAlphaAnimation;

        [Header("Alpha Animation")]
        public CanvasGroup CanvasGroup;
        public float AlphaSpeed;
        public AnimationCurve AlphaCurve;
        public float AlphaDelay;

        public bool UseStartingAlpha;
        public float StartingAlpha;

        public float TargetAlpha = 1;

        private List<AnimationQuery> animations = new List<AnimationQuery>();

        public void Stop () {
            // stop old anims.
            foreach (var animation in animations) {
                animation.Stop();
            }

            animations.Clear();
        }

        /// <summary>
        /// Stops all anims, but make them completed to target state.
        /// </summary>
        public void StopWithInstantFinish () {
            var tempAnimations = new AnimationQuery[animations.Count];
            animations.CopyTo(tempAnimations);

            foreach (var animation in tempAnimations) {
                animation.StopWithInstantFinish();
            }

            animations.Clear();
        }

        public void Play (MonoBehaviour player, Action onCompleted = null) {
            if (!player.isActiveAndEnabled) {
                return;
            }

            Stop();

            // play new ones.

            int animCount = 0;

            void checkAnimDone () {
                if (--animCount == 0) {
                    onCompleted?.Invoke();
                }
            }

            if (PlayAlphaAnimation && CanvasGroup) {
                animCount++;


                if (UseStartingAlpha) {
                    CanvasGroup.alpha = StartingAlpha;
                }

                var query = new TimerAction(AlphaDelay).GetQuery();
                animations.Add(query);

                query.Start(player, () => {
                    UIAlphaAnimation alphaAnimation = new UIAlphaAnimation(CanvasGroup);
                    var alphaAnimationQuery = new DynamicFloatAction(
                        alphaAnimation,
                        CanvasGroup.alpha,
                        TargetAlpha,
                        AlphaSpeed,
                        AlphaCurve).GetQuery();

                    animations.Add(alphaAnimationQuery);

                    alphaAnimationQuery.Start(player, checkAnimDone);
                });
            }
        }
    }
}
