namespace DashAttack
{
    public class PlayerInputs : IRunInput, IFallInput, IJumpInput, IWallJumpInput
    {
        public float RunDirection => InputManager.Instance.Move;

        public bool Jump => InputManager.Instance.Jump;

        public bool JumpPressedThisFixedFrame => InputManager.Instance.JumpPressedThisFixedFrame;
    }
}