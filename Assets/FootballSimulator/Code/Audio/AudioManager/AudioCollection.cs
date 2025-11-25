
using UnityEngine;

namespace AudioManager.Public {
    [CreateAssetMenu(fileName = "NewAudioCollection", menuName = "AudioManager/Create new audio collection", order = 1)]
    public class AudioCollection : ScriptableObject {
        [SerializeField] private AudioBucket[] buckets;

        public AudioBucket[] Buckets => buckets;

        public AudioBucket Find(string id) {
            for (int i = 0, length = buckets.Length; i < length; i++) {
                if (buckets[i].name == id) {
                    return buckets[i];
                }
            }

            return null;
        }
    }
}