using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IWallJumpData : IBehaviourData
    {
        Vector2 ImpulseVelocity { get; }

        Vector2 Deceleration { get; }
    }
}