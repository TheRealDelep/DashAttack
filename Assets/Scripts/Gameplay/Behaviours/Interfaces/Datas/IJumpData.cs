namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IJumpData : IMovementBehaviourData
    {
        float Gravity { get; }

        float JumpVelocity { get; }

        float WallSlideMultiplier { get; }

        float WallClimbMultiplier { get; }
    }
}