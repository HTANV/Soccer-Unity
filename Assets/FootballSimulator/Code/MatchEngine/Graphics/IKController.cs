using UnityEngine;
using UnityEngine.Animations.Rigging;

using FStudio.Animation;

namespace FStudio.MatchEngine.Graphics {
    [RequireComponent(typeof (Animator))]
    public class IKController : MonoBehaviour, IDynamicFloat {
        [SerializeField] private AnimationCurve weightCurve;
        [SerializeField] private float curveSpeed;

        [SerializeField] private Transform leftHand, rightHand;

        [SerializeField] private Animator animator;

        private AnimationQuery currentAnimation;

        private float weight;

        public float GetFloat() => weight;

        public void SetFloat(float dynamicFloat) => weight = dynamicFloat;

        private void OnValidate() {
            this.animator = GetComponent<Animator>();
        }

        public void LerpTo (float target) {
            if (currentAnimation != null) {
                currentAnimation.Stop();
            }

            currentAnimation = new DynamicFloatAction(this, GetFloat(), target, curveSpeed, weightCurve).GetQuery ();
            currentAnimation.Start(this, () => {
                currentAnimation = null;
            });
        }

        private void OnAnimatorIK(int layerIndex) {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);

            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }
    }
}
