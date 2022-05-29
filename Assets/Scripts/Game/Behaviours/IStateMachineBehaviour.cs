using System;
using TheRealDelep.StateMachine;

namespace DashAttack.Game.Behaviours
{
    public interface IStateMachineBehaviour<TData, TInput, TState> : IBehaviour<TData, TInput>
        where TInput : ICharacterInputs
        where TState : Enum
    {
        TState CurrentState { get; }

        void Subscribe(TState state, StateEvent stateEvent, Action action);
    }
}
