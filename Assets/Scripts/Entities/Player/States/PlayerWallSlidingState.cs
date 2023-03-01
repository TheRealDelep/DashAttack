﻿using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;
using System;
using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerWallSlidingState : PlayerState
    {
        public PlayerWallSlidingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, WallSliding)
        {
        }

        public override void OnStateUpdate()
        {
            var hasCollisionOnSide = !Context.Collisions.Right && !Context.Collisions.Left;
            if (hasCollisionOnSide || Context.RunInputDirection == HorizontalDirection.None)
            {
                StateMachine.TransitionTo(Falling);
                return;
            }

            var wallDirection = Context.Collisions.Right
                ? HorizontalDirection.Right
                : HorizontalDirection.Left;

            // Make wallstick ?
            if (Context.RunInputDirection != wallDirection)
            {
                StateMachine.TransitionTo(RunningFalling);
                return;
            }

            if (Context.Collisions.Bottom)
            {
                StateMachine.TransitionTo(Idle);
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