using TheRealDelep.Physics.Interfaces;

namespace DashAttack.Game.Behaviours
{
    public interface IBehaviour<TData, TInputs>
        where TInputs : ICharacterInputs
    {
        void Init(IPhysicsObject physicsObject, TData data, TInputs input);

        void Execute();

        void Reset();
    }
}
