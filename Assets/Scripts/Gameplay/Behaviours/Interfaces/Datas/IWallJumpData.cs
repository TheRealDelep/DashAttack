using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IWallJumpData : IMovementBehaviourData
    {
        float MaxSpeed { get; }
        
        Vector2 WallJumpVelocity { get; }

        Vector2 WallJumpDeceleration { get; }
    }
}