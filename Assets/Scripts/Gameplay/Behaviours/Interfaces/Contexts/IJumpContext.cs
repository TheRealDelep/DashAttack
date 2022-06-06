namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IJumpContext : IBehaviourContext
    {
        public bool Jump { get; }

        public bool JumpPressedThisFixedFrame { get; }
    }
}