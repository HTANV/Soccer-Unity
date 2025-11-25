using System.Threading.Tasks;
using UnityEngine;

namespace AudioManager.Public {
    public class AudioManager {
        public static float Volume = 1;

        private AudioCollection currentCollection;

        private static AudioSource m_source2D;
        private static AudioSource source2D {
            get {
                if (m_source2D == null) {
                    m_source2D = new GameObject("source2D").AddComponent<AudioSource>();
                    m_source2D.spatialBlend = 0;
                }

                return m_source2D;
            }
        }

        private bool isLoading = false;
        private bool isLoaded = false;

        public async Task LoadCollection (AudioCollection audioCollection) {
            if (isLoading) {
                return;
            }

            if (isLoaded) {
                return;
            }

            isLoading = true;

            foreach (var audio in audioCollection.Buckets) {
                await audio.Load();
            }

            currentCollection = audioCollection;

            isLoading = false;
            isLoaded = true;
        }

        public void UnloadCollection () {
            if (isLoaded) {
                foreach (var audio in currentCollection.Buckets) {
                    audio.Unload();
                }

                isLoaded = false;
            }
        }

        private AudioBucket FindInCollection (string id) {
            if (!isLoaded) {
                return null;
            }

            var bucket = currentCollection.Find(id);
            if (bucket != null) {
                return bucket;
            }
            
            Debug.LogWarning($"bucket {id} is not found in collection.");

            return null;
        }

        /// <summary>
        /// Play 2d sound by using the system default 2d Source.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        /// <returns>Lengh of the clip as seconds.</returns>
        public float Play (string id, float volume = 1) {
            var bucket = FindInCollection(id);
            if (bucket == null) {
                Debug.LogWarning($"Bucket not found {id}");
                return default;
            }

            if (!bucket.GetPlayPermission()) {
                return 0;
            }

            var clip = bucket.RandomClip();

            source2D.PlayOneShot (clip, volume * bucket.Volume * Volume);

            return clip.length;
        }

        /// <summary>
        /// Play 3D Sound.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <param name="volume"></param>
        /// <returns>Lengh of the clip as seconds.</returns>
        public float Play (string id, Vector3 position, float volume = 1) {
            var bucket = FindInCollection(id);
            if (bucket == null) {
                return default;
            }

            if (!bucket.GetPlayPermission()) {
                return 0;
            }

            var clip = bucket.RandomClip();

            AudioSource.PlayClipAtPoint(clip, position, volume * bucket.Volume * Volume);

            return clip.length;
        }

        /// <summary>
        /// Play 3D sound but by a private audio source.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="source"></param>
        /// <param name="volume"></param>
        /// <returns>Lengh of the clip as seconds.</returns>
        public float Play (string id, AudioSource source, float volume = 1) {
            var bucket = FindInCollection(id);
            if (bucket == null) {
                return default;
            }

            if (!bucket.GetPlayPermission()) {
                return 0;
            }

            var clip = bucket.RandomClip();

            source.PlayOneShot (clip, volume * bucket.Volume * Volume);

            return clip.length;
        }

        /// <summary>
        /// Returns a clip from the bucket with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AudioClip GetClip (string id) {
            var bucket = FindInCollection(id);
            if (bucket == null) {
                return default;
            }

            return bucket.RandomClip();
        }
    }
}

