using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static DashAttack.Utilities.StateMachine.StateEvent;

namespace DashAttack.Utilities.StateMachine
{
    public class StateMachine<TStateEnum>
        where TStateEnum : Enum
    {
        private event Action AfterStateUpdate;
        private event Action BeforeStateUpdate;

        public event Action<TStateEnum, TStateEnum> OnStateChange;

        private Dictionary<TStateEnum, State<TStateEnum>> states = new();

        public TStateEnum CurrentState { get; private set; }

        public TStateEnum PreviousState { get; private set; }

        public TStateEnum EntryState { get; private set; }

        public bool LogTransitions { get; set; }

#if UNITY_EDITOR
        private List<TStateEnum> transitionsThisFrame = new();
#endif

        public void AddState(
            TStateEnum stateEnum,
            Action onStateEnter = null,
            Action onStateUpdate = null,
            Action onStateLeave = null)
        {
            AddState(new State<TStateEnum>(stateEnum), onStateEnter, onStateUpdate, onStateLeave);
        }

        public void AddState<TState>(
            TState state,
            Action onStateEnter = null,
            Action onStateUpdate = null,
            Action onStateLeave = null)
            where TState : State<TStateEnum>
        {
            if (states.ContainsKey(state.Type))
            {
                throw new ArgumentException($"StateMachine already defines a state for type {state.Type}");
            }

            state.StateEntered += onStateEnter;
            state.StateLeft += onStateLeave;
            state.StateUpdated += onStateUpdate;

            states.Add(state.Type, state);
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
#if UNITY_EDITOR
            if (transitionsThisFrame.Any())
            {
                transitionsThisFrame.Clear();
            }
#endif

            BeforeStateUpdate?.Invoke();
            states[CurrentState].OnStateUpdate();
            AfterStateUpdate?.Invoke();
        }

        public void TransitionTo(TStateEnum nextState)
        {
#if UNITY_EDITOR
            if (LogTransitions)
            {
                Debug.Log($"Transition from {CurrentState} to {nextState}");
            }

            if (transitionsThisFrame.Contains(CurrentState))
            {
                Debug.LogError($"Circular transition between states of type: {typeof(TStateEnum)}.  {string.Join(" => ", transitionsThisFrame)}");
                return;
            }

            transitionsThisFrame.Add(CurrentState);
#endif

            PreviousState = CurrentState;

            states[CurrentState].OnStateLeave();
            CurrentState = nextState;

            states[CurrentState].OnStateEnter();

            OnStateChange?.Invoke(PreviousState, CurrentState);

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