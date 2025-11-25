using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FStudio.Utilities {
    [Serializable]
    public class AssetEntry<Key, Value> where Value : UnityEngine.Object {
        public Key Id;
        public AssetReferenceT<Value> Val;
    }

    /// <summary>
    /// Creates a serializable key/value collection on Unity behaviours.
    /// Also features resizing to fix a key value collection.
    /// 
    /// <para>This collection works with addressable assets. When you request a value from this collection,
    /// it will use LoadAssetAsync before returning to you, since the values need to be loaded.</para>
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    [Serializable]
    public class SerializableAssetCollection <Key, Value> where Value : UnityEngine.Object {
        public AssetEntry<Key, Value>[] Entries;

        private Key lastLoadedKey;

        /// <summary>
        /// Release an instantiated. 
        /// This is like Object.Destroy if you used Instantiate() to instantiate the value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="instance"></param>
        public void ReleaseInstantiated (Key key, GameObject instance) {
            for (int i = 0, length = Entries.Length; i < length; i++) {
                if (Entries[i].Id.Equals(key)) {
                    Entries[i].Val.ReleaseInstance(instance);
                }
            }
        }

        /// <summary>
        /// Release the last loaded asset.
        /// </summary>
        public void Release () {
            for (int i = 0, length = Entries.Length; i < length; i++) {
                if (Entries[i].Id.Equals(lastLoadedKey)) {
                    Entries[i].Val.ReleaseAsset ();
                }
            }
        }

        public void ReleaseAll () {
            for (int i = 0, length = Entries.Length; i < length; i++) {
                Entries[i].Val.ReleaseAsset();
            }
        }

        /// <summary>
        /// Finds and loads value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Value> FindAsync (Key key) {
            lastLoadedKey = key;

            for (int i=0, length = Entries.Length; i<length; i++) {
                if (Entries[i].Id.Equals ( key )) {
                    var item = await Addressables.LoadAssetAsync<Value>(Entries[i].Val).Task;
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds and instantiated the value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<GameObject> Instantiate (Key key, Transform holder) {
            lastLoadedKey = key;

            for (int i = 0, length = Entries.Length; i < length; i++) {
                if (Entries[i].Id.Equals(key)) {
                    var item = await Entries[i].Val.InstantiateAsync (holder).Task;
                    return item;
                }
            }

            return null;
        }

        public void Resize (Key[] keys) {
            var itemCount = keys.Length;

            if (Entries == null) {
                Entries = new AssetEntry<Key, Value>[0];
            }

            if (Entries.Length != itemCount) {
                Array.Resize(ref Entries, itemCount);

                for (int i = 0; i < itemCount; i++) {
                    if (Entries[i] == null) {
                        Entries[i] = new AssetEntry<Key, Value>();
                    }
                }
            }

            // fix enum
            for (int i = 0; i < itemCount; i++) {
                Entries[i].Id = keys[i];
            }
            //
        }
    }
}
