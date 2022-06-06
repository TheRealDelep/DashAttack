namespace DashAttack
{
    public interface IJumpData
    {
        float Gravity { get; }

        float JumpVelocity { get; }

        float WallSlideMultiplier { get; }

        float WallClimbMultiplier { get; }
    }
}