namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IJumpContext : IMovementBehaviourContext
    {
        bool JumpInput { get; }

        bool JumpInputDown { get; }
        
        float TimeSinceCollisionBelow { get; }
        float TimeSinceJumpInputDown { get; }
    }
}