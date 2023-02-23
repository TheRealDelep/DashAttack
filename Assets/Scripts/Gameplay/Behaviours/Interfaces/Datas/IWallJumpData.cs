using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IWallJumpData : IBehaviourData
    {
        float MaxSpeed { get; }

        float TurningTime { get; }

        Vector2 WallJumpVelocity { get; }

        Vector2 WallJumpDeceleration { get; }

        float EarlyJumpBuffer { get; }

        float LateJumpBuffer { get; }
    }
}