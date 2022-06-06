using DashAttack.Utilities.StateMachine;
using System;

namespace DashAttack.Gameplay.Behaviours.Interfaces
{
    public interface IStateMachineBehaviour<TData, TInput, TState> : IBehaviour<TData, TInput>
        where TData : IBehaviourData
        where TInput : IBehaviourContext
        where TState : Enum
    {
        TState CurrentState { get; }

        void Subscribe(TState state, StateEvent stateEvent, Action action);
    }
}