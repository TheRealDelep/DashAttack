using DashAttack.Assets.Scripts.Entities.Player.States;
using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Utilities.Enums;
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
            if (Context.VerticalVelocity <= 0 || Context.Collisions.Top)
            {
                StateMachine.TransitionTo(RunningFalling);
                return;
            }

            var sideCollision = Context.HorizontalVelocity.ToHorizontalDirection() == HorizontalDirection.Right
                ? Context.Collisions.Right
                : Context.Collisions.Left;

            if (sideCollision)
            {
                StateMachine.TransitionTo(WallClimbing);
                return;
            }

            base.OnStateUpdate();
        }
    }
}