
using UnityEngine;

using System.Collections.Generic;

using System.Linq;

using FStudio.Input;
using FStudio.Utilities;
using FStudio.Graphics.Cameras;
using FStudio.UI.Navigation;
using UnityEngine.UI;
using System.Threading.Tasks;

using FStudio.UI.Utilities;

namespace FStudio.UI.GamepadInput {
    public class SnapManager : SceneObjectSingleton<SnapManager> {
        private class LAYER {
            public List<ISnappable> List = new List<ISnappable>();

            private ISnappable m_LastPointed;
            public ISnappable LastPointed {
                get => m_LastPointed;
                set {  
                    m_LastPointed = value;
                    Debug.Log($"LastPointed on layer {layer} assigned {value}", value != null ? value.gObject : null);
                }
            }

            public bool IsEnabled;

            private readonly int layer;

            public LAYER (int layer) {
                this.layer = layer;
            }
        }

        private bool isAllowed = true;

        private bool m_isEnabled;

        public bool IsEnabled {
            get => m_isEnabled;
            private set {
                m_isEnabled = value;
                gamepadCursorPanel.SetActive(value);
            }
        }

        private bool isActivateable {
            get {
                // platform dependent.
                var schema = inputListener.PlayerInput.currentControlScheme;

                switch (schema) {
                    case "XBoxController": 
                    case "Gamepad": 
                    case "Joystick":
                    case "Keyboard&Mouse":
                        return true;
                }

                return false;
            }
        }


        private const int MAX_LAYERS = 32;
        private const float SNAP_FOR_SECONDS = 2f;
        private const float SNAP_SPEED = 10f;
        private const float CURSOR_SNAP_SPEED = 10f;

        private readonly static Dictionary<byte, LAYER> layers =
            new Dictionary<byte, LAYER>();

        private ISnappable currentPointed;

        private byte currentPointedLayer;

        private Vector2 currentPointerPosition;


        [SerializeField] private bool debugActiveLayer;

        [SerializeField] private bool debugger;

        [SerializeField] private GameObject gamepadCursorPanel;
        
        [SerializeField] private Vector2 currentPointerDirection;
        [SerializeField] private RectTransform gamepadCursor;
        [SerializeField] private float snapFrequencyByDistance = 0.00015f;
        [SerializeField] private float minSnapFrequency = 0.1f;
        [SerializeField] private float maxSnapFrequency = 0.5f;
        [SerializeField] private float minInputMagnitude = 0.2f;

        [SerializeField]
        [Range(0f, 1f)]
        private float distanceOrAnglePower = 0.5f;

        [SerializeField] private float minSnapAngle = 60;

        private InputListener inputListener;

        private float nextSnap;

        private byte activeLayer;

        private ScrollRect scrollRect;
        private SnapScrollViewExtension scrollRectExtension;
        private SnapScrollViewExtension scrollHorizontalBarExtension;
        private SnapScrollViewExtension scrollVerticalBarExtension;


        private Vector2 snappingTarget;

        private float snapFor;

        public static void Clear () {
            Current.activeLayer = 0;
            Current.currentPointed = null;

            layers.Clear();

            // initialize layers.
            for (byte i = 0; i < MAX_LAYERS; i++) {
                layers.Add(i, new LAYER(i));
            }
            //
        }

        public bool IsInLayer (ISnappable snappable) {
            return layers[activeLayer].List.Contains(snappable);
        }

        public bool IsActiveLayer (byte layerId) {
            return activeLayer == layerId;
        }

        protected void Awake() {
            Clear();

            inputListener = new InputListener("UI", 0);

            inputListener.RegisterAction ("Navigate", (ctx) => {
                if (!isAllowed) {
                    return false;
                }

                currentPointerDirection = ctx.ReadValue<Vector2>();

                if (InteractiveInput.Current != null) {
                    currentPointerDirection = Vector2.zero;
                }

                if (currentPointerDirection.magnitude < minInputMagnitude) {
                    // reset snapper.
                    nextSnap = 0;
                }

                return false;
            });

            inputListener.RegisterAction("Submit", (ctx) => {
                if (!isAllowed) {
                    return false;
                }

                if (ctx.ReadValue <float>() >= 1f) {
                    Debug.Log($"Submit {Time.time}");
                    if (currentPointed != null && activeLayer == currentPointedLayer) {
                        currentPointed.OnClick();
                    }
                }

                return false;
            });
        }

        public static void Disable() {
            if (Current == null) {
                return;
            }

            Current.isAllowed = false;
        }

        public static void Enable() {
            if (Current == null) {
                return;
            }

            Current.isAllowed = true;
        }

        private static void FixNulls () {
            foreach (var layer in layers) {
                try {
                    layer.Value.List = layer.Value.List.Where(x => x != null && x.gObject != null).ToList();

                    if (layer.Value.LastPointed != null && !layer.Value.List.Contains(layer.Value.LastPointed)) {
                        layer.Value.LastPointed = null;
                    }
                } catch {
                    layer.Value.LastPointed = null;
                }
            }
        }

        public static async void Register (IEnumerable<ISnappable> snappables, 
            byte layer, 
            IEnumerable<ISnappable> snapRightAway) {

            Debug.Log($"[SnapManager] Register snap right away count {snapRightAway.Count()}");

            var targetLayer = layers[layer].List;

            layers[layer].IsEnabled = true;

            foreach (var snapTarget in snappables) {
                if (!targetLayer.Contains(snapTarget)) {
                    targetLayer.Add(snapTarget);
                }
            }

            FixNulls();

            Current.UpdateActiveLayer();

            var length = snapRightAway.Count();
            Debug.Log($"Target snappables count {length}");

            if (length > 0) {
                foreach (var snapAtAppear in snapRightAway) {
                    Debug.Log($"[Trying To Snap] {snapAtAppear}", snapAtAppear.gObject);
                    var result = await AutoSnap(snapAtAppear);
                    if (result) break;
                }
            }
        }

        public static async void UnRegister (IEnumerable<ISnappable> snappables, byte layer, bool updateSnap) {
            var targetLayer = layers[layer].List;

            int objCount = targetLayer.Count;

            var currentPointed = Current.currentPointed;

            foreach (var snapTarget in snappables) {
                if (currentPointed == snapTarget) {
                    Current.currentPointed = null;
                }

                if (targetLayer.Contains(snapTarget)) {
                    targetLayer.Remove (snapTarget);
                }
            }

            if (updateSnap) {
                FixNulls();
            }

            if (targetLayer.Count == 0) {
                layers[layer].IsEnabled = false;
            }

            Current.UpdateActiveLayer();

            if (updateSnap && objCount > 0 && Current.IsEnabled) {
                await AutoSnap(null, false);
            }
        }

        /// <summary>
        /// Try to snap on the given target.
        /// Won't succeeded if given snappable is not in top layer.
        /// </summary>
        /// <param name="snapTo"></param>
        public static async Task<bool> AutoSnap (ISnappable snapTo, bool force = false) {
            if (snapTo != null && Current.debugger) {
                Debug.Log($"Snapping to {snapTo}", snapTo.gObject);
            }

            var topLayer = layers[Current.activeLayer];

            if (topLayer == null || topLayer.List.Count == 0) {
                return false;
            }

            if (Current.debugger) {
                Debug.Log($"Top layer {Current.activeLayer}");
                Debug.Log($"Top layer member count => {topLayer.List.Count}");
            }

            ISnappable snapTarget = null;

            if (snapTo != null &&
                (force || snapTo.isSnappable) &&
                topLayer.List.Contains (snapTo)) {
                snapTarget = snapTo;
                
                if (Current.debugger) {
                    Debug.Log("Snap target approved.");
                }

            } else {
                if (snapTo != null) {
                    if (Current.debugger) {
                        Debug.Log("Snap target declined.");
                    }

                    return false;
                }
            }

            if (snapTarget == null && 
                Current.currentPointed != null &&
                Current.currentPointed.gObject.activeInHierarchy &&
                Current.activeLayer == Current.currentPointedLayer) {
                // no need for auto snap.

                if (Current.debugger) {
                    Debug.Log("No needed for auto snap");
                }

                return false;
            }

            var oldPointed = Current.currentPointed;

            if (oldPointed != null && oldPointed.gObject != null) {
                Debug.Log($"[SnapManager] Old pointed leaving.", oldPointed.gObject);
                layers[Current.currentPointedLayer].LastPointed = oldPointed;
                oldPointed.OnSnapLeft();
            }

            Current.currentPointed = null; // discard.

            if (Current.debugger) {
                Debug.Log($"Last Pointed: {topLayer.LastPointed}",
                    topLayer.LastPointed?.gObject != null ? topLayer.LastPointed.gObject : null);
            }

            if (snapTarget == null && 
                topLayer.LastPointed != null &&
                topLayer.List.Contains(topLayer.LastPointed)) {

                snapTarget = topLayer.LastPointed;
            }

            for (int i = 0; i < 1000; i++) { // total 2 (1000*2 (below)) seconds.
                if (snapTarget == null) {
                    snapTarget = topLayer.List.Where(x => x != null && x.isSnappable && x != oldPointed).
                        OrderBy(x => Vector2.Distance(x.position, Current.currentPointerPosition)).FirstOrDefault();
                }

                if (snapTarget != null) {
                    break;
                } else {
                    // wait.
                    await Task.Delay(2); // wait a little bit.
                }
            }

            if (snapTarget != null) {
                if (Current.debugger) {
                    Debug.Log($"AUTO Snap to {snapTarget}", snapTarget.gObject);
                }

                Current.Snap(Time.time, UICamera.Current.Camera, snapTarget);
            }

            return true;
        }

        private void UpdateActiveLayer () {
            activeLayer = layers.
                Where(x => x.Value.IsEnabled).
                OrderByDescending(x => x.Key).
                Select (x=>x.Key).
                FirstOrDefault();

            Debug.Log($"[SnapManager] Active layer: {activeLayer}");
        }

        private void SnapScrollRect (in float time) {
            if (scrollRect != null && currentPointed != null) {
                if (time > snapFor || scrollRectExtension.IsDraggingByUser || scrollHorizontalBarExtension?.IsDraggingByUser == true || scrollVerticalBarExtension?.IsDraggingByUser == true) {
                    scrollRect = null;
                    return;
                }

                const float CUT = 0.002f;

                if (scrollRect.verticalScrollbar?.size > 1 - CUT || 
                    scrollRect.horizontalScrollbar?.size > 1 - CUT) {

                    return;
                }

                snappingTarget =
                    NavigationScrollSnapper.CalculateSnapPosition
                    (scrollRect, currentPointed.position, scrollRect.horizontal);

                var currentAnchored = scrollRect.content.anchoredPosition;

                var verticalScrollBar = scrollRect.verticalScrollbar;
                var horizontalScrollBar = scrollRect.horizontalScrollbar;

                scrollRect.CalculateLayoutInputHorizontal();
                scrollRect.CalculateLayoutInputVertical();
                scrollRect.SetLayoutHorizontal();
                scrollRect.SetLayoutVertical();

                var difference = snappingTarget - currentAnchored;


                if (verticalScrollBar != null) {
                    bool needsVerticalFix = false;

                    if (verticalScrollBar.value > 1- CUT) {
                        if (difference.y < 0) {
                            needsVerticalFix = true;
                        }
                    }

                    if (verticalScrollBar.value < CUT) {
                        if (difference.y > 0) {
                            needsVerticalFix = true;
                        }
                    }

                    if (needsVerticalFix) {
                        snappingTarget.y = currentAnchored.y;
                    }
                }

                if (horizontalScrollBar != null) {
                    bool needsHorizontalFix = false;
                    
                    if (horizontalScrollBar.value > 1 - CUT) {
                        if (difference.x < 0) {
                            needsHorizontalFix = true;
                        }
                    }

                    if (horizontalScrollBar.value < CUT) {
                        if (difference.x > 0) {
                            needsHorizontalFix = true;
                        }
                    }

                    if (needsHorizontalFix) {
                        snappingTarget.x = currentAnchored.x;
                    }
                }

                float dt = Time.deltaTime * SNAP_SPEED;

                var target = Vector2.Lerp(currentAnchored, snappingTarget, dt);

                scrollRect.content.anchoredPosition = target;
            }
        }

        private void UpdateSnapPosition (Camera mainCam) {
            if (currentPointed == null || currentPointed.gObject == null) {
                return;
            }

            var screenPos = mainCam.WorldToScreenPoint(currentPointed.position);

            currentPointerPosition = screenPos;

            var currentAnchored = gamepadCursor.anchoredPosition;
            gamepadCursor.anchoredPosition = Vector3.Lerp (currentAnchored, screenPos, Time.deltaTime * CURSOR_SNAP_SPEED); // LERP

            // parent snappable fixer.
            var parentSnappable = currentPointed.
                gObject.
                GetComponentsInParent<ISnappable>().
                Where(x => x.gObject != currentPointed.gObject).
                FirstOrDefault();

            if (parentSnappable != null) {
                screenPos = mainCam.WorldToScreenPoint(parentSnappable.position);
                currentPointerPosition = screenPos;
            }
            //
        }

        private void Update() {
            if (IsEnabled) {
                if (!isAllowed) {
                    IsEnabled = false;
                } else if (!isActivateable) {
                    IsEnabled = false;
                }
            } else {
                if (isAllowed && isActivateable) {
                    IsEnabled = true;
                }
            }

            if (debugActiveLayer) {
                debugActiveLayer = false;

                Debug.Log("active layer: " + activeLayer);

                Debug.Log("active snapped: " + currentPointed);

                foreach (var snappable in layers[activeLayer].List) {
                    Debug.Log(snappable.gObject + " is interactable: " + snappable.isSnappable, snappable.gObject);
                }
            }


            float time = Time.time;

            SnapScrollRect(in time);


            if (!IsEnabled) {
                return;
            }

            var mainCam = UICamera.Current.Camera;

            UpdateSnapPosition(mainCam);

            if (nextSnap > time) {
                return;
            }

            if (mainCam == null) {
                return;
            }

            if (this.activeLayer >= layers.Count) {
                return; // no layers found.
            }

            var m_activeLayer = layers[this.activeLayer];

            var pointerVector = currentPointerDirection;

            if (currentPointed != null && currentPointed.blockHorizontalNavigation) {
                pointerVector.x = 0;
            }

            if (minInputMagnitude > pointerVector.magnitude) {
                return;
            }

            if (debugger) {
                Debug.Log(currentPointerDirection);
            }

            if (currentPointed != null && debugger) {
                Debug.Log($"Searching snap point, current snapped {currentPointed}", currentPointed.gObject);
            }

            var activeSnaps = m_activeLayer.List.
                    Where(x => x != null && x.isSnappable && x != currentPointed);

            var targets = activeSnaps.
                Where(x => x != currentPointed).
                Select (x => (snapAngle (x),x)).
                Where (x=>x.Item1 >= 0). // eleminate non zero.
                OrderBy(x => x.Item1).
                Select (x=>x.x);

            if (debugger) {
                Debug.Log(m_activeLayer.List.Count + " avail: "+activeSnaps.Count());
                foreach (var avail in targets) {
                    Debug.Log($"Available target: {avail.gObject}", avail.gObject);
                }
            }

            var target = targets.FirstOrDefault();

            if (target != null) {
                Snap(time, mainCam, target);
            }

            float snapAngle (ISnappable snappable) { 
                var screenPos = mainCam.WorldToScreenPoint(snappable.position);

                var distance = Vector2.Distance(currentPointerPosition, screenPos);

                var dir = new Vector2 (screenPos.x, screenPos.y) - currentPointerPosition;

                var angle = Mathf.Abs(Vector2.SignedAngle(pointerVector, dir));

                if (minSnapAngle < angle) {
                    return -1; // ignore.
                }

                return distance * (1-distanceOrAnglePower) + angle * distanceOrAnglePower;
            }
        }

        private void Snap (in float time, Camera mainCam, ISnappable target) {
            // snap completed.
            if (currentPointed != null) {
                // register as last pointed on its layer.
                layers[currentPointedLayer].LastPointed = currentPointed;
                currentPointed.OnSnapLeft();
            }

            Current.currentPointedLayer = Current.activeLayer;

            Debug.Log($"Snapped: {target}", target.gObject);

            currentPointed = target;
            scrollRect = target.OnSnap();

            if (scrollRect != null) {
                scrollRect.scrollSensitivity = scrollRect.scrollSensitivity > 0 ? 8 : -8;

                snapFor = time + SNAP_FOR_SECONDS;

                Debug.Log("Scroll rected snap.");

                scrollRectExtension = scrollRect.GetComponent<SnapScrollViewExtension>();
                if (scrollRectExtension == null) {
                    scrollRectExtension = scrollRect.gameObject.AddComponent<SnapScrollViewExtension>();
                }

                if (scrollRect.horizontalScrollbar != null) {
                    scrollHorizontalBarExtension = scrollRect.horizontalScrollbar.GetComponent<SnapScrollViewExtension>();

                    if (scrollHorizontalBarExtension == null) {
                        scrollHorizontalBarExtension = scrollRect.horizontalScrollbar.gameObject.AddComponent<SnapScrollViewExtension>();
                    }
                } else {
                    scrollHorizontalBarExtension = null;
                }

                if (scrollRect.verticalScrollbar != null) {
                    scrollVerticalBarExtension = scrollRect.verticalScrollbar.GetComponent<SnapScrollViewExtension>();

                    if (scrollVerticalBarExtension == null) {
                        scrollVerticalBarExtension = scrollRect.verticalScrollbar.gameObject.AddComponent<SnapScrollViewExtension>();
                    }
                } else {
                    scrollVerticalBarExtension = null;
                }
            }

            var screenPos = mainCam.WorldToScreenPoint(currentPointed.position);

            // delay by distance.
            nextSnap = time +
                Mathf.Clamp(snapFrequencyByDistance * Vector2.Distance(currentPointerPosition, screenPos),
                minSnapFrequency,
                maxSnapFrequency);
        }
    }
}
