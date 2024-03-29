﻿using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;
using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerJumpingState : PlayerState
    {
        public PlayerJumpingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, Jumping)
        {
        }

        public override void OnStateUpdate()
        {
            if (Context.EndOfJump)
            {
                StateMachine.TransitionTo(Falling);
                return;
            }

            if (Context.RunningIntoWall)
            {
                StateMachine.TransitionTo(WallClimbing);
                return;
            }

            if (Context.RunInputDirection is not HorizontalDirection.None)
            {
                StateMachine.TransitionTo(RunningJumping);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
