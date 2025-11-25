using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FStudio.Graphics {
    [RequireComponent(typeof (MeshRenderer))]
    public class PlatformDependentMaterialSwitcher : MonoBehaviour {
        [Serializable]
        private class MaterialEntry {
            public RuntimePlatform platform;
            public Material material;
        }

        [SerializeField] private Material defaultMaterial;
        [SerializeField] private List<MaterialEntry> overrides = new List<MaterialEntry>();

        // Start is called before the first frame update
        void Awake() {
            UpdateMat();
        }

        private void OnValidate() {
            UpdateMat();
        }

        void UpdateMat() {
            var runtimePlatform = Application.platform;

            var overrider = overrides.FirstOrDefault(x => x.platform == runtimePlatform);

            var mat = defaultMaterial;
            if (overrider != null) { 
                mat = overrider.material;
            }

            GetComponent<MeshRenderer>().material = mat;
        }
    }
}

