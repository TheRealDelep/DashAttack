namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IJumpContext : IMovementBehaviourContext
    {
        public bool JumpInput { get; }

        public bool JumpInputDown { get; }
    }
}