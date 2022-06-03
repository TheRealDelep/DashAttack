using TheRealDelep.Physics.Interfaces;

namespace DashAttack.Game.Behaviours
{
    public interface IBehaviour<TData, TInputs>
        where TInputs : ICharacterInputs
    {
        bool IsExecuting { get; }

        void Init(IPhysicsObject physicsObject, TData data, TInputs input);

        void Update();

        void Reset();
    }
}
