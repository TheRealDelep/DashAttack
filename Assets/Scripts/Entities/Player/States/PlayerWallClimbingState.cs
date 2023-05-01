using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;

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
            if (Context.ExitingWall)
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

            if (Context.EndOfJump)
            {
                StateMachine.TransitionTo(WallSliding);
                return;
            }

            if (Context.TimeSinceJumpInputDown < Data.LateJumpBuffer)
            {
                StateMachine.TransitionTo(WallJumping);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
