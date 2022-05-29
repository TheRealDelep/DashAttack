using System;
using System.Collections.Generic;

using static TheRealDelep.StateMachine.StateEvent;

namespace TheRealDelep.StateMachine
{

    public class StateMachine<TStateEnum>
        where TStateEnum : Enum
    {
        private Dictionary<TStateEnum, State<TStateEnum>> states = new();

        private TStateEnum entryState;

        public TStateEnum CurrentState { get; private set; }
        public TStateEnum PreviousState { get; private set; }

        public void AddState(TStateEnum stateEnum, Action onStateEnter = null, Action onStateLeave = null, Action onStateUpdate = null, bool isEntryState = false)
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

            if (isEntryState)
            {
                entryState = stateEnum;
            }
        }

        public void RunMachine()
        {
            states[CurrentState].OnStateUpdate();
        }

        public void Start()
        {
            CurrentState = entryState;
        }

        public void TransitionTo(TStateEnum nextState)
        {
            states[CurrentState].OnStateLeave();
            CurrentState = nextState;
            states[CurrentState].OnStateEnter();
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

        public void SubscribeAnyState(StateEvent stateEvent, Action action)
        {
            foreach (var state in states)
            {
                Subscribe(state.Value.Type, stateEvent, action);
            }
        }
    }
}
