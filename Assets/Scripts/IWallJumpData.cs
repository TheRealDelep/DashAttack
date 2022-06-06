using UnityEngine;

namespace DashAttack
{
    public interface IWallJumpData
    {
        Vector2 ImpulseVelocity { get; }

        Vector2 Deceleration { get; }
    }
}