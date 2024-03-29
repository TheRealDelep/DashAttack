﻿namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IJumpData : IBehaviourData
    {
        float Gravity { get; }

        float JumpVelocity { get; }

        float WallClimbMultiplier { get; }
    }
}