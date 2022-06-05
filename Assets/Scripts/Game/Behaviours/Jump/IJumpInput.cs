namespace DashAttack.Game.Behaviours.Jump
{
    public interface IJumpInput : IBehaviourContext
    {
        public bool Jump { get; }

        public bool JumpPressedThisFixedFrame { get; }
    }
}