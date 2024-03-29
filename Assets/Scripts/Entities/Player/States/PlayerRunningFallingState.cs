﻿using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;

using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerRunningFallingState : PlayerState
    {
        public PlayerRunningFallingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, RunningFalling)
        {
        }

        public override void OnStateUpdate()
        {
            if (Context.RunInputDirection is HorizontalDirection.None && Context.HorizontalVelocity == 0)
            {
                StateMachine.TransitionTo(Falling);
                return;
            }

            if (Context.Collisions.Bottom)
            {
                StateMachine.TransitionTo(Running);
                return;
            }

            if (Context.RunningIntoWall)
            {
                StateMachine.TransitionTo(WallSliding);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
