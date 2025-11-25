
using System;
using UnityEngine;
using System.Linq;

namespace FStudio.UI.Animators {
    [System.Serializable]
    public class UIArrayAnimation {
        [SerializeField] private UIAnimation[] animations;

        public void Play (MonoBehaviour player, Action onCompleted) {
            int count = animations.Length;

            foreach (var animation in animations) {
                animation.Play(player, () => {
                    if (--count == 0) {
                        onCompleted?.Invoke();
                    }
                });
            }
        }

        public void StopWithInstantFinish () {
            var tempAnimations = new UIAnimation[animations.Length];
            animations.ToList ().CopyTo(tempAnimations);

            foreach (var animation in tempAnimations) {
                animation.StopWithInstantFinish();
            }
        }

        public void Stop () {
            var tempAnimations = new UIAnimation[animations.Length];
            animations.ToList ().CopyTo(tempAnimations);

            foreach (var animation in tempAnimations) {
                animation.Stop ();
            }
        }
    }
}
