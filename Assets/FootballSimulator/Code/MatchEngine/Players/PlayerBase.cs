
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using System.Linq;

using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;

using FStudio.MatchEngine.Tactics;

using System.Collections.Generic;
using FStudio.MatchEngine.Players.Behaviours;
using System.Threading.Tasks;
using FStudio.MatchEngine.Utilities;
using FStudio.Events;
using FStudio.MatchEngine.Events;

using FStudio.MatchEngine.Players.PlayerController;
using FStudio.MatchEngine.Players.InputBehaviours;

namespace FStudio.MatchEngine.Players {

    public abstract partial class PlayerBase {
        #region Constants
        public const float BEHAVIOUR_CHANGE_OFFSET_AS_SECONDS = 0.2f;

        private const float STRUGGLE_TIME = 1.5f;

        private const float BALL_CONTROL_FAIL_DISABLING_TIME = 0.4f;

        public const int MAX_THROWIN_DISTANCE = 15;
        #endregion
        public Vector3 Position => PlayerController.Position;
        public Quaternion Rotation => PlayerController.Rotation;

        public readonly IPlayerController PlayerController;

        /// <summary>
        /// Throught pass points. Length is fixed.
        /// </summary>
        private readonly ThroughtPassPoint[] passPoints = new ThroughtPassPoint[15];

        /// <summary>
        /// Current markers of the player.
        /// Limit is '5' including GK.
        /// </summary>
        public readonly LimitedCollection<PlayerBase> Markers = new LimitedCollection<PlayerBase>(5);

        protected readonly IEnumerable<BaseBehaviour> baseBehaviours = new BaseBehaviour[] {
            new InputTackleBehaviour(),
            new InputCrossBehaviour(),
            new InputShortPassBehaviour(),
            new InputThroughtPassBehaviour(),
            new InputShootBehaviour(),
            new InputBlockRestBehaviour(),

            new ThrowInBehaviour(),
            new CornerBehaviour(),
            new KickOffBehaviour(),

            new OurGKDegageBehaviour (),
            new OpponentGKDegageBehaviour (),

            new IsInOffsideBehaviour (),

            // Try chip shot if GK is away.
            new ChipShootingBehaviour (),

            new ShootingBehaviour (0.75f, 0.5f),

            // Relax if there noone around.
            new RunForwardWithBallBehaviour(0,
                RunForwardWithBallBehaviour.ForwardCurve.Wingman,
                RunForwardWithBallBehaviour.BewareMod.SuperCareful,
                false,
                0,
                0.4f,
                 MovementType.Normal), // max 0.55f ball progress
 
            // Stop unnecessary headers.
            new RunForwardWithBallBehaviour(0f,
                RunForwardWithBallBehaviour.ForwardCurve.Wingman,
                RunForwardWithBallBehaviour.BewareMod.Risky,
                false,
                0.5f, // activate on ball height 0.5f
                0.65f), // activate before ball progress 0.65),

            new ShootingBehaviour (0, 1f),

            new CrossingBehaviour (0.925f, 2),

            new DribblingBehaviour (RunForwardWithBallBehaviour.ForwardCurve.MostlyStraight),

            // Run to the goal with a normal chasing check.
            new RunForwardWithBallBehaviour(0.5f,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal,
                RunForwardWithBallBehaviour.BewareMod.Careful, 
                false),

            new ShootingBehaviour (0, 1.25f),
				
			// Run to the goal with a risky chasing check.
            new RunForwardWithBallBehaviour(0.7f,
                RunForwardWithBallBehaviour.ForwardCurve.EarlyToGoal,
                RunForwardWithBallBehaviour.BewareMod.Normal, 
                false),

            new ShootingBehaviour (0, 1.5f),

            new CrossingBehaviour (0.8f),
            new PassingBehaviour (0.8f, true, 5),
            new PassingBehaviour (0.95f),

            new ShootingBehaviour (0, 2.5f),

            new CrossingBehaviour (0.925f, 10),
            new CrossingBehaviour (0.8f, 1),
            new CrossingBehaviour (0.7f, 0.25f),

            new CriticalSendBallToSafe (),
        };

        protected virtual IEnumerable<BaseBehaviour> PrivateBehaviours { get; }

        private IEnumerable<BaseBehaviour> m_behaviours = null;

        public virtual IEnumerable<BaseBehaviour> Behaviours {
            get {
                if (m_behaviours == null) {
                    m_behaviours = baseBehaviours.Concat(PrivateBehaviours).ToList().AsReadOnly();
                }

                return m_behaviours;
            }
        }

        public Acts CurrentAct;

        public Vector3 PositioningMistake { private set; get; }

        private float nextPositionMistakeCalculation;

        public bool IsRunningBehindTheDefenseLine;

        public float MoveSpeed => PlayerController.MoveSpeed;

        public float TargetMoveSpeed => PlayerController.TargetMoveSpeed;

        public bool isInputControlled { private set; get; }

        public float PlayerFieldProgress { private set; get; }

        public float toGoalXDirection { private set; get; }

        public Vector3 GoalDirection => new Vector3(toGoalXDirection, 0, 0);

        public Vector3 TargetGoalNet { private set; get; }

        public Vector3 Direction => PlayerController.Direction;

        public Vector3 Velocity => Direction * MoveSpeed;

        public BallHitAnimationEvent ballHitAnimationEvent { protected set; get; }

        public bool IsGK => MatchPlayer.Position.HasFlag(FStudio.Data.Positions.GK);

        [HideInInspector] public Vector3 runForwardBehaviourFinalPosition;

        /// <summary>
        /// When this is true, noone can take the ball from us.
        /// </summary>
        public bool IsGKUntouchable { protected set; get; }

        private Vector3 targetBallHitVector;
        private float targetBallHitSpeed;

        /// <summary>
        /// If this is not null, we will trigger him with ball chasing behaviour after a pass.
        /// This will be set by behaviours.
        /// </summary>
        public PlayerBase PassingTarget;

        public bool IsHoldingBall { private set; get; }

        private float ballHoldTime;

        public PlayerBase MarkingTarget;

        /// <summary>
        /// Value is the chaser if chasers can catch us.
        /// Null if no one can catch us yet.
        /// </summary>
        private PlayerBase chaserTarget;

        private bool m_isInOffside;
        public bool IsInOffside { private set {
                m_isInOffside = value;
            } get => m_isInOffside; }

        public bool IsThrowHolder;

        public bool IsGoalKickHolder;

        public bool IsCornerHolder;

        private bool _caughtInOffside;

        public bool CaughtInOffside {
            get {
                return _caughtInOffside;
            }

            set {
                _caughtInOffside = value;
                PlayerController.SetOffside (value);
            }
        }

        public BaseBehaviour ActiveBehaviour;

        public float NextBehaviour;

        public readonly GameTeam GameTeam;

        public MatchPlayer MatchPlayer;

        public PlayerBase (GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) {
            this.MatchPlayer = matchPlayer;
            this.GameTeam = gameTeam;

            // Instantiate a renderer.
            PlayerController = Object.Instantiate(
                PlayerControllerPrefab.Current.PlayerController.UnityObject,
                Vector3.zero, 
                Quaternion.identity).GetComponent<IPlayerController>();

            PlayerController.SetPlayer(matchPlayer.Number, this, kitMaterial);

            /// register for collisions.
            PlayerController.CollisionEnterEvent = OnCollisionEnter;

            /// disable physics for start.
            PlayerController.IsPhysicsEnabled = false;
        }

        public void Dispose () {
            Object.Destroy(PlayerController.UnityObject);
        }

        private void OnCollisionEnter (Collision collision) {
            if (!PlayerController.IsPhysicsEnabled) {
                return;
            }

            if (!IsHoldingBall) {
                return;
            }
            
            if (collision.gameObject.layer == LayerMask.NameToLayer (Tags.PLAYER_LAYER)) {
                // hit to another player.

                if (!MatchManager.Current.MatchFlags.HasFlag (MatchStatus.Playing)) {
                    return;
                }

                var otherPlayer = collision.gameObject.GetComponent<IPlayerController>().BasePlayer;

                if (!PlayerController.IsPhysicsEnabled) {
                    return;
                }

                if (otherPlayer.IsGK || IsGK) {
                    return;
                }

                if (otherPlayer.GameTeam == GameTeam) {
                    return; // teammate.
                }

                var myStrength = MatchPlayer.ActualStrength;
                var ballKeeping = MatchPlayer.ActualBallKeeping;

                var otherStrength = otherPlayer.MatchPlayer.ActualStrength;

                // make a randomization.
                var bkpMod = ballKeeping * 0.1f;
                var strDiff = Mathf.Clamp (otherStrength - (myStrength + bkpMod), 0, 50);

                if (Random.Range (strDiff, 50) > 30) {
                    if (IsHoldingBall) {
                        EventManager.Trigger(new PlayerWinTheBallEvent(otherPlayer));
                        EventManager.Trigger(new PlayerLossTheBallEvent(this));
                    }

                    Struggle();
                } else {
                    if (otherPlayer.IsHoldingBall) {
                        EventManager.Trigger(new PlayerWinTheBallEvent(this));
                        EventManager.Trigger(new PlayerLossTheBallEvent(otherPlayer));
                    }

                    otherPlayer.Struggle();
                }
            }
        }

        public void ActivateBehaviour (string behaviourName) {
            if (Behaviours == null) {
                return; // this player is not using behaviours.
            }

            Debug.Log($"Activate behaviour {behaviourName}");

            var targetBehaviour = Behaviours.FirstOrDefault(x => x.GetType ().Name == behaviourName);

            if (targetBehaviour == null) {
                Debug.Log($"Activate behaviour {behaviourName} but not found in behaviour list.");
            } else {
                ActiveBehaviour = targetBehaviour;
                NextBehaviour = -1; // disable timer deactivation.
            }
        }

        public void ResetBehaviours () {
            foreach (var behaviour in Behaviours) {
                behaviour.ForceBehaviour = false;
            }

            ActiveBehaviour = null;
            NextBehaviour = 0;
        }

        private readonly Dictionary<AILevel, float> difficultySkipRate = new Dictionary<AILevel, float>() {
            { AILevel.Amateur, 80 },
            { AILevel.SemiPro, 45 },
            { AILevel.Professional, 25 },
            { AILevel.WorldClass, 5 },
            { AILevel.Legendary, 0 },
        };

        public void ProcessBehaviours (in float time) {
            // check animation block
            if (PlayerController.IsAnimationABlocker (in EngineOptions.EngineOptions_BallHitAnimations.Current.BlockBehavioursWhenThisClipsAreaPlaying)) {
                return;
            }

            var skipRate = difficultySkipRate[GameTeam.Team.AILevel];

            if (Random.Range (0, 100) < skipRate) {
                return;
            }

            var forcedOnes = Behaviours.Where(x => x.ForceBehaviour);

            if (forcedOnes.Any()) {
                foreach (var forced in forcedOnes) {
                    if (PlayerController.IsDebuggerEnabled) {
                        Debug.Log($"[ProcessBehaviours] Forced behaviour => {forced.GetType()}");
                    }

                    forced.Behave(ActiveBehaviour == forced);
                }

                return;
            }

            bool reset = NextBehaviour >= 0 && NextBehaviour < time;

            if (!reset) {
                // no reset, do the active.
                if (ActiveBehaviour != null) {
                    if (PlayerController.IsDebuggerEnabled) {
                        Debug.Log($"[ProcessBehaviours] Active behaviour => {ActiveBehaviour.GetType()}");
                    }

                    if (!ActiveBehaviour.Behave(true)) {
                        ActiveBehaviour = null;
                    }

                    return;
                }
            } else {
                ActiveBehaviour = null;
            }

            foreach (var behaviour in Behaviours) {
                bool isActiveBehaviour = behaviour == ActiveBehaviour;
                if (behaviour.Behave(isActiveBehaviour)) {
                    ActiveBehaviour = behaviour;
                    NextBehaviour = time + BEHAVIOUR_CHANGE_OFFSET_AS_SECONDS;

                    if (PlayerController.IsDebuggerEnabled) {
                        Debug.Log($"[ProcessBehaviours] Active behaviour changed to => {ActiveBehaviour.GetType()}");
                    }

                    return;
                } else if (isActiveBehaviour) {
                    Debug.Log($"[ProcessBehaviours] Active behaviour reset (was => {ActiveBehaviour.GetType()})");
                    ActiveBehaviour = null;
                }
            }
        }

        /// <summary>
        /// Check our markers if they can catch us or not.
        /// Returns a marker if he can catch.
        /// </summary>
        /// <returns></returns>
        public bool CanMyMarkersChaseMe (float carefulness) {
            var chasers = Markers.Members.Where (x=>x.PlayerController.IsPhysicsEnabled).Select(marker => 
            (marker, CanChaseMe(marker, in carefulness)));

            chaserTarget = chasers.OrderBy(x => x.Item2.distance).FirstOrDefault().marker;

            return chasers.Where(x => x.Item2.result).Any();
        }

        public Vector3 PredictPositionWithVelocityMod (in float velocityMod) {
            return Position + Velocity * velocityMod;
        }

        public bool BoundCheck(in float fieldBoundCheck, in Vector3 position, in Vector2 fieldSize) {
            var up = position.z + fieldBoundCheck;
            var down = position.z - fieldBoundCheck;
            var left = position.x - fieldBoundCheck;
            var right = position.x + fieldBoundCheck;

            if (up > fieldSize.y ||
                down < 0 ||
                left < 0 ||
                right > fieldSize.x) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Find a pass box around of the target player.
        /// </summary>
        /// <param name="targetPlayer"></param>
        /// <returns></returns>
        public IEnumerable<(
            string optionName,
            PlayerBase actualTarget, 
            Vector3 position,
            int priority,
            bool enableUserInput,
            List<PassType> passTypes,
            Color debugColor)> 
            FindPassPositions (PlayerBase targetPlayer) {

            float predicter = EngineSettings.Current.CanSeePredictPositionVelocityMod;
            var predictPosition = PredictPositionWithVelocityMod(predicter);
            var predictTargetPosition = targetPlayer.PredictPositionWithVelocityMod(predicter);

            float dist = Vector3.Distance(predictPosition, predictTargetPosition);

            var toPasser = (Position - predictTargetPosition).normalized;

            var passAngle =
                Mathf.Abs(Vector3.SignedAngle(toPasser, targetPlayer.Direction, Vector3.up));

            var passAngleCurved = EngineSettings.Current.
                PassingAngleCurve.Evaluate(
                passAngle / 180);

            var passAngleMultiplied = passAngleCurved;

            var distanceModifier = EngineSettings.Current.PassingDistancePlayerVelocityModifier * dist;

            var forwardDir =
                targetPlayer.Velocity *
                passAngleMultiplied *
                EngineSettings.Current.PassingPlayerVelocityModifier +
                targetPlayer.Velocity *
                passAngleMultiplied *
                distanceModifier;

            var passOptions = EngineSettings.Current.ThroughtPassOptions.Where (x=>x.IsEnabled).ToArray();
           
            if (passOptions.Length >= passPoints.Length) {
                Debug.LogError($"Pass points has {passOptions.Length} on engine settings, but constantly has {passPoints.Length} length on PlayerBase script.");
                return default;
            }

            int passOptionsLength = passOptions.Length;

            for (int i=0; i<passOptionsLength; i++) {
                passPoints[i] = new ThroughtPassPoint(
                    passOptions[i],
                    predictTargetPosition,
                    forwardDir,
                    TargetGoalNet);
            }

            var fieldSize = MatchManager.Current.SizeOfField;
            var fieldBoundCheck = EngineSettings.Current.PassingFieldBoundCheck;

            (Vector3 dir, bool isPassed) throughtPointCheck (ThroughtPassPoint point) {
                if (point.OnlyWhenRunningForward) {
                    if (
                        (targetPlayer.toGoalXDirection > 0 && targetPlayer.Direction.x < 0) ||
                        (targetPlayer.toGoalXDirection < 0 && targetPlayer.Direction.x > 0)) {

                        return default;
                    }
                }

                var playerToPoint = point.Position - predictPosition;

                var canCross = !point.DisableCrossing;

                if (!BoundCheck (in fieldBoundCheck, in point.Position, fieldSize)) {
                    return default;
                }

                return (point.Position, true);
            }

            var targets = passPoints.Take (passOptionsLength).
                Select(x => (x, (throughtPointCheck(x), x.Priority))).
                Where (x=>x.Item2.Item1.isPassed).
                Select (x=>(x.x.OptionName, targetPlayer, x.Item2.Item1.dir, x.Item2.Priority, x.x.EnableUserInput, x.x.PassTypes, x.x.DebugColor));

            return targets;

        }

        /// <summary>
        /// Closest to the attack power. For ordering.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public float XPower (in float sizeOfFieldX, in Vector3 position) {
            float result;
            if (toGoalXDirection > 0) {
                result = position.x;
            } else {
                result = sizeOfFieldX - position.x;
            }

            return result;
        }

        public enum PassType {
            ShortPass,
            LongPass,
            ThroughtPass
        }

        public struct PassTarget {
            public readonly PassType _PassType;
            public readonly string _OptionName;
            public readonly PlayerBase _ActualTarget;
            public readonly Vector3 _Position;
            public readonly float _PassPower;

            public PassTarget (
                PassType passType, 
                string optionName,
                Vector3 position,
                PlayerBase actualTarget,
                float passPower) {

                _PassType = passType;
                _OptionName = optionName;
                _ActualTarget = actualTarget;
                _Position = position;
                _PassPower = passPower;
            }

            public bool IsValid => !string.IsNullOrEmpty(_OptionName);
        }

        /// Returns the pass targets, ordered by priority.
        public ((PassType passType, string optionName, Vector3 position, int priority, PlayerBase actualTarget), float calculatedPriority)[]
        AvailablePassTargets (IEnumerable<PlayerBase> possibleTeammates, Vector3 targetGoalNetPosition) {
            var minPassDistance = EngineSettings.Current.MinimumPassDistance;

            var passOptions = possibleTeammates.Where(x => x.PlayerController.IsPhysicsEnabled && !x.IsInOffside).
                // no diving GK.
                Where (x=>!x.IsGK || !x.Behaviours.Where (x=>x is GKShieldBehaviour).Cast<GKShieldBehaviour>().FirstOrDefault().IsOnJump).
                Select(x => FindPassPositions(x)).
                SelectMany(x => x).
                Where(x => Vector3.Distance(x.position, Position) > minPassDistance).
                ToArray();

            if (passOptions.Length == 0) {
                return default;
            }

            var blockers = GameTeam.TeamId == 0 ? 
                MatchManager.Current.GameTeam2.GamePlayers.AsEnumerable (): 
                MatchManager.Current.GameTeam1.GamePlayers.AsEnumerable ();

            // check can see.
            var passOptionsInVision = passOptions.Select (x => (
                x.optionName, // 0
                x.position, // 1
                x.priority, // 2
                CanSeeTarget ( // 3
                    x.passTypes,
                    x.actualTarget,
                    x.position,
                    blockers), 
                x.actualTarget // 4
                
                )).ToArray();

            //
            var shortPasses = passOptionsInVision.Where(x => x.Item4.shortPassAvailable).
                Select(x => (PassType.ShortPass, x.optionName, x.position, x.priority, x.actualTarget));

            var longPasses = passOptionsInVision.Where(x => x.Item4.longPassAvailable).
                Select(x => (PassType.LongPass, x.optionName, x.position, x.priority, x.actualTarget));

            var finalOptions = shortPasses.Concat(longPasses);

            // only short passes for GKs.
            finalOptions = finalOptions.Where(x => 
                !x.actualTarget.IsGK || 
                x.Item1 == PassType.ShortPass);
            //

            // remove if i am closer to pass point than actual target.
            var myPosition = Position;
            finalOptions = finalOptions.Where(
                x => 
                    Vector3.Distance(x.position, myPosition) > 
                    Vector3.Distance(x.position, x.actualTarget.Position));
            //

            if (!finalOptions.Any ()) {
                return default;
            }

            var fieldSize = MatchManager.Current.SizeOfField;

            var engineSettigs_middlePriority = EngineSettings.Current.PassingMiddlePriority;
            var engineSettigs_distancePriority = EngineSettings.Current.PassingOptionDistanceToPriority;
            var engineSettings_crossPriority = EngineSettings.Current.PassingCrossPriority;

            var myPos = Position;

            float distancePriority (Vector3 pos) {
                var distance = Vector3.Distance(myPos, pos);
                return distance * engineSettigs_distancePriority;
            }

            float middlePriority(Vector3 pos) {
                var distanceToMiddle = Mathf.Abs((fieldSize.y / 2) - pos.z);
                return distanceToMiddle * GameTeam.BallProgress * engineSettigs_middlePriority;
            }

            float priority (PassType passType, Vector3 position, int priority) {
                return (passType == PassType.LongPass ? engineSettings_crossPriority : 0) + // cross priority
                Mathf.Pow (XPower(in fieldSize.x, position), EngineSettings.Current.XPowerPow) + 
                middlePriority (position) + 
                distancePriority (position) +
                priority * EngineSettings.Current.PassingOptionPriorityPower;
            }

            var orderedOptions = finalOptions.
                Select (x=>(x, priority (x.Item1, x.position, x.priority))).OrderByDescending (x=>x.Item2).ToArray ();

            return orderedOptions;
        }

        private PassTarget BestPassTarget(
            IEnumerable<PlayerBase> possibleTeammates, Vector3 targetGoalNetPosition
            ) {

            var availables = AvailablePassTargets (possibleTeammates, targetGoalNetPosition);

            if (availables == null || !availables.Any ()){
                return default;
            }

            var first = availables.Select (x=>x.Item1).First ();

            var actualTargetPosition = first.actualTarget.Position;
            var ourPosition = Position;
            var usToActualTarget = actualTargetPosition - ourPosition;
            var usToPoint = first.position - ourPosition;
            var angle = Mathf.Abs(Vector3.SignedAngle(usToActualTarget, usToPoint, Vector3.up));

            var m_angle = EngineSettings.Current.PassPowerByPassAngleCurve.Evaluate(angle);
            var m_distance = EngineSettings.Current.PassPowerByAngledPassDistanceCurve.Evaluate(usToPoint.magnitude);
            var passPowerMod = m_angle * m_distance;

            return new PassTarget(first.Item1, 
                first.optionName, 
                first.position, 
                first.actualTarget, 
                passPowerMod);
        }

        private (bool shortPassAvailable, bool longPassAvailable) CanSeeTarget (
            List<PassType> passTypes,
            PlayerBase actualTarget,
            Vector3 target, 
            IEnumerable<PlayerBase> colliders) {

            float predictPositionDistance = EngineSettings.Current.CanSeePredictPositionVelocityMod;

            var passerPosition = PredictPositionWithVelocityMod (predictPositionDistance);

            var actualTargetPosition = actualTarget.PredictPositionWithVelocityMod (predictPositionDistance);

            var passerToPoint = target - passerPosition;

            var actualTargetToPoint = target - actualTargetPosition;

            var actualTargetToPointDistance = actualTargetToPoint.magnitude;

            var actualTargetToPasser = passerPosition - actualTargetPosition;

            var passerToActualTarget = actualTargetPosition - passerPosition;

            var actualTargetToPasserDistance = actualTargetToPasser.magnitude;

            var passerToPointDistance = passerToPoint.magnitude;

            var ballProgressMod = EngineSettings.Current.
                CanSeeSecureAngleModifierByBallProgressCurve.
                Evaluate (actualTarget.PlayerFieldProgress);

            float secureAngleBetweenPasserAndThread = 
                EngineSettings.Current.CanSeeSecureAngleBetweenPasserAndThread * ballProgressMod;

            var angleModByDistanceCurve = EngineSettings.
                Current.CanSeeAngleModByDistanceCurve;

            float threadDistanceAdd = EngineSettings.Current.CanSeeThreadDistanceAdditionByBallProgress.Evaluate (actualTarget.PlayerFieldProgress);

            float crossingSecureDistanceFromPasser =
                EngineSettings.
                Current.
                PassingCrossBlockApproveDistanceToPasserByDistance.Evaluate (passerToPointDistance);

            float crossingSecureDistanceToTarget =
                EngineSettings.
                Current.
                PassingCrossBlockApproveDistanceToTargetByDistance.Evaluate (passerToPointDistance);

            float crossingBehindLimit = EngineSettings.Current.CrossingBehindDistanceByFieldProgress.Evaluate (actualTarget.PlayerFieldProgress);

            (bool shortPassBlocked, bool longPassBlocked) isInCone (PlayerBase @thread) {
                var threadPosition = thread.PredictPositionWithVelocityMod (predictPositionDistance);

                var passerToThread = threadPosition - passerPosition;
                var actualTargetToThread = threadPosition - actualTargetPosition;

                var passerToPoint_passerToThreadAngle = Mathf.Abs(Vector3.SignedAngle(passerToPoint, passerToThread, Vector3.up));

                var passerToPoint_passerToActualTargetAngle = Mathf.Abs(Vector3.SignedAngle(passerToActualTarget, passerToPoint, Vector3.up));

                var threadDistanceToPasser = Vector3.Distance(threadPosition, passerPosition) + threadDistanceAdd;
                var threadDistanceToPoint = Vector3.Distance(threadPosition, target) + threadDistanceAdd;

                var passerAngleMod = angleModByDistanceCurve.Evaluate(threadDistanceToPasser);

                bool threadForPasser = 
                    passerToPoint_passerToActualTargetAngle > passerToPoint_passerToThreadAngle || (
                    threadDistanceToPasser < passerToPointDistance && 
                    passerToPoint_passerToThreadAngle < secureAngleBetweenPasserAndThread * passerAngleMod);

                bool threadForActualTarget = threadDistanceToPoint < actualTargetToPointDistance;

                bool shortPassBlocked = threadForPasser || threadForActualTarget;
                bool longPassBlocked = true;

                #region
                var backwards = 0f;

                if (toGoalXDirection > 0) {
                    // home team.
                    backwards = -passerToPoint.z;
                } else {
                    // away team.
                    backwards = passerToPoint.z;
                }

                var outOfAngle = passerToPoint_passerToThreadAngle > secureAngleBetweenPasserAndThread * 2;

                if (
                    backwards < crossingBehindLimit && 
                    actualTargetToPointDistance < threadDistanceToPoint && 
                    (outOfAngle || threadDistanceToPasser > crossingSecureDistanceFromPasser) &&
                    (outOfAngle || threadDistanceToPoint > crossingSecureDistanceToTarget)
                    ) {
                    longPassBlocked = false;
                }
                #endregion

                return (
                    !passTypes.Contains (PassType.ShortPass) || shortPassBlocked, 
                    !passTypes.Contains(PassType.LongPass) || longPassBlocked);
            }

            var results = colliders.Select (x => isInCone(x)).ToArray ();

            return (!results.Any (x=>x.shortPassBlocked), !results.Any(x => x.longPassBlocked));
        }

        private void OnDrawGizmosSelected() {
            // draw all pass positions

            if (Ball.Current.HolderPlayer != null) {
                var points = Ball.Current.HolderPlayer.FindPassPositions(this);

                foreach (var point in points) {
                    Gizmos.color = point.debugColor;
                    Gizmos.DrawSphere(point.position, 1);
                }
            }
        }

        /// <summary>
        /// When we want to find a teammate, we will be sure he is front of us, or not that far back in X axis.
        /// </summary>
        /// <param name="teammate"></param>
        /// <returns></returns>
        private bool DoesItMakeSenseToFindThisTeammate(PlayerBase teammate) {
            return
                IsFrontOfMe(teammate.Position) || // is at front of me
                Mathf.Abs(teammate.Position.x - Position.x) < EngineSettings.Current.PassingBackwardMaxDistance; // or backward, but not too far.
        }

        public PassTarget FindPassTarget (
            in PlayerBase[] teammates, 
            in Vector3 targetGoalNetPosition,
            bool ignoreChecks = false) {
            var targets = teammates.Where(x =>
            x != this &&
            (ignoreChecks || DoesItMakeSenseToFindThisTeammate(x)));

            return BestPassTarget(targets, targetGoalNetPosition);
        }

        /// <summary>
        /// Is the given player front of us.
        /// </summary>
        /// <param name="goalNet"></param>
        /// <param name="targetPlayer"></param>
        /// <returns></returns>
        public bool IsFrontOfMe (Vector3 target, float threshold = 0) {
            if (toGoalXDirection > 0) {
                return target.x >= Position.x + threshold;
            } else {
                return target.x <= Position.x - threshold;
            }
        }

        /// <summary>
        /// Can the target player chase me in min distance.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="carefulness">It should be between 0 and 1</param>
        /// <returns></returns>
        public (bool result, float distance) CanChaseMe (PlayerBase player, in float carefulness) {
            var dir = Direction;

            var meToPlayer = player.Position - Position;

            var dot = Vector3.Dot(dir, meToPlayer.normalized);

            var dot2 = Vector3.Dot((TargetGoalNet - Position).normalized, meToPlayer.normalized);

            float reachTime (PlayerBase player) {
                float speedMod = 0.04f;
                float distMod = 0.3f;

                var accLate = player.TargetMoveSpeed * speedMod;
                var distLate = distMod * meToPlayer.magnitude * (2.5f - dot - (dot2*0.25f));

                return distLate / (accLate +1);
            }

            // 

            var time = reachTime(player);
			
            return (time < carefulness, time);
        }

        /// <summary>
        /// When a player wants to reach another one, it should predict his next position.
        /// </summary>
        /// <param name="sourcePlayer"></param>
        /// <param name="targetPlayer"></param>
        /// <returns></returns>
        public static Vector3 Predicter (PlayerBase sourcePlayer, PlayerBase targetPlayer) {
            var targetPoint = Predicter(sourcePlayer, targetPlayer.Position, targetPlayer.Velocity);

            return targetPoint;
        }

        /// <summary>
        /// Predict position of a moving target from a player.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="targetVelocity"></param>
        /// <returns></returns>
        public static Vector3 Predicter(PlayerBase source, Vector3 target, Vector3 targetVelocity) {
            var angle = AngleBetweenPlayer(source, target, targetVelocity);
            var angleMod = AngleDifferenceMod(angle, source);
            return angleMod * targetVelocity.normalized + target;
        }

        private static float AngleBetweenPlayer (PlayerBase sourcePlayer, Vector3 targetPosition, Vector3 targetVelocity) {
            var dir = sourcePlayer.Position - targetPosition;
            var targetDir = targetVelocity;

            var angleWithDistance = AngleDistanceMod(dir.magnitude);

            return Mathf.Abs (Vector3.SignedAngle(dir, targetDir, Vector3.up)) * targetVelocity.magnitude * angleWithDistance;
        }

        private static float AngleDistanceMod (float distance) {
            return Mathf.Pow(distance, EngineSettings.Current.Angle_Distance_Power);
        }

        /// <summary>
        /// When a player runs forward, what should I do with my angle to that player to catch him?
        /// Usually we power the angle, and mod with something.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static float AngleDifferenceMod (float angle, PlayerBase source) {
            var engineSettings = EngineSettings.Current;
            return Mathf.Pow (angle, engineSettings.Angle_Power) * engineSettings.Angle_Multi * engineSettings.Angle_PlayerProgress.Evaluate (source.PlayerFieldProgress);
        }

        public virtual void FocusToBall (in float deltaTime, Ball ball) {
            if (!IsHoldingBall && !IsRunningBehindTheDefenseLine) {
                PlayerController.LookTo(in deltaTime, ball.transform.position - Position);
            }
        }

        /// <summary>
        /// Returns isoffside & position for onside.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="targetGoalNet"></param>
        /// <param name="offsideLine"></param>
        /// <returns></returns>
        public static (bool, float) IsPositionOffside (Vector3 position, GoalNet targetGoalNet, in float offsideLine) {
            var posX = position.x;
            var tGoalNetX = targetGoalNet.Position.x;
            var distanceToGoal = Mathf.Abs(tGoalNetX - posX);
            var offsideLineToGoal = Mathf.Abs(tGoalNetX - offsideLine);

            //

            return (distanceToGoal < offsideLineToGoal, offsideLine + targetGoalNet.Direction.x);
        }

        /// <summary>
        /// Behave players.
        /// </summary>
        public virtual void Behave(
                in bool isInputControlled,
                in float time,
                in float deltaTime,
                in int fieldEndX,
                in int fieldEndY,
                in MatchStatus matchStatus,
                in TeamBehaviour tactics,
                in float offsideLine,
                Ball ball,
                GoalNet goalNet, 
                GoalNet targetGoalNet, 
                in PlayerBase [] teammates, 
                in PlayerBase[] opponents
            ) {

            // check if we are at offside.
            if (targetGoalNet != null) {
                IsInOffside = IsPositionOffside(Position, targetGoalNet, in offsideLine).Item1;
            }
            //

            #region not holding ball specials
            if (!IsHoldingBall) {
                RandomizePositioningMistake(in deltaTime);
            }
            #endregion

            TargetGoalNet = targetGoalNet.Position;

            toGoalXDirection = goalNet.Direction.x;

            if (toGoalXDirection > 0) {
                PlayerFieldProgress = Position.x / fieldEndX;
            } else {
                PlayerFieldProgress = (fieldEndX - Position.x) / fieldEndX;
            }

            CurrentAct = Acts.Nothing;

            this.isInputControlled = isInputControlled;

            PlayerController.Up(in deltaTime, matchStatus, ball);

            var headLookPosition = !IsHoldingBall ? ball.transform.position : Position + PlayerController.Forward * 10 + Vector3.up * 2f;
            // head look
            if (tactics != TeamBehaviour.Attacking) {
                if (MarkingTarget != null) {
                    headLookPosition = MarkingTarget.Position;
                }
            } else {
                if (chaserTarget != null) {
                    headLookPosition = chaserTarget.Position;
                }
            }

            PlayerController.SetHeadLook(in deltaTime, headLookPosition, 1);

            #region design issue
            /// We are resetting chaser target at the end of every frame.
            /// Use CanAnyMarkersChaseMe () to auto assign this on player behaviours.
            chaserTarget = null;
            #endregion
            //
        }

        private void RandomizePositioningMistake (in float time) {
            if (nextPositionMistakeCalculation > time) {
                return;
            }

            nextPositionMistakeCalculation = time + EngineSettings.Current.PositioningMistakeUpdateInPerSeconds;

#region positioning skill randomizing
            var mistake = 101 - Random.Range(MatchPlayer.ActualPositioning, 100);
            var mistakeModifier = EngineSettings.Current.PositioningMistakeModifier;
            var mistakeVector = new Vector3(Random.Range(-mistake, mistake), 0, Random.Range(-mistake, mistake));
            PositioningMistake = mistakeVector * mistakeModifier;
#endregion
        }

        /// <summary>
        /// Find field position of player by the team tactics.
        /// </summary>
        /// <param name="teamBehaviour"></param>
        /// <param name="fieldEndX"></param>
        /// <param name="fieldEndY"></param>
        /// <param name="goalNet"></param>
        /// <returns></returns>
        public virtual Vector3 GetFieldPosition(
            in bool teammateHasTheBall,
            in TeamBehaviour teamBehaviour,
            in int fieldEndX,
            in int fieldEndY,
            in Vector3 ballPosition,
            in PlayerBase markingTarget,
            in float offsideLine,
            GoalNet goalNet,
            GoalNet targetGoalNet) {

            var fieldPosition = GameTeam.Team.TeamTactics.GetFieldPosition
                (
                MatchPlayer.Position,
                in GameTeam.Team.TacticPresetType,
                in teamBehaviour,
                GameTeam.BallProgress,
                in fieldEndX,
                in fieldEndY,
                in ballPosition,
                in markingTarget,
                goalNet,
                false);

            // fix by offside.
            var (isOffside, onSide) = IsPositionOffside(fieldPosition + GoalDirection, targetGoalNet, in offsideLine);
            if (isOffside) {
                // fix by onside.
                fieldPosition.x = onSide;
            }

            fieldPosition += PositioningMistake;

            // clamp
            fieldPosition.z = Mathf.Clamp(fieldPosition.z, 0, fieldEndY);
            fieldPosition.x = Mathf.Clamp(fieldPosition.x, 0, fieldEndX);
            //
            
            return fieldPosition;
        }

        /// <summary>
        /// Check ball control.
        /// </summary>
        /// <param name="ball"></param>
        public virtual bool OnBallTouch(
            float touchHeight, 
            float impulse, 
            Ball ball) {

            if (ball.HolderPlayer != this) {
                Debug.Log("Ball control: impulse:" + impulse + ", height:" + touchHeight);

                impulse = Mathf.Min (impulse, EngineSettings.Current.BallControlMaxBallImpulse);

                var heightMod = EngineSettings.Current.BallControlHeightMultiplierCurve.Evaluate(touchHeight);
                var impulseMod = EngineSettings.Current.BallControlDifficultyCurveByImpactPulseCurve.Evaluate(impulse);

                var max = impulseMod * heightMod;

                if (MatchPlayer.ActualBallControl > Random.Range (max/2f, max)) {
                    return true;
                }
            }

            DisablePlayerForAWhile (BALL_CONTROL_FAIL_DISABLING_TIME);
            
            return false;
        }

        public float SpeedModForPassing () {
            var skill = MatchPlayer.ActualAcceleration * 0.2f + MatchPlayer.ActualTopSpeed * 0.8f;
            return EngineSettings.Current.PassPowerReceiverSpeedCurve.Evaluate (skill);
        }

        private async void DisablePlayerForAWhile (float seconds) {
            var mili = Mathf.RoundToInt(seconds * 1000 / Time.timeScale);

            Debug.Log("disable for :" + mili);

            PlayerController.IsPhysicsEnabled = false;

            await Task.Delay(mili);

            try {
                PlayerController.IsPhysicsEnabled = true;
            } catch {}
        }

#region those will be called by anim events
        public virtual void BallHitEvent () {
            if (MatchManager.Current.MatchFlags == MatchStatus.NotPlaying) {
                return; // ignore.
            }

            if (Ball.Current.HolderPlayer != this) {
                ballHitAnimationEvent = BallHitAnimationEvent.None;
                return; // we are not the holder anymore.
            }

            if (ballHitAnimationEvent == BallHitAnimationEvent.None) {
                return;
            }

            if (IsThrowHolder) {
                switch (ballHitAnimationEvent) {
                    case BallHitAnimationEvent.Pass:
                        targetBallHitVector = Position + Vector3.ClampMagnitude(targetBallHitVector - Position, MAX_THROWIN_DISTANCE);
                        break;
                }
            }

            var headerMulti = EngineOptions.EngineOptions_BallHitAnimations.Current.Header_VelMulti;
            if (latestHitAnim == PlayerAnimatorVariable.Header_R || 
                latestHitAnim == PlayerAnimatorVariable.GroundHeader_R) {
                targetBallHitVector *= headerMulti;
            }

            switch (ballHitAnimationEvent) {
                case BallHitAnimationEvent.Shoot:
                    Ball.Current.Shoot(targetBallHitVector, this);
                    break;

                case BallHitAnimationEvent.Pass:
                    bool passAndRun = false;

                    if (!IsThrowHolder) {
                        // Pass and run.
                        passAndRun = GameTeam.Team.TeamTactics.RollPassAndRunChance(GameTeam.BallProgress);
                    }

                    Ball.Current.Pass(targetBallHitVector, this, targetBallHitSpeed, IsThrowHolder);

                    if (passAndRun) {
                        ActivateBehaviour("PassAndRunBehaviour");
                    }

                    break;

                case BallHitAnimationEvent.LongBall:
                    Ball.Current.Cross (targetBallHitVector, this, IsThrowHolder);
                    break;
            }

            // trigger passing target if its not null.
            if (PassingTarget != null) {
                PassingTarget.ActivateBehaviour("BallChasingWithoutCondition");
                PassingTarget = null;
            }

            IsThrowHolder = false;
            IsCornerHolder = false;
            IsGoalKickHolder = false;
            IsGKUntouchable = false;

            ballHitAnimationEvent = BallHitAnimationEvent.None;
        }
        #endregion

        private PlayerAnimatorVariable latestHitAnim;

        private bool PlayBallHitAnimation (in Vector3 velocity, PlayerAnimatorVariable request, bool disableVolley = false) {
            if (!PlayerController.HitBall (in velocity, request, out latestHitAnim, in ballHoldTime, disableVolley)) {
                return false;
            }

            var animSettings = EngineOptions.EngineOptions_BallHitAnimations.Current;

            Debug.Log("Ball hit animation: " + latestHitAnim);

            switch (latestHitAnim) {
                case PlayerAnimatorVariable.GroundHeader_R:

                    DisablePlayerForAWhile(animSettings.BlockForGroundHeader / (MatchPlayer.ActualAgility / 100f));

                    break;
                case PlayerAnimatorVariable.Header_R:

                    DisablePlayerForAWhile(animSettings.BlockForHeader / (MatchPlayer.ActualAgility / 100f));

                    break;
                case PlayerAnimatorVariable.Volley_R:

                    DisablePlayerForAWhile(animSettings.BlockForVolley / (MatchPlayer.ActualAgility / 100f));

                    break;
            }

            return true;
        }

        public void Shoot (Vector3 targetVelocity) {
            if (!IsHoldingBall) {
                return;
            }

            if (!PlayBallHitAnimation (in targetVelocity, PlayerAnimatorVariable.Shoot_R)) {
                return;
            }

            ballHitAnimationEvent = BallHitAnimationEvent.Shoot;

            targetBallHitVector = targetVelocity;

            Debug.Log("[PlayerBase] Shoot!");
        }

        public void Pass (Vector3 targetPoint, float speedMod = 1) {
            if (!IsHoldingBall) {
                return;
            }

            if (!PlayBallHitAnimation(targetPoint - Position, PlayerAnimatorVariable.Pass_R, true)) {
                return;
            }

            Debug.Log($"[PlayerBase] Pass to {targetPoint}");
            targetBallHitVector = targetPoint;
            targetBallHitSpeed = speedMod;

            ballHitAnimationEvent = BallHitAnimationEvent.Pass;
        }

        public void Cross (Vector3 targetPoint) {
            if (!IsHoldingBall) {
                return;
            }

            if (IsGK && IsGKUntouchable) {
                if (!PlayBallHitAnimation(targetPoint - Position, PlayerAnimatorVariable.GKDegage_R, true)) {
                    return;
                }
            } else {
                if (!PlayBallHitAnimation(targetPoint - Position, PlayerAnimatorVariable.LongBall_R, true)) {
                    return;
                }
            }

            Debug.Log("[PlayerBase] LongBall (Cross)!");
            targetBallHitVector = targetPoint;

            ballHitAnimationEvent = BallHitAnimationEvent.LongBall;
        }

        public bool PassToTarget (
            in float deltaTime, 
            Vector3 targetPosition) {

            if (PlayerController.LookTo(in deltaTime, targetPosition - Position)) {
                Pass(targetPosition);
                return true;
            }

            return false;
        }

        /// <summary>
        /// The best player can reach to target player at the shortest time.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="players"></param>
        public static IEnumerable<PlayerBase> BestOptionsToTargetPlayer (
            PlayerBase target, 
            IEnumerable<PlayerBase> players,
            in float ballProgress,
            in int howManyPlayersToPick, 
            bool considerOffside = true) {

            var maxDistance = 
                EngineSettings.
                Current.
                BestOptionToTargetMaxDistanceByBallProgressCurve.
                Evaluate(ballProgress);

            (bool isEligible, float reachTime) ReachTime (PlayerBase player) {
                var predicted = Predicter (player, target);

                float distance = Vector3.Distance(predicted, player.Position);

                if (player.IsGK) {
                    distance += EngineSettings.Current.BestOptionToTargetGKAddition;
                }

                float playerSpeed = player.MatchPlayer.GetAcceleration() * player.MatchPlayer.GetTopSpeed ();

                return (distance < maxDistance, distance / playerSpeed);
            }

            return players.Where (x=>
            x.PlayerController.IsPhysicsEnabled && 
            (!considerOffside || !x.CaughtInOffside)).
            Select (x => (x, ReachTime(x))). // convert to (player, isEligible, reachtime)
            Where (x=>x.Item2.isEligible). // eleminate not eligibles
            OrderBy (x=>x.Item2.reachTime). // order by reach time
            Take(howManyPlayersToPick). // take required amount
            Select (X=>X.x); // convert to player
        }

        /// <summary>
        /// The best player can reach to target position at the shortest time.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="players"></param>
        public static IEnumerable<PlayerBase> BestOptionsToTargetPosition (
            Vector3 position,
            IEnumerable<PlayerBase> players,
            int howManyPlayersToPick,
            bool considerOffside = true) {

            float ReachTime(PlayerBase player) {
                float distance = Vector3.Distance(position, player.Position);

                float playerSpeed = player.MatchPlayer.GetAcceleration() * player.MatchPlayer.GetTopSpeed();

                return distance / playerSpeed;
            }

            return players.Where(x =>
            x.PlayerController.IsPhysicsEnabled &&
            (!considerOffside || !x.CaughtInOffside)).
            OrderBy(x => ReachTime(x)).
            Take(howManyPlayersToPick);
        }

        public virtual void OnBallHold () {
            IsHoldingBall = true;

            ballHoldTime = Time.time;

            #region BEHAVIOURS HARD RESET
            NextBehaviour = 0;

            foreach (var behaviour in Behaviours) {
                behaviour.ForceBehaviour = false;
            }

            ActiveBehaviour = null;
            #endregion
        }

        public virtual void OnBallRelease() { IsHoldingBall = false; }
        public void Struggle () {
            ballHitAnimationEvent = BallHitAnimationEvent.None;

            if (IsHoldingBall) {
                Ball.Current.Release();
            }

            EventManager.Trigger(new PlayerDisbalancedEvent(this));

            PlayerController.Animator.SetTrigger(PlayerAnimatorVariable.Struggle);

            DisablePlayerForAWhile(STRUGGLE_TIME);
        }

        public bool MoveTo (
            in float dT, 
            Vector3 to, 
            bool faceTowards = true, 
            MovementType movementType = MovementType.BestHeCanDo) {
            return PlayerController.MoveTo(in dT, to, faceTowards, movementType);
        }

        public bool LookTo (in float dT, Vector3 to) {
            return PlayerController.LookTo(in dT, to);
        }

        public void Stop (in float dT) {
            PlayerController.Stop(in dT);
        }

        public void ProcessMovement (in float time, in float dT) {
            PlayerController.ProcessMovement (in time, in dT);
        }

        public void InstantStop () {
            float dT = Mathf.Infinity;
            PlayerController.Stop(in dT);

            PlayerController.Animator.SetFloat(PlayerAnimatorVariable.Horizontal, 0);
            PlayerController.Animator.SetFloat(PlayerAnimatorVariable.Vertical, 0);
        }
    }
}
