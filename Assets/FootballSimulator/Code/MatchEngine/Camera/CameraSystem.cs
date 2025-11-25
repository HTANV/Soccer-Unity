using UnityEngine;
using FStudio.Utilities;
using FStudio.MatchEngine.Balls;
using FStudio.Events;
using FStudio.UI.Events;
using FStudio.MatchEngine.Events;
using System.Threading.Tasks;

namespace FStudio.MatchEngine.Cameras {
    [RequireComponent (typeof (Camera))]
    public class CameraSystem : SceneObjectSingleton<CameraSystem> {
        public new Camera camera = default;

        [SerializeField] private SerializableAssetCollection<string, MatchCamera> matchCameras = 
            new SerializableAssetCollection<string, MatchCamera>();

        [SerializeField] public Transform target = default;

        [SerializeField] private float positionDifferencePower = 0.25f;
        [SerializeField] private float rotationDifferencePower = 0.25f;

        public float ZoomMultiplier;

        private bool isInTransition = false;
        private float transitionValue = 1;

        private bool instantTransitionInNextFrame = false;

        [SerializeField] private float transitionSpeed = 0.5f;


        [Header("Camera Speed")]
        public float CameraPositionSpeed = 4;
        public float CameraRotationSpeed = 20;
        public float CameraZoomSpeed = 4;

        public MatchCamera CurrentCamera { get; private set; }

        public string CurrentCameraType { get; private set; }

        public Vector3? TargetPosition;

        private void ActiveCameraChanged () {
            if (target == null) {
                EventManager.Trigger<MatchCameraActiveEvent>(null);
            } else {
                EventManager.Trigger(new MatchCameraActiveEvent());
            }
        }

        public void SetTarget(Transform target) {
            Debug.Log($"[SetTarget] {target}");
            this.target = target;
            ActiveCameraChanged();
        }

        public async Task SwitchCamera(string cameraType) {
            Debug.Log($"[CameraSystem] Switch Camera: {cameraType}");
            CurrentCamera = await matchCameras.FindAsync(cameraType);
            CurrentCameraType = cameraType;
            isInTransition = false;
            instantTransitionInNextFrame = true; 

            ActiveCameraChanged();
        }

        private async void Start() {
            await SwitchCamera("Stadium"); // default camera.
        }

        /// <summary>
        /// Make the transition instant.
        /// </summary>
        public void FocusToBall () {
            SetTarget(Ball.Current.transform);
            TargetPosition = null;

            instantTransitionInNextFrame = true;
            isInTransition = false;
        }

        private void OnValidate() {
            camera = GetComponent<Camera>();
        }

        private void Update() {
            if (target == null) {
                return;
            }

            var dT = Time.fixedDeltaTime;

            if (isInTransition) {
                transitionValue += dT * transitionSpeed;

                if (transitionValue >= 1 ) {
                    isInTransition = false;
                }
            } else {
                transitionValue = 1;
            }

            if (CurrentCamera != null) {
                Vector3 targetPos;
                if (TargetPosition.HasValue) {
                    targetPos = TargetPosition.Value;
                } else {
                    if (target == null) {
                        return;
                    }

                    targetPos = target.position;
                }

                var (position, rotation, zoom) = CurrentCamera.Behave(in dT, targetPos);

                
                if (instantTransitionInNextFrame) {
                    instantTransitionInNextFrame = false;

                    transform.position = position;
                    transform.rotation = rotation;
                    camera.fieldOfView = zoom / (ZoomMultiplier + 1);
                } else {
                    var positionDifference = (Vector3.Distance(transform.position, position)+1) * transitionValue;
                    var rotationDifference = (1+ Quaternion.Angle(transform.rotation, rotation)) * transitionValue;
                    transform.position = Vector3.Lerp(transform.position, position, dT * CameraPositionSpeed * positionDifference * positionDifferencePower);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, dT * CameraRotationSpeed * rotationDifference * rotationDifferencePower);

                    camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom / (ZoomMultiplier + 1), dT * CameraZoomSpeed);
                }
            }
        }
    }
}