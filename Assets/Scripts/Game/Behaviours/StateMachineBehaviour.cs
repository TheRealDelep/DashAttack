using System;
using TheRealDelep.Physics.Interfaces;
using TheRealDelep.StateMachine;
using UnityEngine;

namespace DashAttack.Game.Behaviours
{
    public abstract class StateMachineBehaviour<TData, TInput, TState> : IStateMachineBehaviour<TData, TInput, TState>
        where TInput : ICharacterInputs
        where TState : Enum
    {
        protected StateMachine<TState> stateMachine;

        protected IPhysicsObject physicsObject;
        protected TData data;
        protected TInput input;

        public abstract TState CurrentState { get; }

        public void Subscribe(TState state, StateEvent stateEvent, Action action)
            => stateMachine.Subscribe(state, stateEvent, action);

        public void Execute()
        {
            stateMachine.RunMachine();
        }

        public abstract void Reset();

        protected abstract void InitStateMachine();

        public void Init(IPhysicsObject physicsObject, TData data, TInput input)
        {
            this.physicsObject = physicsObject;
            this.data = data;
            this.input = input;

            InitStateMachine();
        }
    }
}