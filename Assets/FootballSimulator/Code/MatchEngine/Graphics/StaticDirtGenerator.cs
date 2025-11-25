
using System.IO;
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.EventRenderer {
    public class StaticDirtGenerator : AbstractEventRenderer<StaticDirtGenerator> {
        [SerializeField] private Transform[] m_prefabs;

        protected override Transform[] prefabs() => m_prefabs;

        [SerializeField] private new Camera camera;

        [SerializeField] private RenderTexture renderTexture;

        [SerializeField] private Vector3 fieldSize = new Vector3(84.5f, 0, 60);

        protected override void Start()
        {
            base.Start();

            for (int i = 0; i < poolSize; i++)
            {
                var randomPos = new Vector3(Random.Range(0, fieldSize.x), 0, Random.Range(0, fieldSize.z));
                SetPosition(Random.Range(0, prefabs().Length), randomPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }

            camera.Render();

            // save render texture to jpg.
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            var bytes = tex.EncodeToPNG();

            File.WriteAllBytes($"{Application.dataPath}/Arts/Textures/StaticDirtTextures/staticDirtTexture_{Random.Range(0, 65535)}.png", bytes);
        }

        public void SetPosition (int prefabId, Vector3 position, Quaternion rotation = default) {
            var pref = pool[prefabId].Get();
            pref.position = position;
            pref.rotation = rotation;
        }
    }
}