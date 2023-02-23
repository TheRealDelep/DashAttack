using System;

namespace DashAttack.Assets.Scripts.Utilities.StateMachine
{
    public interface IStateMachine<TState> where TState : Enum
    {
        void TransitionTo(TState nextState);
    }
}