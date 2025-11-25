
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.EventRenderer {
    public class ShadowRenderer : AbstractEventRenderer<ShadowRenderer> {
        [SerializeField] private Transform[] m_prefabs;

        protected override Transform[] prefabs() => m_prefabs;

        public Transform Get () {
            return pool[0].Get();
        }
    }
}