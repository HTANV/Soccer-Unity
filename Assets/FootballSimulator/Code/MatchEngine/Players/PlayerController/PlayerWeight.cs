
using UnityEngine;

namespace FStudio.MatchEngine.Players.PlayerController {
	[ExecuteInEditMode]
    public class PlayerWeight : MonoBehaviour {
        [System.Serializable]
        public class BoneWeight {
            public Transform bone;
			public Vector3 defaultScale;
            public Vector3 scaleAxisByWeight;
            public float scaleMod;

            public void SetWeight (float weight) {
                bone.localScale = defaultScale + scaleAxisByWeight * scaleMod * weight;
            }
        }

        [SerializeField] private AnimationCurve weightEffectCurve;

        [SerializeField] private float weightEffect;

        [SerializeField] private BoneWeight[] boneWeights;

        public void SetWeight (float weight) {
            float effect = weightEffectCurve.Evaluate(weight) * weightEffect;

            target = weight;

            Debug.Log(effect);

            foreach (var bone in boneWeights) {
                bone.SetWeight (effect);
            }
        }

        public bool updateIt = false;
		public float target;

		public void Update () {
            if (updateIt) {
                updateIt = false;
                SetWeight(target);
            }
		}
    }
}
