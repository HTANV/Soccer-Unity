
using UnityEngine;
using System;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;

namespace FStudio.MatchEngine.Players.PlayerController {
    public interface IPlayerController {
        GameObject UnityObject { get; }

        CapsuleCollider UnityCollider { get; }

        PlayerBase BasePlayer { get; }

        Vector3 Position { get; }

        Action<Collision> CollisionEnterEvent { set; get; }

        Vector3 Forward { get; }

        bool IsPhysicsEnabled { set; get; }

        Quaternion Rotation { get; }

        Vector3 Direction { get; }

        bool IsDebuggerEnabled { get; }

        float MoveSpeed { get; }

        float TargetMoveSpeed { get; }

        void SetOffside(bool value);

        PlayerAnimator Animator { get; }
        PlayerUI UI { get; }

        /// <summary>
        /// Enable / disable player UI
        /// </summary>
        /// <param name="value"></param>
        void SetUI(bool value);

        bool IsAnimationABlocker(in string[] clips);

        void SetPlayer(int number, PlayerBase playerBase, Material kitMaterial);

        void SetAsLineReferee();

        /// <summary>
        /// Move to a direction vector (smooth)
        /// </summary>
        /// <param name="to"></param>
        bool MoveTo(
            in float dT, 
            Vector3 to, 
            bool faceTowards = true, 
            MovementType movementType = MovementType.BestHeCanDo);
        void Stop(in float dT);

        void SetInstantPosition(Vector3 position);
        void SetInstantRotation(Quaternion rotation);

        /// <summary>
        /// Look to a direction vector
        /// </summary>
        /// <param name="to"></param>
        bool LookTo(in float dT, Vector3 to);

        void Up(in float dT, MatchStatus matchStatus, Ball ball);

        void SetHeadLook(in float dT, Vector3 target, float weight);

        bool HitBall(
        in Vector3 targetVelocity, 
        PlayerAnimatorVariable animatorVariable, 
        out PlayerAnimatorVariable result, 
        in float ballHoldTime,
        bool disableVolley = false);

        void ProcessMovement(in float time, in float dT);

        void BallHitEvent();
    }
}