using UnityEngine;
using UnityEngine.InputSystem;

using FStudio.MatchEngine.Players;
using FStudio.Input;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Enums;

using System.Linq;

using FStudio.MatchEngine.Players.Behaviours;
using FStudio.MatchEngine.Players.InputBehaviours;
using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Tactics;
using FStudio.MatchEngine.Cameras;
using FStudio.MatchEngine.Players.PlayerController;
using static UnityEngine.Rendering.DebugUI;

namespace FStudio.MatchEngine.Input {
    public class TeamInputListener : InputListener {
        private static Vector3 inputWorldOffset = Vector3.up * 2;

        public enum ActionType {
            None,
            Pass,
            Shoot
        }

        private const float MOVE_DEADZONE = 0.3f;

        private PlayerBase m_ActivePlayer;
        public PlayerBase ActivePlayer { get => m_ActivePlayer; private set {
                Debug.Log("Assigned active player: " + value);
                m_ActivePlayer = value;
            } }

        private readonly GameTeam gameTeam;


        private Vector3 m_lastValidDirection;

        /// <summary>
        /// Active direction
        /// </summary>
        private Vector3 direction;

        private readonly Transform inputPointer;
        private readonly Transform inputPointerFollower;

        public TeamInputListener(
            int playerIndex, 
            GameTeam gameTeam) : base("MatchEngine", playerIndex) {

            this.gameTeam = gameTeam;

            inputPointer = Object.Instantiate(Resources.Load<Transform>("UI/InputPointer"));
            inputPointerFollower = inputPointer.GetChild(0);

            // create direction listener.
            RegisterAction("Move", MoveInput);
            RegisterAction("ChangePlayer", ChangePlayerInput);
            RegisterAction("Pass", PassInput);
            RegisterAction("ThroughtPass", ThroughtPass);
            RegisterAction("Tackle", TackleInput);
            RegisterAction("Shoot", ShootInput);
            RegisterAction("Cross", CrossInput);
            RegisterAction("ChangeTacticHigh", ChangeTacticHighInput);
            RegisterAction("ChangeTacticLow", ChangeTacticLowInput);
            //

            Debug.Log("Team Input Listener Created.");
        }

        ~TeamInputListener () {
            Clear();
        }

        private bool ChangeTacticHighInput(InputAction.CallbackContext ctx) {
            UpdateTactic(true);
            return true;
        }

        private bool ChangeTacticLowInput(InputAction.CallbackContext ctx) {
            UpdateTactic(false);
            return true;
        }

        private void UpdateTactic(bool increase) {
            var currentTactic = (int)MatchManager.Current.UserTeam.Team.TacticPresetType;
            currentTactic += increase ? 1 : -1;
            if (currentTactic < 0) {
                currentTactic = (int)TacticPresetTypes.ParameterCount - 1;
            } else if (currentTactic >= (int)TacticPresetTypes.ParameterCount) {
                currentTactic = 0;
            }

            var newTactic = (TacticPresetTypes)currentTactic;

            MatchManager.Current.UserTeam.Team.TacticPresetType = newTactic;
            EventManager.Trigger(new TeamChangedTactic(MatchManager.Current.UserTeam, newTactic));
        }

        private bool MoveInput (InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            if (ActivePlayer == null || !ActivePlayer.PlayerController.IsPhysicsEnabled) {
                direction = Vector3.zero;
                return true;
            }

            var value = ctx.ReadValue<Vector2>();

            var camMod = CameraSystem.Current.transform.rotation;
            var camMul = Quaternion.Euler(0, camMod.eulerAngles.y, 0);

            direction = camMul * new Vector3(value.x, 0, value.y);

            return true;
        }

        private void ActivatePlayerBehaviour<T>() where T : BaseBehaviour, IInputBehaviour {
            if (ActivePlayer.ActiveBehaviour is IInputBehaviour) {
                return;
            }

            var tBehaviour = ActivePlayer.Behaviours.
                Where(x => x is T).
                Select(x => (T)x).
                First();

            tBehaviour.IsTriggered = true;

            var typeName = typeof(T).Name.Split('.').Last();

            Debug.Log("[TeamInputListener]" + typeName);

            ActivePlayer.ActivateBehaviour(typeName);
        }

        private void DectivatePlayerBehaviour<T>() where T : BaseBehaviour, IInputBehaviour {
            var tBehaviour = ActivePlayer.Behaviours.
                Where(x => x is T).
                Select(x => (T)x).
                First();

            tBehaviour.IsTriggered = false;

            ActivePlayer.ResetBehaviours();
        }

        private bool PassInput(InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            var value = ctx.ReadValue<float>();

            if (ActivePlayer == null || 
                !ActivePlayer.PlayerController.IsPhysicsEnabled) {
                return true;
            }

            if (value == 1) {
                ActivatePlayerBehaviour<InputShortPassBehaviour>();
            }

            return true;
        }

        private bool ShootInput (InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            var value = ctx.ReadValue<float>();

            if (ActivePlayer == null || 
                !ActivePlayer.PlayerController.IsPhysicsEnabled) {
                return true;
            }

            if (value == 1) {
                ActivatePlayerBehaviour<InputShootBehaviour>();
            }

            return true;
        }

        private bool ThroughtPass (InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            if (ActivePlayer == null ||
                !ActivePlayer.PlayerController.IsPhysicsEnabled) {
                return true;
            }

            if (!ActivePlayer.IsHoldingBall) {
                return false;
            }

            ActivatePlayerBehaviour<InputThroughtPassBehaviour>();
            return true;
        }

        private bool CrossInput (InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            var value = ctx.ReadValue<float>();

            if (ActivePlayer == null ||
                !ActivePlayer.PlayerController.IsPhysicsEnabled) {
                return true;
            }

            if (!ActivePlayer.IsHoldingBall) {
                return false;
            }

            if (value == 1) {
                ActivatePlayerBehaviour<InputCrossBehaviour>();
                return true;
            }

            return false;
        }

        private bool TackleInput(InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            if (ActivePlayer == null || !ActivePlayer.PlayerController.IsPhysicsEnabled) {
                return true;
            }

            if (ActivePlayer.IsHoldingBall) {
                return false;
            }

            if (ctx.control.IsPressed()) {
                Debug.Log("[TackleInput] Tackle on");
                ActivatePlayerBehaviour<InputTackleBehaviour>();
            } else {
                Debug.Log("[TackleInput] Tackle off");
                DectivatePlayerBehaviour<InputTackleBehaviour>();
            }

            return true;
        }

        private bool ChangePlayerInput(InputAction.CallbackContext ctx) {
            if (MatchPause.IsPaused) {
                return false;
            }

            var value = ctx.ReadValue<float>();
            if (value == 1) {
                Debug.Log("[TeamInputListener] Change Player");
                AssignPlayer();
            }

            return true;
        }

        private void AssignPlayer () {
            var currentBallHolder = Ball.Current.HolderPlayer;

            if (currentBallHolder != null && ActivePlayer == currentBallHolder) {
                return;// cannot select another player.
            }

            var possibleTargets = gameTeam.GamePlayers.Where(x => !x.IsGK && x != ActivePlayer);

            if (currentBallHolder != null && currentBallHolder.GameTeam == gameTeam && !currentBallHolder.IsGK) {
                ActivePlayer = currentBallHolder;
                return;
            }

            if (currentBallHolder != null && currentBallHolder.GameTeam != gameTeam) {
                // opponent has the ball.
                // select tacklers.

                #region select from tacklers
                var tacklers = possibleTargets.Where(x => x.ActiveBehaviour is TryToTackleBehaviour).Select (x=>x.ActiveBehaviour as TryToTackleBehaviour).ToArray ();

                if (tacklers.Length > 0) {
                    //select one of the tacklers.
                    ActivePlayer = tacklers.OrderBy(x => x.MovementType).FirstOrDefault ().Player;
                    return;
                }
                #endregion
            }
            else {
                // ball is free, select one of chasers.

                #region select from receivers
                var receivers = possibleTargets.Where(x => x.ActiveBehaviour is BallChasingWithoutCondition).Select(x => x.ActiveBehaviour as BallChasingWithoutCondition).ToArray();
                if (receivers.Length > 0) {
                    ActivePlayer = receivers.OrderBy (x=>x.ChasingDistance).FirstOrDefault ().Player;
                    return;
                }
                #endregion

                #region select from chasers
                var chasers = possibleTargets.Where(x => x.ActiveBehaviour is BallChasingBehaviour).ToArray();

                if (chasers.Length > 0) {
                    //select one of the tacklers.
                    ActivePlayer = chasers.OrderBy(x => BallChasingBehaviour.BallChasingDistance(x)).FirstOrDefault();
                    return;
                }
                #endregion

                // select the best option.
                var bestOption = possibleTargets.
                    OrderBy(x => BallChasingBehaviour.BallChasingDistance(x)).FirstOrDefault();

                ActivePlayer = bestOption;
            }
        }

        public void Update (in float deltaTime) {
            if (ActivePlayer == null) {
                Debug.Log("[TeamInputListener] Input listener doesnt have a player. Looking for a player to control.");

                AssignPlayer();
            }

            var holderPlayer = Ball.Current.HolderPlayer;

            if (ActivePlayer != holderPlayer && holderPlayer != null && !holderPlayer.IsGK && holderPlayer.GameTeam == gameTeam) {
                ActivePlayer = holderPlayer;
            }

            if (ActivePlayer == null) {
                return;
            }

            if (direction.magnitude > 0.2f) {
                m_lastValidDirection = direction;
            }

            if (m_lastValidDirection.magnitude < 0.2f) {
                m_lastValidDirection = ActivePlayer.Rotation * Vector3.forward;
            }

            var behaviours = ActivePlayer.Behaviours.
                Where(x => x is IInputBehaviour).
                Select(x => (IInputBehaviour)x);

            foreach (var ib in behaviours) {
                ib.InputDirection = ib.InputDirection = m_lastValidDirection;
            }

            if (ActivePlayer.PlayerController.IsPhysicsEnabled) {
                if (!MatchManager.Current.MatchFlags.HasFlag( MatchStatus.Playing )) {
                    if (ActivePlayer.IsHoldingBall) {
                        ActivePlayer.LookTo(in deltaTime, direction);
                    }
                }
            }


            if (ActivePlayer.ActiveBehaviour is not IInputBehaviour) {
                var length = direction.magnitude;

                var activePosition = ActivePlayer.Position;
                activePosition += direction.normalized * 2;

                if (length < MOVE_DEADZONE) {
                    ActivePlayer.Stop(in deltaTime);
                } else {
                    MovementType movementType = MovementType.BestHeCanDo;

                    if (length < 0.5f) {
                        movementType = MovementType.Relax;
                    } else if (length < 0.75f) {
                        movementType = MovementType.Normal;
                    }

                    ActivePlayer.MoveTo(in deltaTime, activePosition, true, movementType);
                }
            }

            // reposition UI Input pointer.
            var arrowIndicatorTransform =
                GetArrowIndicatorPositionAndAngle(Camera.main.WorldToScreenPoint(ActivePlayer.Position + inputWorldOffset));

            inputPointerFollower.position = arrowIndicatorTransform.position;
            inputPointerFollower.rotation = arrowIndicatorTransform.rotation;
        }

        private (Vector2 position, Quaternion rotation) GetArrowIndicatorPositionAndAngle(
                    Vector3 originalPosition) {

            bool isInBounds(Vector3 pos, Vector2 screen) {
                return !(pos.x > screen.x - 50 || pos.x < 50 || pos.y > screen.y - 50 || pos.y < 50);
            }

            var screen = new Vector2(Screen.width, Screen.height);

            if (!isInBounds(originalPosition, screen)) {
                var ballOnScreen = Camera.main.WorldToScreenPoint(Ball.Current.transform.position);
                var dir = (originalPosition - ballOnScreen).normalized;

                originalPosition = ballOnScreen;

                while (isInBounds(originalPosition, screen)) {
                    originalPosition += dir;
                }

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

                return (originalPosition, q);
            }

            return (originalPosition, Quaternion.Euler(0, 0, -90));
        }
    }
}
