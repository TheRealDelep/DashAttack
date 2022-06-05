using UnityEngine;

namespace DashAttack.Game.Behaviours.WallJump
{
    public interface IWallJumpData
    {
        Vector2 ImpulseVelocity { get; }
        Vector2 Deceleration { get; }
    }
}
