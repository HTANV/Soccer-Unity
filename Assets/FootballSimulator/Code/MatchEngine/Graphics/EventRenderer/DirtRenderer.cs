
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.EventRenderer {
    public class DirtRenderer : AbstractEventRenderer<DirtRenderer> {
        [SerializeField] private Transform[] m_prefabs;

        protected override Transform[] prefabs() => m_prefabs;

        [SerializeField] private new Camera camera;

        public void SetPosition (int prefabId, Vector3 position, Quaternion rotation = default) {
            var pref = pool[prefabId].Get();
            pref.position = position;
            pref.rotation = rotation;

            camera.Render();
        }
    }
}