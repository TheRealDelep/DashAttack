using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;

using UnityEngine;

using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerWallClimbingState : PlayerState
    {
        public PlayerWallClimbingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, WallClimbing)
        {
        }

        public override void OnStateUpdate()
        {
            var hasCollisionOnSide = Context.Collisions.Right || Context.Collisions.Left;
            if (!hasCollisionOnSide || Context.RunInputDirection == HorizontalDirection.None)
            {
                StateMachine.TransitionTo(Jumping);
                return;
            }

            var wallDirection = Context.Collisions.Right
                ? HorizontalDirection.Right
                : HorizontalDirection.Left;

            if (Context.RunInputDirection != wallDirection)
            {
                StateMachine.TransitionTo(RunningJumping);
                return;
            }

            if (Context.Collisions.Top ||
                Context.VerticalVelocity < Mathf.Epsilon ||
                !Context.JumpInput)
            {
                StateMachine.TransitionTo(WallSliding);
                return;
            }

            var jumpRequested = Context.JumpInputDown || Context.TimeSinceJumpInputDown < Data.LateJumpBuffer;
            if (jumpRequested)
            {
                StateMachine.TransitionTo(WallJumping);
                return;
            }

            base.OnStateUpdate();
        }
    }
}