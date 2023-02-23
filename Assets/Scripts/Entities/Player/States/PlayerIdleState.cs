using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;
using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, Idle)
        {
        }

        private bool CanJump
            => Context.Collisions.Bottom || Context.TimeSinceCollisionBelow < Data.LateJumpBuffer;

        public override void OnStateUpdate()
        {
            if (!Context.Collisions.Bottom)
            {
                StateMachine.TransitionTo(Falling);
                return;
            }

            if (Context.RunInputDirection is not HorizontalDirection.None)
            {
                StateMachine.TransitionTo(Running);
                return;
            }

            if (Context.JumpInputDown && CanJump)
            {
                StateMachine.TransitionTo(Jumping);
                return;
            }

            base.OnStateUpdate();
        }
    }
}