using DashAttack.Physics;

namespace DashAttack.Gameplay.Behaviours.Interfaces
{
    public interface IBehaviourContext
    {
        float DeltaTime { get; }

        CollisionInfos Collisions { get; }
    }
}