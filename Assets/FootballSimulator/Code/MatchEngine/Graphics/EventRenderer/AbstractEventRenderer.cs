using FStudio.Loaders;
using FStudio.Utilities;
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.EventRenderer {
    public abstract class AbstractEventRenderer<T> : SceneObjectSingleton<T> where T : Object {
        protected abstract Transform[] prefabs ();

        [SerializeField] protected int poolSize;
        [SerializeField] private Transform holder;
        protected Pool<Transform>[] pool;

        protected virtual void Start () {
            int length = prefabs().Length;
            pool = new Pool<Transform>[length];
            for (int i = 0; i < length; i++) {
                pool[i] = new Pool<Transform>(prefabs()[i], holder, poolSize);
            }
        }
    }
}