
using UnityEngine;

namespace FStudio.MatchEngine.Cameras {
    public abstract class MatchCamera : ScriptableObject {
        [SerializeField] private string cameraName;
        public string CameraName => cameraName;
        public abstract (Vector3, Quaternion, float zoom) Behave(in float deltaTime, Vector3 targetPosition);
    }
}
