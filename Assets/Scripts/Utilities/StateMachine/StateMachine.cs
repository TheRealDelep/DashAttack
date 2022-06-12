using System;
using System.Collections.Generic;

using static DashAttack.Utilities.StateMachine.StateEvent;

namespace DashAttack.Utilities.StateMachine
{
    public class StateMachine<TStateEnum>
        where TStateEnum : Enum
    {
        private event Action AfterStateUpdate;
        private event Action BeforeStateUpdate;

        private Dictionary<TStateEnum, State<TStateEnum>> states = new();

        public TStateEnum CurrentState { get; private set; }

        public TStateEnum PreviousState { get; private set; }

        public TStateEnum EntryState { get; private set; }

        public void AddState(
            TStateEnum stateEnum, 
            Action onStateEnter = null,
            Action onStateUpdate = null,
            Action onStateLeave = null)
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

            EntryState = entryState;
            CurrentState = EntryState;
        }

        public void RunMachine()
        {
            BeforeStateUpdate?.Invoke();
            states[CurrentState].OnStateUpdate();
            AfterStateUpdate?.Invoke();
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
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        }

        public void SubscribeAfterUpdate(Action action)
            => AfterStateUpdate += action;

        public void SubscribeBeforeUpdate(Action action)
            => BeforeStateUpdate += action;
    }
}