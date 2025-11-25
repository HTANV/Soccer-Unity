using FStudio.Animation;
using FStudio.MatchEngine.Players;
using FStudio.Utilities;

using System;
using UnityEngine;
using URandom = UnityEngine.Random;
using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.EngineOptions;
using System.Linq;
using System.Threading.Tasks;
using FStudio.MatchEngine.Players.PlayerController;
using FStudio.MatchEngine.Graphics.EventRenderer;

namespace FStudio.MatchEngine.Balls {
    [ExecuteInEditMode]
    [RequireComponent (typeof (Rigidbody))]
    [RequireComponent (typeof (CapsuleCollider))]
    public partial class Ball : SceneObjectSingleton <Ball> {
        private const string BALL_POSITION_SHADER_VARIABLE = "_BallPosition";
        private const string NETS_FACTOR_SHADER_VARIABLE = "_NetsFactor";
        private const string BALL_SHADOW_POWER = "_Power";

        private const float NETS_FACTOR_SPEED = 5, NETS_FACTOR_DISABLED_SPEED = 0.5f;

        private const float IGNORE_COLLISION_TIME_FOR_BALL_HITTER = 1f;

        private const float BALL_COLLIDE_DELAY = 0.25f;

        private const float MAX_WORLD_POW = 200;

        private const float MAX_VELOCTIY_POW = 200;

        [SerializeField] private new Rigidbody rigidbody = default;

        public Vector3 Velocity => rigidbody.velocity;

        public CapsuleCollider Collider;

        public Transform ballAssetPoint;

        [SerializeField] private Transform ballShadow;
        [SerializeField] private Material shadowMaterial;

        private float nextCollision;

        private bool m_isOnCrossMode;

        private float crossHeight;

        public bool IsOnCrossMode {
            private set {
                m_isOnCrossMode = value;

                crossIndicator.SetActive(value);
            }
            get { return m_isOnCrossMode; }
        }

        public Vector3 CrossTarget { private set; get; }

        [SerializeField] private GameObject crossIndicator;

        private float crossCollisionCheck;

        public PlayerBase LastTouchedPlayer {
            private set;
            get;
        }

        public PlayerBase LastHolder {
            private set;
            get;
        }

        private Vector3 holdedPosition;
        private float followSpeedProgress;
        private float followSpeed;

        [SerializeField] private AnimationCurve followSpeedCurve;
        [SerializeField] private float holdedBallFollowSpeed = 1f;

        private AnimationQuery holdBlocker;

        [SerializeField] private bool isOnGoal;
        #region events
        public Action<PlayerBase> OnBallHold;
        public Action<GameTeam> OnBallHit;
        #endregion

        /// <summary>
        /// Current holder of the ball.
        /// </summary>
        public PlayerBase HolderPlayer { get; private set; }

        /// <summary>
        /// If the ball holded by a player, this is the team of the player.
        /// </summary>
        public GameTeam HolderTeam {
            get {
                if (HolderPlayer != null) {
                    return HolderPlayer.GameTeam;
                }
                
                return null;
            }
        }

        /// <summary>
        /// Returns the BallPosition, or ball drop point.
        /// </summary>
        public Vector3 BallPosition (PlayerBase player, float relaxation = 0) {
            return Predicter (player, relaxation);
        }

        private void OnValidate() {
            rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<CapsuleCollider>();
        }

        protected override void OnEnable () {
            if (Application.isPlaying) {
                EventManager.Subscribe<GoalEvent>(OnGoal);
            }
        }

        private void OnDisable() {
            if (Application.isPlaying) {
                EventManager.UnSubscribe<GoalEvent>(OnGoal);
            }
        }

        private async void OnGoal (GoalEvent goal) {
            isOnGoal = true;
            await Task.Delay((int) (250 / Time.timeScale));
            isOnGoal = false;
        }

        private void LateUpdate() {
            var factor = Shader.GetGlobalFloat(NETS_FACTOR_SHADER_VARIABLE);
            factor = Mathf.Lerp(factor, isOnGoal ? 1 : 0, Time.deltaTime * (isOnGoal ? NETS_FACTOR_SPEED : NETS_FACTOR_DISABLED_SPEED));
            Shader.SetGlobalFloat(NETS_FACTOR_SHADER_VARIABLE, factor);

            Shader.SetGlobalVector(BALL_POSITION_SHADER_VARIABLE, transform.position);
            
            // keep it in limits.
            var ballPos = transform.position;
            var ballVel = Velocity;

            ballPos.x = Mathf.Clamp(ballPos.x, -MAX_WORLD_POW, MAX_WORLD_POW);
            ballPos.y = Mathf.Clamp(ballPos.y, -MAX_WORLD_POW, MAX_WORLD_POW);
            ballPos.z = Mathf.Clamp(ballPos.z, -MAX_WORLD_POW, MAX_WORLD_POW);

            transform.position = ballPos;

            ballVel.x = Mathf.Clamp(ballVel.x, -MAX_VELOCTIY_POW, MAX_VELOCTIY_POW);
            ballVel.y = Mathf.Clamp(ballVel.y, -MAX_VELOCTIY_POW, MAX_VELOCTIY_POW);
            ballVel.z = Mathf.Clamp(ballVel.z, -MAX_VELOCTIY_POW, MAX_VELOCTIY_POW);

            rigidbody.velocity = ballVel;

            ballShadow.position = ballPos;
            // ball shadow power.
            float height = ballPos.y;
            ballPos.y = 0;
            float heightPow = Mathf.Max (0, 0.6f - height);
            shadowMaterial.SetFloat(BALL_SHADOW_POWER, heightPow);
        }

        /// <summary>
        /// Ball holded by player.
        /// </summary>
        /// <param name="basePlayer"></param>
        public void Hold(PlayerBase basePlayer) {
            if (HolderPlayer != null) {
                Release();
            }

            LastTouchedPlayer = basePlayer;

            MatchManager.Current.ResetOffsides();

            Debug.LogFormat("[Ball] Hold by {0}", basePlayer);

            HolderPlayer = basePlayer;

            holdedPosition = transform.position;

            rigidbody.isKinematic = true;
            Collider.enabled = false;

            followSpeedProgress = 0;

            basePlayer.OnBallHold();

            MatchManager.Current.ResetBehaviours();
        }

        /// <summary>
        /// Release the ball.
        /// </summary>
        public void Release() {
            rigidbody.isKinematic = false;
            Collider.enabled = true;

            if (HolderPlayer != null) {
                IgnoreCollisionTemporary(HolderPlayer);
            }

            LastHolder = HolderPlayer;

            if (HolderPlayer != null) {
                HolderPlayer.OnBallRelease();
            }

            HolderPlayer = null;

            if (holdBlocker != null) {
                holdBlocker.Stop();
                holdBlocker = null;
            }

            #region ;disable player collision for 0.5 seconds;
            holdBlocker = new AnimationQuery();
            holdBlocker.AddToQuery(new TimerAction(0.25f));
            holdBlocker.Start(this, () => { 
                holdBlocker = null;
            });
            #endregion
        }

        /// <summary>
        /// Behave ball for holder. Returns true if progress is completed.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public bool HolderBehave (Vector3 position, Quaternion rotation, in float deltaTime, float speedMod) {
            if (HolderPlayer != null) {
                if (MatchManager.Current.MatchFlags == Enums.MatchStatus.WaitingForKickOff || 
                    MatchManager.Current.MatchFlags == Enums.MatchStatus.NotPlaying) {
                    return false;
                }

                followSpeedProgress = Mathf.Min (1, 
                    followSpeedProgress + deltaTime * holdedBallFollowSpeed * speedMod);

                followSpeed = followSpeedCurve.Evaluate(followSpeedProgress) * 1;

                rigidbody.velocity = Vector3.zero;

                var targetPosition = Vector3.Lerp(
                    holdedPosition,
                     position,
                        followSpeed);

                if (MatchManager.Current.MatchFlags.HasFlag (Enums.MatchStatus.Playing)) {
                    // ball should be in the size of field when holded by someone.
                    var sizeOfField = MatchManager.Current.SizeOfField;
                    targetPosition.x = Mathf.Clamp(targetPosition.x, 0, sizeOfField.x);
                    targetPosition.z = Mathf.Clamp(targetPosition.z, 0, sizeOfField.y);
                    //
                }

                transform.position = targetPosition;
                transform.rotation = rotation;

                return followSpeed >= 1;
            }

            return false;
        }
        
        private void IgnoreCollisionTemporary (PlayerBase playerBase) {
            var playerCollider = playerBase.PlayerController.UnityCollider;
            Physics.IgnoreCollision(Collider, playerCollider, true);

            new TimerAction(IGNORE_COLLISION_TIME_FOR_BALL_HITTER).GetQuery().Start(this, ()=> {
                if (playerCollider != null) {
                    Physics.IgnoreCollision(Collider, playerCollider, false);
                }
            });
        }

        private void ApplyError (ref Vector3 target, float skill) {
            Vector3 position = transform.position;
            var dir = (target - position).normalized;
            float distance = Vector3.Distance(target, position);
            dir = ApplyDirectionError(dir, skill);
            target = position + dir * distance;
        }

        /// <summary>
        /// Send a curved pass to the target spot.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="hitter"></param>
        public void Cross (Vector3 target, PlayerBase playerBase, bool ignoreOffside = false) {
            var crossingSkill = playerBase.MatchPlayer.GetLongBall();

            ApplyError(ref target, crossingSkill);

            target = EngineOptions_CrossBallSettings.Current.CrossPoint(playerBase.Position, target);

            crossHeight = EngineSettings.Current.CrossHeight(Vector3.Distance(target, playerBase.Position)) + 
                transform.position.y - HeightRemovalByHeight ().magnitude;

            var requiredVelocity = calcBallisticVelocityVector(
            transform.position,
            target,
            crossHeight);

            crossHeight = requiredVelocity.y;

            BallHit (playerBase, requiredVelocity, ignoreOffside);

            // Reset behaviours for normal pass.
            MatchManager.Current.DelayBehaviourSelectionByReactionSkill ();

            if (!playerBase.IsThrowHolder) {
                EventManager.Trigger(new PlayerCrossEvent(playerBase, requiredVelocity.magnitude));
            }

            CrossTarget = EngineOptions_CrossBallSettings.Current.TargetPointForDrag (playerBase.Position, target);

            // reset y.
            CrossTarget = new Vector3(CrossTarget.x, 0, CrossTarget.z);
            target.y = 0;
            //

            // keep in field.
            target.x = Mathf.Clamp(target.x, 1, MatchManager.Current.SizeOfField.x - 1);
            target.z = Mathf.Clamp(target.z, 1, MatchManager.Current.SizeOfField.y - 1);
            //

            crossIndicator.transform.position = target + Vector3.up * 0.05f;

            IsOnCrossMode = true;

            crossCollisionCheck = Time.time + Time.fixedDeltaTime * 2;

            Debug.LogFormat("[Ball] Cross => {0} magniute", requiredVelocity.magnitude);
        }

        public void Pass (
            Vector3 target, 
            PlayerBase hitter, 
            float speedMod = 1, 
            bool ignoreOffside = false) {

            var distance = Vector3.Distance(transform.position, target);

            var longBallPercentage = 
                EngineSettings.Current.LongBallSkillPercentageAtDistance(distance);

            var longBallSkill = longBallPercentage * hitter.MatchPlayer.ActualPassing;
            var passingSkill = (1 - longBallPercentage) * hitter.MatchPlayer.ActualLongBall;

            ApplyError (ref target, longBallSkill + passingSkill);

            var pos = transform.position;
            pos.y = 0;

            var requiredVelocity = calcBallisticVelocityVector(
            pos,
            target,
            EngineSettings.Current.PassHeight (distance));

            Debug.LogFormat("[Ball] Pass => {0} required salt velocity.", requiredVelocity.magnitude);

            requiredVelocity *= EngineSettings.Current.PassPowerDistanceCurve.Evaluate (distance);
            requiredVelocity *= speedMod;

            if (!hitter.IsThrowHolder) {
                EventManager.Trigger(new PlayerPassEvent(hitter, requiredVelocity.magnitude));
            }
            //

            BallHit (hitter, requiredVelocity, ignoreOffside);

            // delay behaviours for normal pass.
            MatchManager.Current.DelayBehaviourSelectionByReactionSkill();

            Debug.LogFormat ("[Ball] Pass => {0} magniute", requiredVelocity.magnitude);
        }

        public void Shoot (Vector3 velocity, PlayerBase hitter) {
            Debug.Log("[Ball] Shoot");

            EventManager.Trigger(new PlayerShootEvent (hitter, velocity.magnitude));

            BallHit(hitter, velocity, false);
        }

        /// <summary>
        /// Get direction error. Target dir should not be normalized since we gonna use its magnitude on calculation.
        /// </summary>
        /// <returns>Returns a direction vector (not normalized). It has same velocity magnitude with the given vector</returns>
        public static Vector3 ApplyDirectionError (Vector3 targetDir, float skill, in float maxErrorAngle = -1) {
            var dirError = GetDirectionError(targetDir.magnitude, skill, in maxErrorAngle);
            return dirError * targetDir;
        }

        public static Quaternion GetDirectionError (float velocityMagnitude, float skill, in float maxAngleError = -1) {
            if (!EngineSettings.Current.IsDirectionErrorEnabled) {
                return Quaternion.identity;
            }

            var maxDirError = EngineSettings.Current.DirectionErrorModByVelocityCurve.Evaluate(velocityMagnitude) * 
                EngineSettings.Current.DirectionErrorSkillModCurve.Evaluate (skill/100f);

            var @error = URandom.Range(maxDirError / 4f, maxDirError);

            var @sideForward = URandom.Range(0, 10) > 4 ? -1 : 1;
            var @sideUp  = URandom.Range(0, 10) > 4 ? -1 : 1;

            if (maxAngleError > 0) {
                // clamp error.
                error = Mathf.Min(maxAngleError, error);
            }

            return Quaternion.Euler(error * sideUp, error * sideForward, 0);
        }

        private void AssignOffsides (PlayerBase hitter, bool ignoreOffside = false) {
            if (MatchManager.Current != null) {
                if (!ignoreOffside) {
                    MatchManager.Current.AssignOffsides(hitter.GameTeam, hitter);
                } else {
                    MatchManager.Current.ResetOffsides();
                }

                MatchManager.Current.MatchFlags = Enums.MatchStatus.Playing;
            }
        }

        private void ClampVelocity (ref Vector3 velocity) {
            // Clamp.
            if (velocity.magnitude < EngineSettings.Current.Ball_MinHitVelocity) {
                velocity = velocity.normalized * EngineSettings.Current.Ball_MinHitVelocity;
            }
            //
        }

        private Vector3 HeightRemovalByHeight () {
            return Vector3.up * EngineSettings.Current.ShootHeightByBallHeightCurve.Evaluate(transform.position.y);
        }

        private void BallHit (
            PlayerBase hitter,
            Vector3 velocity, 
            bool ignoreOffside = false) {

            if (float.IsNaN(velocity.magnitude)) {
                return;
            }

            var ballPos = transform.position;
            if (ballPos.y < 0.2f) {
                if (UnityEngine.Random.Range (0, 100) < velocity.magnitude) {
                    DirtRenderer.Current.SetPosition(0, transform.position);
                }
            }

            var heightVector = HeightRemovalByHeight();
            velocity -= heightVector;

            AssignOffsides(hitter, ignoreOffside);

            // release ball and hit.
            Release();

            rigidbody.velocity = Vector3.zero;

            ClampVelocity(ref velocity);

            Debug.LogFormat("[Ball] Velocity => {0}", velocity);

            rigidbody.angularVelocity = velocity;

            rigidbody.AddForce(velocity, ForceMode.VelocityChange);

            OnBallHit?.Invoke(hitter.GameTeam);
        }

        private bool CheckPlayerTouch (Collision collision) {
            var player = collision.collider.GetComponent<IPlayerController>();
            if (!player.IsPhysicsEnabled) {
                return false;
            }

            if (player.BasePlayer.CaughtInOffside) {
                return false;
            }
			
			var impulse = rigidbody.velocity;
			impulse.y = 0;

            // check hit angle.
            var contactPoint = collision.contacts.First().normal;
            var angle = Mathf.Abs (Vector3.SignedAngle(-contactPoint, transform.forward, Vector3.up));

            if (angle > 180) { // not possible to hold.
                return false;
            }

            if (player.BasePlayer.OnBallTouch (transform.position.y, impulse.magnitude, this)) {
                Hold(player.BasePlayer);

                if (player.BasePlayer.IsGK) {
                    EventManager.Trigger(new KeeperSavesTheBallEvent(player.BasePlayer, collision.impulse.magnitude));
                } else {
                    EventManager.Trigger(new PlayerControlBallEvent(player.BasePlayer, collision.impulse.magnitude));
                }

                return true;
            }
            
            if (player.BasePlayer.IsGK) {
                EventManager.Trigger(new KeeperHitTheBallButCouldNotControlEvent(player.BasePlayer, rigidbody.velocity.magnitude));
            }

            // throw ball here.
            var playerPos = player.Position;
            playerPos.y = transform.position.y;
            var dir = transform.position - playerPos;
            rigidbody.AddForce(dir * (rigidbody.velocity.magnitude + 1));

            return false;
        }

        public void ResetBall (Vector3 target) {
            Debug.Log("Ball Reset ()");
            target.y = 0.1f;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.position = target;
            rigidbody.MovePosition(target);
            LastHolder = null;
            LastTouchedPlayer = null;
            Release();
        }

        private void OnCollisionEnter (Collision collision) {
            if (crossCollisionCheck < Time.time) {
                IsOnCrossMode = false;
            }

            var colliderLayer = collision.collider.gameObject.layer;
            var colliderTag = collision.collider.gameObject.tag;

            if (colliderTag.Equals(Tags.GOALNET_TAG)) {
                EventManager.Trigger(new BallHitNetEvent(collision.impulse.magnitude));
            } else if (colliderTag.Equals(Tags.GOALOUT_TAG)) {
                EventManager.Trigger(new BallOutByNetEvent(collision.impulse.magnitude));
            } else if (colliderTag.Equals(Tags.WOODWORK_TAG)) {
                EventManager.Trigger(new BallHitTheWoodWorkEvent(collision.impulse.magnitude));
            }

            if (!MatchManager.Current.MatchFlags.HasFlag(Enums.MatchStatus.Playing)) {
                return; //rest is for player ball control
            }

            if (colliderLayer == LayerMask.NameToLayer (Tags.PLAYER_LAYER)) {
				
                LastTouchedPlayer = collision.collider.gameObject.GetComponent<IPlayerController>().BasePlayer;

                float time = Time.time;
                if (nextCollision > time) {
                    return;
                }

                nextCollision = time + BALL_COLLIDE_DELAY;

                // check for new ball taker.
                if (holdBlocker == null) {
                    if (CheckPlayerTouch(collision)) {
                        return;
                    }
                }
            }
        }

        private void OnCollisionStay(Collision collision) {
            OnCollisionEnter(collision);
        }
    }
}
