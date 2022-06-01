using System;
using TheRealDelep.Physics.Interfaces;
using TheRealDelep.StateMachine;
using UnityEngine;

namespace DashAttack.Game.Behaviours
{
    public abstract class StateMachineBehaviour<TData, TInput, TState> :
        BaseBehaviour<TData, TInput>,
        IStateMachineBehaviour<TData, TInput, TState>
        where TInput : ICharacterInputs
        where TState : Enum
    {
        protected StateMachine<TState> stateMachine;

        public virtual TState CurrentState => stateMachine.CurrentState;

        public void Subscribe(TState state, StateEvent stateEvent, Action action)
            => stateMachine.Subscribe(state, stateEvent, action);

        public override void Execute()
        {
            stateMachine.RunMachine();
        }

        public override void Init(IPhysicsObject physicsObject, TData data, TInput input)
        {
            base.Init(physicsObject, data, input);
            InitStateMachine();
        }

        protected virtual void InitStateMachine()
        {
            stateMachine = new();
        }
    }
}