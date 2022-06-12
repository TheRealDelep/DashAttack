namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IJumpData : IMovementBehaviourData
    {
        float Gravity { get; }

        float JumpVelocity { get; }
        
        float WallClimbMultiplier { get; }
        
        float EarlyJumpBuffer { get; }
        
        float LateJumpBuffer { get; }
    }
}