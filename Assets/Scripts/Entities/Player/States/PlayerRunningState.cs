using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;

using UnityEngine;

using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerRunningState : PlayerState
    {
        public PlayerRunningState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, Running)
        {
        }

        public override void OnStateUpdate()
        {
            if (Context.RunInputDirection is HorizontalDirection.None && Context.HorizontalVelocity == 0)
            {
                StateMachine.TransitionTo(Idle);
                return;
            }

            var jumpRequested = Context.JumpInputDown || Context.TimeSinceJumpInputDown < Data.LateJumpBuffer;
            if (Context.Collisions.Bottom && jumpRequested)
            {
                StateMachine.TransitionTo(RunningJumping);
                return;
            }

            if (!Context.Collisions.Bottom)
            {
                StateMachine.TransitionTo(RunningFalling);
            }

            base.OnStateUpdate();
        }
    }
}