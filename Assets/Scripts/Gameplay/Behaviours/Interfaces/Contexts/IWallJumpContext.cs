namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IWallJumpContext : IMovementBehaviourContext
    {
        bool JumpInputDown { get; }

        float TimeSinceCollisionOnSide { get; }
        float TimeSinceJumpInputDown { get; }
    }
}