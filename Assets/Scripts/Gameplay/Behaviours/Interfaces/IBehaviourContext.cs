using DashAttack.Physics;

namespace DashAttack.Gameplay.Behaviours.Interfaces
{
    public interface IMovementBehaviourContext
    {
        float DeltaTime { get; }

        CollisionInfos Collisions { get; }
    }
}