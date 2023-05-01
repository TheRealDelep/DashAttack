using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;
using UnityEngine;
using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerRunningJumpingState : PlayerState
    {
        public PlayerRunningJumpingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, RunningJumping)
        {
        }

        public override void OnStateUpdate()
        {
            if (Context.RunInputDirection is HorizontalDirection.None && Context.HorizontalVelocity == 0)
            {
                StateMachine.TransitionTo(Jumping);
                return;
            }

            if (Context.EndOfJump)
            {
                StateMachine.TransitionTo(RunningFalling);
                return;
            }

            if ((Context.Collisions.Right && Context.RunInputDirection is HorizontalDirection.Right) ||
                (Context.Collisions.Left && Context.RunInputDirection is HorizontalDirection.Left))
            {
                StateMachine.TransitionTo(WallClimbing);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
