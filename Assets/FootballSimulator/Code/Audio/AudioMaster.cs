using UnityEngine;
using AudioManager.Public;
using FStudio.Utilities;

namespace FStudio.Audio {
    public abstract class AudioMaster<T> : SceneObjectSingleton<T> where T : Object {
        [SerializeField] protected AudioCollection audioCollection;
        public AudioManager.Public.AudioManager audioManager;

        protected bool isInitialized = false;

        protected override async void OnEnable  () {
            audioManager = new AudioManager.Public.AudioManager();
            await audioManager.LoadCollection(audioCollection);
            isInitialized = true;
        }

        protected virtual void OnDisable () {
            if (!isInitialized) {
                return;
            }

            audioManager.UnloadCollection();
        }
    }
}

