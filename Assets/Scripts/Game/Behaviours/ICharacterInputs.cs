using TheRealDelep.Physics.Interfaces;

namespace DashAttack.Game.Behaviours
{
    public interface ICharacterInputs
    {
        void Init(IPhysicsObject physicsObject);
        void UpdateInputs();
    }
}