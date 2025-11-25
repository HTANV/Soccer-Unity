using UnityEngine;
using AudioManager.Public;

using FStudio.Events;
using System.Threading.Tasks;
using FStudio.MatchEngine.Events;

namespace FStudio.Audio {
    public class MatchAudio : AudioMaster<MatchAudio> {
        private const float VELOCITY_TO_SOUND_VOLUME = 0.0275f;
        private const float BALL_CONTROL_SOUND_MOD = 0.4f;
        private const float BALL_HIT_GOAL_SOUND_MOD = 0.75f;
        private const float SLIDE_TACKLE_SOUND_VOLUME = 0.1f;
        private const float THROW_IN_SOUND_VOLUME = 0.2f;
        private const float CHIP_SHOOT_SOUND_VOLUME = 0.4f;
        private const float WHISTLE_MOD = 0.15f;

        protected override void OnEnable() {
            base.OnEnable();

            EventManager.Subscribe<BallHitNetEvent>(BallHitNet);
            EventManager.Subscribe<BallHitTheWoodWorkEvent>(BallHitTheWoodWork);
            EventManager.Subscribe<BallOutByNetEvent>(BallOutByNet);
            EventManager.Subscribe<KeeperSavesTheBallEvent>(KeeperSavesTheBall);
            EventManager.Subscribe<PlayerChipShootEvent>(PlayerChipShoot);
            EventManager.Subscribe<PlayerControlBallEvent>(PlayerControlBall);
            EventManager.Subscribe<PlayerCrossEvent>(PlayerCross);
            EventManager.Subscribe<PlayerDisbalancedEvent>(PlayerDisbalanced);
            EventManager.Subscribe<PlayerPassEvent>(PlayerPass);
            EventManager.Subscribe<PlayerShootEvent>(PlayerShoot);
            EventManager.Subscribe<PlayerSlideTackleEvent>(PlayerSlideTackle);
            EventManager.Subscribe<PlayerTackledEvent>(PlayerTackled);
            EventManager.Subscribe<PlayerThrowInEvent>(PlayerThrowIn);
            EventManager.Subscribe<RefereeLastWhistleEvent>(RefereeLastWhistle);
            EventManager.Subscribe<RefereeLongWhistleEvent>(RefereeLongWhistle);
            EventManager.Subscribe<RefereeShortWhistleEvent>(RefereeShortWhistle);
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<BallHitNetEvent>(BallHitNet);
            EventManager.UnSubscribe<BallHitTheWoodWorkEvent>(BallHitTheWoodWork);
            EventManager.UnSubscribe<BallOutByNetEvent>(BallOutByNet);
            EventManager.UnSubscribe<KeeperSavesTheBallEvent>(KeeperSavesTheBall);
            EventManager.UnSubscribe<PlayerChipShootEvent>(PlayerChipShoot);
            EventManager.UnSubscribe<PlayerControlBallEvent>(PlayerControlBall);
            EventManager.UnSubscribe<PlayerCrossEvent>(PlayerCross);
            EventManager.UnSubscribe<PlayerDisbalancedEvent>(PlayerDisbalanced);
            EventManager.UnSubscribe<PlayerPassEvent>(PlayerPass);
            EventManager.UnSubscribe<PlayerShootEvent>(PlayerShoot);
            EventManager.UnSubscribe<PlayerSlideTackleEvent>(PlayerSlideTackle);
            EventManager.UnSubscribe<PlayerTackledEvent>(PlayerTackled);
            EventManager.UnSubscribe<PlayerThrowInEvent>(PlayerThrowIn);
            EventManager.UnSubscribe<RefereeLastWhistleEvent>(RefereeLastWhistle);
            EventManager.UnSubscribe<RefereeLongWhistleEvent>(RefereeLongWhistle);
            EventManager.UnSubscribe<RefereeShortWhistleEvent>(RefereeShortWhistle);
        }

        private float Volume (float velocityMagnitude) => VELOCITY_TO_SOUND_VOLUME * velocityMagnitude;

        private void BallHitNet(BallHitNetEvent eventObject) {
            audioManager.Play("BallHitTheGoalNet", eventObject.Power * BALL_HIT_GOAL_SOUND_MOD);
        }

        private void BallHitTheWoodWork(BallHitTheWoodWorkEvent eventObject) {
            audioManager.Play("BallHitTheWoodWork", Volume(eventObject.Power * BALL_HIT_GOAL_SOUND_MOD));
        }

        private void BallOutByNet(BallOutByNetEvent eventObject) {
            Debug.Log(eventObject.Power);
            audioManager.Play("BallOutByTheNet", Volume(eventObject.Power * BALL_HIT_GOAL_SOUND_MOD));
        }

        private void KeeperSavesTheBall(KeeperSavesTheBallEvent eventObject) {
            audioManager.Play("GoalKeeperHoldTheBall", eventObject.Power * BALL_CONTROL_SOUND_MOD);
        }

        private void PlayerChipShoot(PlayerChipShootEvent eventObject) {
            audioManager.Play("PlayerChipShoot", CHIP_SHOOT_SOUND_VOLUME);
        }

        private void PlayerControlBall(PlayerControlBallEvent eventObject) {
            audioManager.Play("PlayerControlBall", eventObject.Power * BALL_CONTROL_SOUND_MOD);
        }

        private void PlayerCross(PlayerCrossEvent eventObject) {
            audioManager.Play("PlayerCross", Volume(eventObject.Power));
        }

        private void PlayerPass(PlayerPassEvent eventObject) {
            audioManager.Play("PlayerPass", Volume(eventObject.Power));
        }

        private void PlayerDisbalanced(PlayerDisbalancedEvent eventObject) {
            audioManager.Play("PlayerDisbalanced", SLIDE_TACKLE_SOUND_VOLUME);
        }

        private void PlayerShoot(PlayerShootEvent eventObject) {
            audioManager.Play("PlayerShoot", Volume(eventObject.Power));
        }

        private void PlayerSlideTackle(PlayerSlideTackleEvent eventObject) {
            audioManager.Play("PlayerSlideTackle", SLIDE_TACKLE_SOUND_VOLUME);
        }

        private void PlayerTackled(PlayerTackledEvent eventObject) {
            audioManager.Play("PlayerTackled", SLIDE_TACKLE_SOUND_VOLUME);
        }

        private void PlayerThrowIn(PlayerThrowInEvent eventObject) {
            audioManager.Play("PlayerThrowIn", THROW_IN_SOUND_VOLUME);
        }

        private void RefereeLastWhistle(RefereeLastWhistleEvent eventObject) {
            audioManager.Play("RefereeLastWhistle", WHISTLE_MOD);
        }

        private void RefereeLongWhistle(RefereeLongWhistleEvent eventObject) {
            audioManager.Play("RefereeLongWhistle", WHISTLE_MOD);
        }

        private void RefereeShortWhistle(RefereeShortWhistleEvent eventObject) {
            audioManager.Play("RefereeShortWhistle", WHISTLE_MOD);
        }
    }
}

