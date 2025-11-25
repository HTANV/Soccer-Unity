
using UnityEngine;
using System.Collections.Generic;
using FStudio.MatchEngine.Graphics;
using FStudio.MatchEngine.Utilities;
using FStudio.Utilities;
using FStudio.MatchEngine.Balls;
using System.Linq;

using FStudio.MatchEngine.Graphics.EventRenderer;

namespace FStudio.MatchEngine.Players.PlayerController {
    [RequireComponent (typeof (Animator))]
    [RequireComponent (typeof (PlayerGraphic))]
    public class PlayerAnimator : MonoBehaviour {       
        private const string BALL_DRIBBLE_SPEED_ANIMATOR_FLOAT_NAME = "Speed";

        [SerializeField] private Animator animator;

        private Dictionary<string, int> animatorVariableHashes;

        [SerializeField] private IKController gkIkController;

        public IKController GKIKController => gkIkController;

        [SerializeField] private float footShadowForwardOffset = 0.2f;

        [SerializeField] private HeadLook headLook;

        [SerializeField] private Animator ballDribbleAnimator;

        [SerializeField] private PlayerBallPoint ballPoint = default;

        private Transform footLShadow, footRShadow;

        private Material footLMaterial, footRMaterial;

        public Vector3 BallPosition(PlayerBallPoint.Situation situation) => ballPoint.GetPosition(situation);
        public Quaternion BallRotation(PlayerBallPoint.Situation situation) => ballPoint.GetRotation(situation);

        public Vector3 AnimatorDirection { set; private get; }

        [SerializeField] private PlayerGraphic playerGraphic;

        private void OnDestroy() {
            if (footLShadow != null) {
                footLShadow.gameObject.SetActive(false);
            }

            if (footRShadow != null) {
                footRShadow.gameObject.SetActive(false);
            }
        }

        private void OnValidate () {
            animator = GetComponent<Animator>();
            playerGraphic = GetComponent<PlayerGraphic>();
        }

        private void Awake () {
            animatorVariableHashes = AnimatorEnumHasher.GetHashes<PlayerAnimatorVariable>(animator);
            
            footLShadow = FootShadowRenderer.Current.Get();
            footRShadow = FootShadowRenderer.Current.Get();

            footLMaterial = footLShadow.GetComponentInChildren<Renderer>().material;
            footRMaterial = footRShadow.GetComponentInChildren<Renderer>().material;
        }

        public void SetAnimatorSpeed (float f) {
            if (!gameObject.activeSelf) {
                return;
            }

            animator.speed = f;
        }

        public void SetAnimator (bool value) {
            animator.enabled = value;
        }

        public bool IsCurrentClipBlocker (in string[] blockers) {
            foreach (var anim in blockers) {
                var hash = animatorVariableHashes[anim];

                if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == hash) {
                    return true;
                }

                if (animator.GetAnimatorTransitionInfo (0).IsName ($"{anim}Transition")) {
                    return true;
                }
            }

            return false;
        }

        private void Update() {
            const float MIN_MOVE_SPEED_TO_MOVE = 0.5f;

            ballDribbleAnimator.SetFloat (
                BALL_DRIBBLE_SPEED_ANIMATOR_FLOAT_NAME, 
                GetFloat(PlayerAnimatorVariable.MoveSpeed) - MIN_MOVE_SPEED_TO_MOVE);
        }

        private void LateUpdate () {
            void setBone (HumanBodyBones boneId, Transform target, Material material) {
                const string SHADOW_POWER = "_Power";

                var bone = animator.GetBoneTransform(boneId);

                var pos = bone.position;
                target.position = bone.position + bone.forward * footShadowForwardOffset;
                target.rotation = Quaternion.Euler (90, bone.rotation.eulerAngles.y, 0);

                // ball shadow power.
                float height = pos.y;
                pos.y = 0;
                float heightPow = Mathf.Max (0, 0.15f - height);
                material.SetFloat(SHADOW_POWER, heightPow);
            }

            setBone(HumanBodyBones.LeftFoot, footLShadow, footLMaterial);
            setBone(HumanBodyBones.RightFoot, footRShadow, footRMaterial);
        }

        public void SetTrigger (PlayerAnimatorVariable animatorVariable) {
            if (!gameObject.activeSelf) {
                return;
            }

            Debug.Log($"[PlayerRenderer] SetTrigger () => {animatorVariable}", this);
            animator.SetTrigger(animatorVariableHashes[animatorVariable.ToString ()]);
        }

        public void SetBool (PlayerAnimatorVariable animatorVariable, bool value) {
            if (!gameObject.activeSelf) {
                return;
            }
            animator.SetBool (animatorVariableHashes[animatorVariable.ToString()], value);
        }

        public void SetLayerWeight (int layerIndex, float value) {
            if (!gameObject.activeSelf) {
                return;
            }
            animator.SetLayerWeight(layerIndex, value);
        }

        public void SetFloat (PlayerAnimatorVariable animatorVariable, float value) {
            if (!gameObject.activeSelf) {
                return;
            }
            animator.SetFloat (animatorVariableHashes[animatorVariable.ToString()], value);

            switch (animatorVariable) {
                case PlayerAnimatorVariable.MoveSpeed:
                    const string FAKE_CLOTH_POWER = "_FakeClothPower";
                    playerGraphic.mainRenderer.material.SetFloat(FAKE_CLOTH_POWER, value + 1);
                    break;
            }
        }

        public float GetFloat(PlayerAnimatorVariable animatorVariable) {
            if (!gameObject.activeSelf) {
                return 0;
            }
            return animator.GetFloat(animatorVariableHashes[animatorVariable.ToString()]);
        }

        private bool GetBool (PlayerAnimatorVariable animatorVariable) {
            if (!gameObject.activeSelf) {
                return false;
            }
            return animator.GetBool(animatorVariableHashes[animatorVariable.ToString()]);
        }

        public void SetLook (in float deltaTime, Vector3 worldPosition, float weight) {
            if (headLook != null) {
                headLook.SetTarget(deltaTime, worldPosition, weight);
            }
        }

        /// <summary>
        /// Tries to play a ball hit animation.
        /// Returns false if its not possible.
        /// </summary>
        /// <param name="targetVelocity"></param>
        /// <param name="defaultAnim">Requested Animation</param>
        /// <param name="result">Resulted animation</param>
        /// <param name="disableVolley"></param>
        /// <returns></returns>
        public bool PlayBallHitAnimation (
            in Vector3 targetVelocity,
            PlayerAnimatorVariable defaultAnim,
            out PlayerAnimatorVariable result, 
            in float ballHoldTime,
            bool disableVolley = false) {

            result = defaultAnim;

            var animSettings = EngineOptions.EngineOptions_BallHitAnimations.Current;

            if (IsCurrentClipBlocker(in animSettings.BallHitActionBlockers)) {
                return false;
            }

            float ballHeight = Ball.Current.transform.position.y;

            float time = Time.time;

            var specialHit = time < ballHoldTime + animSettings.SPECIAL_BALL_HIT_TIME_OFFSET;

            bool isVolley = specialHit && ballHeight > animSettings.BALL_HEIGHT_VOLLEY_START && ballHeight < animSettings.BALL_HEIGHT_VOLLEY_END;
            bool isGroundHeader = specialHit && ballHeight > animSettings.BALL_HEIGHT_GROUND_HEADER_UNTIL && ballHeight < animSettings.BALL_HEIGHT_VOLLEY_START;
            bool isJumpHeader = specialHit && ballHeight > animSettings.BALL_HEIGHT_VOLLEY_END;
            bool isDefault = ballHeight < animSettings.BALL_HEIGHT_GROUND_HEADER_UNTIL;

            if (isVolley && disableVolley) {
                isJumpHeader = true;
            }

            if (!disableVolley && isVolley && animSettings.AnimSettings.Find (PlayerAnimatorVariable.Volley_R)) {
                result = PlayerAnimatorVariable.Volley_R;

            } else if (isGroundHeader && animSettings.AnimSettings.Find(PlayerAnimatorVariable.GroundHeader_R)) {
                result = PlayerAnimatorVariable.GroundHeader_R;

            } else if (isJumpHeader && animSettings.AnimSettings.Find(PlayerAnimatorVariable.Header_R)) {
                result = PlayerAnimatorVariable.Header_R;
            } else {
                if (animSettings.AnimSettings.Find(defaultAnim)) {
                    result = defaultAnim;
                } else {
                    // play the first enabled, or one of them.
                    var enabledEntry = animSettings.AnimSettings.Entries.OrderByDescending(x => x.Val).FirstOrDefault();
                    result = enabledEntry.Id;
                }
            }

            if (GetBool (PlayerAnimatorVariable.ThrowInIdle)){
                result = PlayerAnimatorVariable.Throw_R;

                SetBool(PlayerAnimatorVariable.ThrowInIdle, false);
            }

            SetTrigger(FootedAnim(result, targetVelocity));

            return true;
        }

        private PlayerAnimatorVariable FootedAnim (PlayerAnimatorVariable defaultAnim, Vector3 targetVelocity) {
            string anim = defaultAnim.ToString ().Split ('_')[0];

            var angle = Vector3.SignedAngle(targetVelocity, transform.forward, Vector3.up);

            if (angle > 0) {
                anim += "_R";
            } else {
                anim += "_L";
            }

            return (PlayerAnimatorVariable)System.Enum.Parse(typeof(PlayerAnimatorVariable), anim);
        }
    }
}