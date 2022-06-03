using TheRealDelep.Physics.Interfaces;

namespace DashAttack.Game.Behaviours
{
    public abstract class BaseBehaviour<TData, TInput> : IBehaviour<TData, TInput>
        where TInput : ICharacterInputs
    {
        protected IPhysicsObject physicsObject;
        protected TData data;
        protected TInput input;

        public abstract bool IsExecuting { get; }

        public virtual void Init(IPhysicsObject physicsObject, TData data, TInput input)
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