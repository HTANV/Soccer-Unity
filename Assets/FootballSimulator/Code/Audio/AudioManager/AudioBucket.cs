
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AudioManager.Public {
    [CreateAssetMenu(fileName = "NewAudioBucket", menuName = "AudioManager/Create new audio bucket", order = 1)]
    public class AudioBucket : ScriptableObject {
        [SerializeField] private AssetReferenceT<AudioClip>[] clips;

        [SerializeField] private float playOffset = 0.2f;

        [System.NonSerialized] public float nextPlay;

        public float Volume = 1;

        public AudioClip[] LoadedClips { private set; get; }

        private bool isLoading = false;

        public bool IsLoaded { private set; get; }

        public AudioClip RandomClip() => LoadedClips[Random.Range(0, LoadedClips.Length)];

        public bool GetPlayPermission () {
            float time = Time.time;

            if (nextPlay > time) {
                return false;
            }

            nextPlay = time + playOffset;

            return true;
        }

        public void Unload () {
            if (!IsLoaded) {
                return;
            }

            int clipsLength = clips.Length;

            for (int i=0; i<clipsLength; i++) {
                clips[i].ReleaseAsset();
            }
        }

        public async Task Load () {
            if (IsLoaded) {
                return;
            }

            int clipsLength = clips.Length;

            if (clips.Length == 0) {
                Debug.LogWarning ("[AudioAsset] has no clips.");
                return;
            }

            if (isLoading) {
                return;
            }

            isLoading = true;

            LoadedClips = new AudioClip[clipsLength];

            for (int i=0; i<clipsLength; i++) {
                LoadedClips[i] = await clips[i].LoadAssetAsync<AudioClip>().Task;
            }

            isLoading = false;
            IsLoaded = true;
        }
    }
}