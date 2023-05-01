using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;
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

            if (Context.TimeSinceJumpInputDown < Data.LateJumpBuffer)
            {
                StateMachine.TransitionTo(WallJumping);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
