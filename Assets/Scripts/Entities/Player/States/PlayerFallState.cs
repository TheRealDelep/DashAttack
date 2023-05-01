using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.Enums;
using static DashAttack.Entities.Player.PlayerStateEnum;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine)
            : base(data, context, stateMachine, Falling)
        {
        }

        public override void OnStateUpdate()
        {
            if (Context.Collisions.Bottom)
            {
                StateMachine.TransitionTo(Idle);
                return;
            }

            if (Context.RunningIntoWall)
            {
                StateMachine.TransitionTo(WallSliding);
                return;
            }

            if (Context.RunInputDirection is not HorizontalDirection.None)
            {
                StateMachine.TransitionTo(RunningFalling);
                return;
            }

            base.OnStateUpdate();
        }
    }
}
