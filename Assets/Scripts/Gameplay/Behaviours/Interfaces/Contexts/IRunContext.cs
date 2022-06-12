namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IRunContext : IMovementBehaviourContext
    {
        float RunDirection { get; }
    }
}