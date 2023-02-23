using DashAttack.Assets.Scripts.Entities.Player.States;
using DashAttack.Assets.Scripts.Utilities.StateMachine;

namespace DashAttack.Entities.Player.States
{
    public class PlayerWallJumpingState : PlayerState
    {
        public PlayerWallJumpingState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine, PlayerStateEnum type)
            : base(data, context, stateMachine, type)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateUpdate();
            StateMachine.TransitionTo(PlayerStateEnum.RunningJumping);
        }
    }
}