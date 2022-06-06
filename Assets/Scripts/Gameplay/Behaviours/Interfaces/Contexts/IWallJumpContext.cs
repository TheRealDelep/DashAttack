namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IWallJumpContext : IBehaviourContext
    {
        bool JumpPressedThisFixedFrame { get; }
    }
}