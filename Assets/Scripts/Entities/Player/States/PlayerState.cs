using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player;
using DashAttack.Utilities.StateMachine;

namespace DashAttack.Assets.Scripts.Entities.Player.States
{
    public abstract class PlayerState : State<PlayerStateEnum>
    {
        protected PlayerData Data;
        protected PlayerContext Context;
        protected IStateMachine<PlayerStateEnum> StateMachine;

        protected PlayerState(PlayerData data, PlayerContext context, IStateMachine<PlayerStateEnum> stateMachine, PlayerStateEnum type)
            : base(type)
        {
            Data = data;
            Context = context;
            StateMachine = stateMachine;
        }
    }
}