using DashAttack.Physics;

namespace DashAttack.Gameplay.Behaviours.Interfaces
{
    public interface IBehaviour
    {
        bool IsExecuting { get; }

        void Update();

        void Reset();
    }

    public interface IBehaviour<TData, TContext> : IBehaviour
        where TData : IBehaviourData
        where TContext : IBehaviourContext
    {
        void Init(IPhysicsObject physicsObject, TData data, TContext input);
    }
}