using UnityEngine;

namespace DashAttack.Entities.Player
{
    public enum PlayerStateEnum
    {
        Idle = 0,
        Running = 1,
        Falling = 2,
        Jumping = 3,
        WallSliding = 4,
        WallClimbing = 5,
        RunningFalling = 6,
        RunningJumping = 7,
        WallJumping = 8
    }
}