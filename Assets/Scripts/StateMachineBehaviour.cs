using System;

namespace DashAttack
{
    public abstract class StateMachineBehaviour<TData, TInput, TState> :
        BaseBehaviour<TData, TInput>,
        IStateMachineBehaviour<TData, TInput, TState>
        where TInput : IBehaviourContext
        where TState : Enum
    {
        protected StateMachine<TState> stateMachine;

        public virtual TState CurrentState => stateMachine.CurrentState;

        public override void Init(IPhysicsObject physicsObject, TData data, TInput input)
        {
            base.Init(physicsObject, data, input);
            InitStateMachine();
        }

        public void Subscribe(TState state, StateEvent stateEvent, Action action)
                    => stateMachine.Subscribe(state, stateEvent, action);

        public override void Update()
        {
            stateMachine.RunMachine();
        }

        protected virtual void InitStateMachine()
        {
            stateMachine = new();
        }
    }
}