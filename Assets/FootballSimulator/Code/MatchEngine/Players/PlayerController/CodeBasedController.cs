using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Enums;
using UnityEngine;

using FStudio.MatchEngine.Players.Behaviours;
using FStudio.MatchEngine.Utilities;
using System;
using FStudio.MatchEngine.Graphics.EventRenderer;

namespace FStudio.MatchEngine.Players.PlayerController {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public partial class CodeBasedController : MonoBehaviour, IPlayerController
    {
        public GameObject UnityObject => gameObject;

        public CapsuleCollider UnityCollider => collider;

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        public Vector3 Forward => transform.forward;

        public Action<Collision> CollisionEnterEvent { get; set; }

        [SerializeField] private bool debugger;

        public bool IsDebuggerEnabled => debugger;

        public Vector3 Direction { get; private set; }

        private bool m_IsPhysicsEnabled;

        public bool IsPhysicsEnabled {
            get {
                return m_IsPhysicsEnabled;
            }

            set {
                m_IsPhysicsEnabled = value;

                rigidbody.isKinematic = !value;
                collider.enabled = value;
            }
        }

        public float MoveSpeed { get; set; }

        public float TargetMoveSpeed { get; set; }

        public PlayerAnimator Animator => playerAnimator;
        public PlayerUI UI => playerUI;

        public PlayerBase BasePlayer { get; set; }

        private Transform shadow;

        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private PlayerGraphic playerGraphic;

        [SerializeField] private PlayerUI playerUI;

        [HideInInspector] [SerializeField] private new Rigidbody rigidbody;

        [HideInInspector] [SerializeField] private new CapsuleCollider collider;

        public float Height => collider.height;

        private Vector3 targetAnimatorDirection;

        private Vector3 targetPosition;

        private void Start () {
            playerAnimator.SetFloat(PlayerAnimatorVariable.Agility, 0.5f + (BasePlayer.MatchPlayer.ActualAgility / 200f));

            UI.SetName(BasePlayer.MatchPlayer.Player.Name);
        }

        private void OnDestroy() {
            if (shadow != null) {
                shadow.gameObject.SetActive(false);
            }
        }

        public void SetAsLineReferee () {
            playerGraphic.SetRefereeFlag(true);
        }

        public bool IsAnimationABlocker (in string [] clips) {
            return playerAnimator.IsCurrentClipBlocker(clips);
        }

        public void SetOffside (bool isInOffide) {
            playerUI.SetBool(PlayerUI.UIAnimatorVariable.ShowOffside, isInOffide);
        }

        public void SetUI (bool value) {
            playerUI.SetBool(PlayerUI.UIAnimatorVariable.ShowName, value);
        }

        public void SetInstantPosition (Vector3 position) {
            position.y = 0; // fix y to 0 always.
            transform.position = position;
            rigidbody.position = position;
        }

        public void SetInstantRotation (Quaternion rotation) {
            rigidbody.rotation = rotation;
            transform.rotation = rotation;
        }

        private void OnValidate () {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<CapsuleCollider>();

            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            rigidbody.drag = 1;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        public void SetPosition(Vector3 position) {
            position.y = 0;
            rigidbody.position = position;
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation) {
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
            rigidbody.rotation = rotation;
            transform.rotation = rotation;
        }

        public void LerpRotation (in float deltaTime, Quaternion rotation, float agility) {
            rigidbody.rotation = Quaternion.Slerp(
                transform.rotation,
                rotation,
                deltaTime * agility);

            transform.rotation = rigidbody.rotation;
        }

        public void SetHeadLook (in float dT, Vector3 target, float weight) {
            playerAnimator.SetLook(in dT, target, weight);
        }

        private (float turnResult, float angleDifferency) AgileToDirection (Vector3 targetDirection) {
            if (Direction == targetDirection) {
                return (1, 0);
            }
			
            float angleDifferency = Mathf.Abs (Vector3.SignedAngle(Direction, targetDirection, Vector3.up));

            float agility = BasePlayer.MatchPlayer.GetAgility();

            float turnDifficulty = 
                EngineSettings.Current.AgileToDirectionMoveSpeedHardness.Evaluate (MoveSpeed) * 
                EngineSettings.Current.AgileToDirectionAngleDifferencyHardness.Evaluate (angleDifferency);

            float turnResult = agility / (turnDifficulty + 1);

            return (turnResult, angleDifferency);
        }

        /// <summary>
        /// Calculate go direction as Vector2 ( [-1, 1], [-1, 1] )
        /// </summary>
        private void CalculateAnimatorDirection () {
            var forward = transform.forward;
            forward.y = 0;

            var agl = Vector3.Angle(forward, Direction);
            var sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(forward, Direction)));
            var angle = agl * sign;

            targetAnimatorDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        /// <summary>
        /// Set the game team of the player.
        /// </summary>
        /// <param name="GameTeam"></param>
        public void SetPlayer (
            int number,
            PlayerBase basePlayer,
            Material kitMaterial) {

            Debug.Log("[PlayerBase] Set Player ()");

            this.BasePlayer = basePlayer;

            gameObject.name = basePlayer.MatchPlayer.Player.Name;

            gameObject.layer = LayerMask.NameToLayer(Tags.PLAYER_LAYER);
            
            // set rigidbody and collider by the player.
            rigidbody.mass = basePlayer.MatchPlayer.GetStrength();
            rigidbody.isKinematic = true;

            float weight = basePlayer.MatchPlayer.Player.weight / 180f;
            float height = basePlayer.MatchPlayer.Player.height / 125f + (basePlayer.MatchPlayer.ActualJump * 0.4f / 100f);

            float radiusMode = basePlayer.IsGK ? 0.5f : 1;

            float xSize = weight * radiusMode;

            collider.radius = xSize * 1.25f;
            collider.height = height;
            collider.center = Vector3.up * height / 2;

            if (MatchManager.Current != null) {
                collider.sharedMaterial = MatchManager.Current.PlayerColliderMaterial;
            }

            playerGraphic.SetPlayer(number, kitMaterial, basePlayer.MatchPlayer.Player);

            playerGraphic.SetGKGloves(basePlayer.IsGK);

            var scale = new Vector3(
                basePlayer.MatchPlayer.Player.weight, 
                basePlayer.MatchPlayer.Player.height,
                basePlayer.MatchPlayer.Player.weight);

            scale.x *= 0.01375f;
            scale.z *= 0.01375f;
            scale.y *= 0.0058f;

            // set visual scale.
            transform.localScale = scale;

			Debug.Log ($"Height: {basePlayer.MatchPlayer.Player.height} - Weight {basePlayer.MatchPlayer.Player.weight}");

            playerAnimator.SetFloat(PlayerAnimatorVariable.Agility, basePlayer.MatchPlayer.ActualAgility / 100f);

            // take a shadow.
            shadow = ShadowRenderer.Current.Get();
        }

        public bool MoveTo (
            in float deltaTime, 
            Vector3 targetPosition, 
            bool faceTowards = true,
            MovementType movementType = MovementType.BestHeCanDo) {

            this.targetPosition = targetPosition;

            var isReached = false;

            targetPosition.y = 0;

            float distance = Vector3.Distance(Position, targetPosition);
            var newDirection = (targetPosition - Position).normalized;
            newDirection.y = 0;

            if (distance > collider.radius) {
                if (newDirection.magnitude <= float.Epsilon) {
                    newDirection = transform.forward;
                }

                var newQuad = Quaternion.LookRotation(newDirection);

                var directionAgile = AgileToDirection(newDirection);

                if (Direction.magnitude > float.Epsilon) {
                    var quad = Quaternion.LookRotation(Direction);
                    var newDir = Quaternion.Slerp(quad, newQuad, deltaTime * directionAgile.turnResult);
                    Direction = newDir * Vector3.forward;
                } else {
                    Direction = newQuad * Vector3.forward;
                }

                var targetMovement = 1 - (directionAgile.angleDifferency / 180);

                switch (movementType) {
                    case MovementType.Relax:
                        targetMovement *= 0.25f;
                        break;

                    case MovementType.Normal:
                        targetMovement *= 0.75f;
                        break;
                }

                TargetMoveSpeed = Mathf.Min (1, targetMovement);
            } else {
                Stop(in deltaTime);
                isReached = true;
            }

            if (faceTowards) {
                LookTo(in deltaTime, targetPosition - Position);
            }

            return isReached;
        }

        /// <summary>
        /// LookTo direction, returns true if we are already looking there.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool LookTo (in float deltaTime, Vector3 lookDirection) {
            const float ANGLE_TO_APPROVE_LOOKAT = 60f;
            const float ANGLE_TO_APPROVE_ADD_BY_PER_BALL_HEIGHT_WHEN_HOLDING_BALL = 60f;

            if (lookDirection == Vector3.zero || transform.forward == lookDirection) {
                return true;
            }

            lookDirection.y = 0;

            float agileSpeed = AgileToDirection(lookDirection).turnResult;

            LerpRotation(
                in deltaTime, 
                Quaternion.LookRotation(lookDirection),
                agileSpeed * (BasePlayer.IsHoldingBall ? EngineSettings.Current.AgileToDirectionWhenHoldingBallModifier : 1));

            float angle = Vector3.SignedAngle(transform.forward, lookDirection, Vector3.up);

            float ballHeightMod = 0;

            if (BasePlayer.IsHoldingBall && !BasePlayer.IsThrowHolder) {
                ballHeightMod = Ball.Current.transform.position.y * ANGLE_TO_APPROVE_ADD_BY_PER_BALL_HEIGHT_WHEN_HOLDING_BALL;
            }

            if (Mathf.Abs (angle) <= (BasePlayer.IsThrowHolder ? 10 : ANGLE_TO_APPROVE_LOOKAT + ballHeightMod)) {
                return true;
            }

            return false;
        }

        public void Stop (in float deltaTime) {
            const float DIRECTION_RECOVERY_WHEN_STOP = 5f;

            const float STOPPING_SPEED = 5;

            TargetMoveSpeed = Mathf.Lerp (TargetMoveSpeed, 0, deltaTime * STOPPING_SPEED);
            Direction = Vector3.Lerp(Direction, transform.forward, deltaTime * DIRECTION_RECOVERY_WHEN_STOP);
        }

        public void InstantStop () {
            targetAnimatorDirection = Vector2.zero;
            playerAnimator.SetFloat(PlayerAnimatorVariable.Horizontal, 0);
            playerAnimator.SetFloat(PlayerAnimatorVariable.Vertical, 0);
            Direction = Vector3.zero;
            TargetMoveSpeed = 0;
            MoveSpeed = 0;
        }

        public void ProcessMovement (in float time, in float deltaTime) {
#region smooth move speed.
            #region constants
            const float ANIMATOR_PARAMETER_LERP_SPEED = 4f;
            const float MOVEMENT_DIRECTION_SPEED_LEANING_MODIFIER = 2f;
            const float MOVEMENT_DIRECTION_ANGLE_LEANING_MODIFIER = 0.05f;
            const float MIN_MOVE_SPEED_TO_MOVE = 0.5f;
            #endregion

            float animatorLerpSpeed = deltaTime * ANIMATOR_PARAMETER_LERP_SPEED;

            float dribbleModifier = BasePlayer.MatchPlayer.GetDribbleSpeedModifier ();

            float finalSpeed = TargetMoveSpeed *
                BasePlayer.MatchPlayer.GetTopSpeed() *
                (BasePlayer.IsHoldingBall ? dribbleModifier : 1);

            bool shouldMove = finalSpeed > MoveSpeed;

            float accMod = (finalSpeed < MoveSpeed) ? 3f : 
                BasePlayer.MatchPlayer.GetAcceleration();

            MoveSpeed = Mathf.Lerp(MoveSpeed, finalSpeed, deltaTime * accMod);

            float angle = Mathf.Abs(Vector3.SignedAngle(transform.forward, Direction, Vector3.up));

            var acceleration =
                Mathf.Abs(finalSpeed - MoveSpeed) * MOVEMENT_DIRECTION_SPEED_LEANING_MODIFIER +
                angle * MOVEMENT_DIRECTION_ANGLE_LEANING_MODIFIER;


            acceleration /= angle / 90 + 1;

            var shouldStop = !enabled ||
                    (!MatchManager.Current.MatchFlags.HasFlag(MatchStatus.Playing) &&
                    !MatchManager.Current.MatchFlags.HasFlag(MatchStatus.Special));

            playerAnimator.SetFloat(PlayerAnimatorVariable.MoveSpeed, Mathf.Max(MIN_MOVE_SPEED_TO_MOVE, MoveSpeed));
            #endregion

            #region smooth animator direction
            CalculateAnimatorDirection();

            if (shouldStop) {
                targetAnimatorDirection = Vector2.zero;
            }

            if (Mathf.Abs(targetAnimatorDirection.x) < 0.001f) {
                targetAnimatorDirection.x = 0;
            }

            if (Mathf.Abs(targetAnimatorDirection.y) < 0.001f) {
                targetAnimatorDirection.y = 0;
            }

            targetAnimatorDirection.x *= TargetMoveSpeed;
            targetAnimatorDirection.y *= TargetMoveSpeed;

            var currentAnimatorHorizontal = playerAnimator.GetFloat (PlayerAnimatorVariable.Horizontal);
            var currentAnimatorVertical = playerAnimator.GetFloat   (PlayerAnimatorVariable.Vertical);

            currentAnimatorHorizontal = Mathf.Lerp(currentAnimatorHorizontal, targetAnimatorDirection.y, animatorLerpSpeed);
            currentAnimatorVertical = Mathf.Lerp(currentAnimatorVertical, targetAnimatorDirection.x, animatorLerpSpeed);

            playerAnimator.SetFloat(PlayerAnimatorVariable.Horizontal, currentAnimatorHorizontal);
            playerAnimator.SetFloat(PlayerAnimatorVariable.Vertical, currentAnimatorVertical);


            playerAnimator.SetBool(PlayerAnimatorVariable.IsHoldingBall, BasePlayer.IsHoldingBall);
#endregion

            if (shouldStop) {
                return;
            }

#region move to direction.
            if (shouldMove || MoveSpeed > MIN_MOVE_SPEED_TO_MOVE) {
                SetPosition(Position + Direction.normalized * Mathf.Min (finalSpeed, MoveSpeed) * deltaTime);
            }
#endregion
        }

        private void OnCollisionEnter (Collision collision) {
            CollisionEnterEvent?.Invoke(collision);
        }

        /// <summary>
        /// Update the controller.
        /// </summary>
        /// <param name="dT"></param>
        /// <param name="matchStatus"></param>
        /// <param name="ball"></param>
        public void Up (in float dT, MatchStatus matchStatus, Ball ball) {
            #region ball follow to ball point
            if (BasePlayer.IsHoldingBall && (BasePlayer.IsThrowHolder || matchStatus == MatchStatus.Playing)) {
                var followSpeedMod = 1f;

                if (
                    BasePlayer.ActiveBehaviour is ChipShootingBehaviour ||
                    BasePlayer.ActiveBehaviour is ShootingBehaviour ||
                    BasePlayer.ActiveBehaviour is PassingBehaviour ||
                    BasePlayer.ActiveBehaviour is CrossingBehaviour) {

                    followSpeedMod = 1 - ball.transform.position.y;
                }

                followSpeedMod = Mathf.Clamp(followSpeedMod, 0.4f, 1f);

                var ballHolderSituation = PlayerBallPoint.Situation.Normal;
                if (BasePlayer.IsThrowHolder) {
                    ballHolderSituation = PlayerBallPoint.Situation.ThrowIn;
                } else if (BasePlayer.IsGKUntouchable && !BasePlayer.IsGoalKickHolder) {
                    ballHolderSituation = PlayerBallPoint.Situation.GK;
                }

                // behave with the ball.
                var holdingPosition = playerAnimator.BallPosition(ballHolderSituation);
                var holdingRotation = playerAnimator.BallRotation(ballHolderSituation);

                ball.HolderBehave(holdingPosition, holdingRotation, in dT, followSpeedMod);
            }
            //
            #endregion
        }

        private void LateUpdate () {
            shadow.position = Position;
        }

        public bool HitBall (
            in Vector3 targetVelocity, 
            PlayerAnimatorVariable animatorVariable,
            out PlayerAnimatorVariable result, 
            in float ballHoldTime,
            bool disableVolley = false){
            return playerAnimator.PlayBallHitAnimation(in targetVelocity, animatorVariable, out result, in ballHoldTime, disableVolley);
        }

        public void BallHitEvent()
        {
            BasePlayer.BallHitEvent();
        }
    }
}