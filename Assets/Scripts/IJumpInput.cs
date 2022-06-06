namespace DashAttack
{
    public interface IJumpInput : IBehaviourContext
    {
        public bool Jump { get; }

        public bool JumpPressedThisFixedFrame { get; }
    }
}