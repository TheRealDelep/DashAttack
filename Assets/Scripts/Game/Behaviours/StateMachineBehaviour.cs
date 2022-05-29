using System;
using TheRealDelep.Physics.Interfaces;
using TheRealDelep.StateMachine;

namespace DashAttack.Game.Behaviours
{
    public abstract class StateMachineBehaviour<TData, TInput, TState> : IStateMachineBehaviour<TData, TInput, TState>
        where TInput : ICharacterInputs
        where TState : Enum
    {
        protected StateMachine<TState> stateMachine;

        public abstract TState CurrentState { get; }

        public void Subscribe(TState state, StateEvent stateEvent, Action action)
            => stateMachine.Subscribe(state, stateEvent, action);

        public virtual void Run(IPhysicsObject physicsObject, TData data, TInput input)
        {
            stateMachine.RunMachine();
        }

        protected virtual void Start()
        {
            InitStateMachine();
        }

        public abstract void Reset();

        protected abstract void InitStateMachine();
    }
}