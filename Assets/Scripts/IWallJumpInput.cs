namespace DashAttack
{
    public interface IWallJumpInput : IBehaviourContext
    {
        bool JumpPressedThisFixedFrame { get; }
    }
}