
using UnityEngine;

namespace FStudio.MatchEngine.Graphics {
    [RequireComponent (typeof (Animator))]
    public class HeadLook : MonoBehaviour {
        private const float FORWARD_DIST = 2;
        private const float LERPSPEED = 10f;

        private Vector3 target;

        [SerializeField] private Animator animator;

        private float weight;

        private void OnValidate() {
            animator = GetComponent<Animator> ();
        }

        private void OnAnimatorIK (int layerIndex) {
            animator.SetLookAtWeight(weight);
            animator.SetLookAtPosition(target);
        }

        public void SetTarget (in float deltaTime, Vector3 targetWorldPosition, float weight) {
            var head = animator.GetBoneTransform(HumanBodyBones.Head).position;
            var dirToTarget = targetWorldPosition - head;
            dirToTarget.y = 0;

            // angle fix.
            var forward = transform.forward;
            var angle = Vector3.SignedAngle(forward, dirToTarget, Vector3.up);
            angle = Mathf.Clamp(angle, -40, 40);
            forward = Quaternion.Euler(0, angle, 0) * forward;
            dirToTarget = forward.normalized * FORWARD_DIST;
            //

            dirToTarget = dirToTarget.normalized * FORWARD_DIST;

            targetWorldPosition = head + dirToTarget;

            this.weight = Mathf.Lerp (this.weight, weight, deltaTime);
            target = Vector3.Lerp(target, targetWorldPosition, LERPSPEED * deltaTime);


        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(target, 1);
        }
    }
}
