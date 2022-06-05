using DashAttack.Game.Behaviours.Fall;
using DashAttack.Game.Behaviours.Jump;
using DashAttack.Game.Behaviours.Run;
using DashAttack.Game.Behaviours.WallJump;
using DashAttack.Game.Managers;

namespace DashAttack.Game.Models
{
    public class PlayerInputs : IRunInput, IFallInput, IJumpInput, IWallJumpInput
    {
        public float RunDirection => InputManager.Instance.Move;

        public bool Jump => InputManager.Instance.Jump;

        public bool JumpPressedThisFixedFrame => InputManager.Instance.JumpPressedThisFixedFrame;
    }
}