using System;
using System.Collections.Generic;

using static DashAttack.Core.StateMachine.StateEvent;

namespace DashAttack.Core.StateMachine
{
    public class StateMachine<TStateEnum>
        where TStateEnum : Enum
    {
        private event Action afterStateUpdate;
        private event Action beforeStateUpdate;

        private Dictionary<TStateEnum, State<TStateEnum>> states = new();

        public TStateEnum CurrentState { get; private set; }

        public TStateEnum PreviousState { get; private set; }

        public void AddState(TStateEnum stateEnum, Action onStateEnter = null, Action onStateLeave = null, Action onStateUpdate = null)
        {
            if (states.ContainsKey(stateEnum))
            {
                throw new ArgumentException($"StateMachine already defines a state for type {stateEnum}");
            }

            var state = new State<TStateEnum>(stateEnum);

            state.StateEntered += onStateEnter;
            state.StateLeft += onStateLeave;
            state.StateUpdated += onStateUpdate;

            states.Add(stateEnum, state);
        }

        public void Start(TStateEnum entryState)
        {
            if (!states.ContainsKey(entryState))
            {
                throw new ArgumentException($"StateMachine doesn't contain a state of type {entryState}");
            }

            CurrentState = entryState;
        }

        public void RunMachine()
        {
            beforeStateUpdate?.Invoke();
            states[CurrentState].OnStateUpdate();
            afterStateUpdate?.Invoke();
        }

        public void TransitionTo(TStateEnum nextState)
        {
            PreviousState = CurrentState;

            states[CurrentState].OnStateLeave();
            CurrentState = nextState;

            states[CurrentState].OnStateEnter();
            states[CurrentState].OnStateUpdate();
        }

        public void Subscribe(TStateEnum state, StateEvent stateEvent, Action action)
        {
            switch (stateEvent)
            {
                case OnEnter:
                    states[state].StateEntered += action;
                    break;

                case OnLeave:
                    states[state].StateLeft += action;
                    break;

                case OnUpdate:
                    states[state].StateUpdated += action;
                    break;
            }
        }

        public void SubscribeAfterUpdate(Action action)
            => afterStateUpdate += action;

        public void SubscribeBeforeUpdate(Action action)
            => beforeStateUpdate += action;
    }
}