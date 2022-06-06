using System;

namespace DashAttack.Utilities.StateMachine
{
    public class State<TStateEnum>
        where TStateEnum : Enum
    {
        public event Action StateEntered;
        public event Action StateLeft;
        public event Action StateUpdated;

        public State(TStateEnum type)
        {
            Type = type;
        }

        public TStateEnum Type { get; private set; }

        public virtual void OnStateEnter()
            => StateEntered?.Invoke();

        public virtual void OnStateLeave()
            => StateLeft?.Invoke();

        public virtual void OnStateUpdate()
            => StateUpdated?.Invoke();
    }
}