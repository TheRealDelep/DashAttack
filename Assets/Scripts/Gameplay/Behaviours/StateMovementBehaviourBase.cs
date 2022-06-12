using DashAttack.Gameplay.Behaviours.Interfaces;
using DashAttack.Utilities.StateMachine;

using System;

using UnityEngine;

namespace DashAttack.Gameplay.Behaviours
{
    public abstract class StateMovementBehaviourBase<TData, TContext, TState>
        where TData : IMovementBehaviourData
        where TContext : IMovementBehaviourContext
        where TState : Enum
    {
        protected readonly TData Data;
        protected readonly TContext Context;

        private readonly StateMachine<TState> stateMachine;

        public event Action<TState, TState> OnStateChange
        {
            add => stateMachine.OnStateChange += value;
            remove => stateMachine.OnStateChange -= value;
        }

        public abstract Vector2 Velocity { get; set; }

        public TState CurrentState => stateMachine.CurrentState;
        public TState PreviousState => stateMachine.PreviousState;

        protected StateMovementBehaviourBase(TData data, TContext context)
        {
            Data = data;
            Context = context;
            stateMachine = new StateMachine<TState>();
        }

        public void TransitionTo(TState nextState)
            => stateMachine.TransitionTo(nextState);

        public void UpdateState()
            => stateMachine.RunMachine();

        public void AddState(
            TState state,
            Action onStateEnter = null,
            Action onStateUpdate = null,
            Action onStateLeave = null)
        {
            stateMachine.AddState(state, onStateEnter, onStateUpdate, onStateLeave);
        }

        protected void SubscribeBeforeUpdate(Action action)
            => stateMachine.SubscribeBeforeUpdate(action);

        protected void SubscribeAfterUpdate(Action action)
            => stateMachine.SubscribeAfterUpdate(action);

        protected void Start(TState entryState)
            => stateMachine.Start(entryState);
    }
}