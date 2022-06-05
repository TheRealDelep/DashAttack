namespace DashAttack.Game.Behaviours.WallJump
{
    public interface IWallJumpInput : IBehaviourContext
    {
        bool JumpPressedThisFixedFrame { get; }
    }
}