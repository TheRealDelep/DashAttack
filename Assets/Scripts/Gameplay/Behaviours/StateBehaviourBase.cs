using DashAttack.Assets.Scripts.Gameplay.Behaviours.Interfaces;
using DashAttack.Gameplay.Behaviours.Interfaces;
using DashAttack.Utilities.StateMachine;

using System;

namespace DashAttack.Gameplay.Behaviours
{
    public abstract class StateBehaviourBase<TData, TContext, TState> : IBehaviour
        where TData : IBehaviourData
        where TContext : IBehaviourContext
        where TState : Enum
    {
        protected TData Data;
        protected TContext Context;

        protected readonly StateMachine<TState> stateMachine;

        protected abstract TState EntryState { get; }

        public TState CurrentState => stateMachine.CurrentState;

        public TState PreviousState => stateMachine.PreviousState;

        protected StateBehaviourBase(TData data, TContext context)
        {
            Data = data;
            Context = context;
            stateMachine = new StateMachine<TState>();

            InitStates();
            stateMachine.Start(EntryState);
        }

        public void TransitionTo(TState nextState)
            => stateMachine.TransitionTo(nextState);

        public virtual void Update()
            => stateMachine.RunMachine();

        public void AddState(
            TState state,
            Action onStateEnter = null,
            Action onStateUpdate = null,
            Action onStateLeave = null)
        {
            stateMachine.AddState(state, onStateEnter, onStateUpdate, onStateLeave);
        }

        public void Subscribe(TState state, StateEvent stateEvent, Action callBack)
        {
            stateMachine.Subscribe(state, stateEvent, callBack);
        }

        protected abstract void InitStates();
    }
}