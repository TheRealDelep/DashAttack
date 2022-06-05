namespace DashAttack.Game.Behaviours.WallJump
{
    public interface IWallJumpInput : ICharacterInputs
    {
        bool JumpPressedThisFixedFrame { get; }
    }
}
