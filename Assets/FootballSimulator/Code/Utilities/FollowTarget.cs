
using UnityEngine;

namespace FStudio.Utilities {
    public class FollowTarget : MonoBehaviour {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        [SerializeField] private bool ignoreY;
        
        public void SetTarget (Transform target) {
            this.target = target;
        }

        private void Update() {
            if (target != null) {
                var targetPosition = target.position;
                if (ignoreY) {
                    targetPosition.y = transform.position.y;
                }
                transform.position = targetPosition + offset;
            }
        }
    }
}
