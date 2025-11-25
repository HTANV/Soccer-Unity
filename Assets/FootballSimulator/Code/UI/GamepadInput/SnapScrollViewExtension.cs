using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using FStudio.Animation;

namespace FStudio.UI.GamepadInput {
    public class SnapScrollViewExtension : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IMoveHandler, IScrollHandler {        
        public bool IsDraggingByUser { private set; get; }

        private AnimationQuery currentTimer;

        public void OnBeginDrag(PointerEventData eventData) {
            Debug.Log("Drag start.");
            IsDraggingByUser = true;
        }

        public void OnEndDrag(PointerEventData eventData) {
            IsDraggingByUser = false;
            Debug.Log("Drag end.");
        }

        public void OnMove(AxisEventData eventData) {
            Debug.Log("Moving.");
            IsDraggingByUser = true;

            Timer();
        }

        public void OnScroll(PointerEventData eventData) {
            Debug.Log("Scrolling.");
            IsDraggingByUser = true;

            Timer();
        }

        private void Timer () {
            if (currentTimer != null) {
                currentTimer.Stop();
            }

            currentTimer = new TimerAction(Time.deltaTime * 2).GetQuery();
            currentTimer.Start(this, () => {
                IsDraggingByUser = false;
            });
        }
    }
}
