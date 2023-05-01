using DashAttack.Assets.Scripts.Entities.Player.States;
using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Utilities.Enums;
using UnityEngine;
using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Entities.Player.States
{
    public class PlayerWallJumpingState : PlayerState
    {
        public PlayerWallJumpingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, WallJumping)
        {
        }

        public override void OnStateUpdate()
        {
            if (Context.EndOfJump)
            {
                StateMachine.TransitionTo(RunningFalling);
                return;
            }

            if (Mathf.Abs(Context.HorizontalVelocity) <= Data.MaxSpeed &&
                (Context.RunInputDirection == Context.HorizontalVelocity.ToHorizontalDirection() ||
                Context.RunInputDirection is HorizontalDirection.None))
            {
                StateMachine.TransitionTo(RunningJumping);
            }

            if (Context.HasSideCollision)
            {
                StateMachine.TransitionTo(WallClimbing);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
