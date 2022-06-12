using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IWallJumpData : IMovementBehaviourData
    {
        Vector2 ImpulseVelocity { get; }

        Vector2 Deceleration { get; }
    }
}