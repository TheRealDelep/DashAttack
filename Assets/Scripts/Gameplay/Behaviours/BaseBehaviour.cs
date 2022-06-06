using DashAttack.Gameplay.Behaviours.Interfaces;
using DashAttack.Physics;

namespace DashAttack.Gameplay.Behaviours
{
    public abstract class BaseBehaviour<TData, TContext> : IBehaviour<TData, TContext>
        where TData : IBehaviourData
        where TContext : IBehaviourContext
    {
        protected IPhysicsObject physicsObject;
        protected TData data;
        protected TContext input;

        public abstract bool IsExecuting { get; }

        public virtual void Init(IPhysicsObject physicsObject, TData data, TContext input)
        {
            this.physicsObject = physicsObject;
            this.data = data;
            this.input = input;
        }

        public virtual void Reset()
        {
        }

        public abstract void Update();
    }
}