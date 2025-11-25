using UnityEngine;
using System;
using System.Collections.Generic;

namespace FStudio.Utilities {
    public static class AnimatorEnumHasher {
        public static Dictionary<string, int> GetHashes<T> (Animator animator) where T : Enum {
            var values = Enum.GetValues(typeof(T));

            int count = values.Length - 1;

            if (animator.parameterCount != count) {
                Debug.LogWarningFormat("[AnimatorEnumHasher] Animator Parameter Count {0} doesnt match with the enum. You may need to fix it.",
                    animator.parameterCount, count);
            }

            var animatorVariableHashes = new Dictionary<string, int>();
            for (int i = 0; i < count; i++) {
                var anim = (T)(object)i;
                animatorVariableHashes.Add(anim.ToString(), Animator.StringToHash(anim.ToString()));
            }

            return animatorVariableHashes;
        }
    }
}
