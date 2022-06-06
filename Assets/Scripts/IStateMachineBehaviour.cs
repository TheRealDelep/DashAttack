using System;

namespace DashAttack
{
    public interface IStateMachineBehaviour<TData, TInput, TState> : IBehaviour<TData, TInput>
        where TInput : IBehaviourContext
        where TState : Enum
    {
        TState CurrentState { get; }

        void Subscribe(TState state, StateEvent stateEvent, Action action);
    }
}