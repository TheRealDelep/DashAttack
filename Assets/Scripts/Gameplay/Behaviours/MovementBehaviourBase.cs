using System;

using DashAttack.Gameplay.Behaviours.Enums;
using DashAttack.Gameplay.Behaviours.Interfaces;

using UnityEngine;

using static DashAttack.Gameplay.Behaviours.Enums.BehaviourState;

namespace DashAttack.Gameplay.Behaviours
{
    public abstract class MovementBehaviourBase<TData, TContext>
        where TData : IMovementBehaviourData
        where TContext : IMovementBehaviourContext
    {
        protected readonly TContext Context;
        protected readonly TData Data;

        private bool isExecuting;

        public event Action<BehaviourState, BehaviourState> OnStateChange;

        protected MovementBehaviourBase(TData data, TContext context)
        {
            Data = data;
            Context = context;
        }

        protected bool IsExecuting
        {
            get => isExecuting;
            set
            {
                if (value == isExecuting)
                {
                    return;
                }
                
                isExecuting = value;

                if (value)
                {
                    OnBehaviourStart();
                    OnStateChange?.Invoke(Rest, Executing);
                }
                else
                {
                    OnBehaviourEnd();
                    OnStateChange?.Invoke(Executing, Rest);
                }
            }
        }

        public abstract Vector2 Velocity { get; }

        public BehaviourState CurrentState
            => IsExecuting ? Executing : Rest;

        public void TransitionTo(BehaviourState nextState)
        {
            IsExecuting = nextState == Executing;
        }

        public abstract void UpdateState();

        protected abstract void OnBehaviourStart();

        protected abstract void OnBehaviourEnd();
    }
}