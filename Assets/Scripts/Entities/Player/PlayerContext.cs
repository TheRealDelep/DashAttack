using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Managers;

namespace DashAttack.Entities.Player
{
    public class PlayerContext : IRunContext, IFallContext, IJumpContext, IWallJumpContext
    {
        public float RunDirection => InputManager.Instance.Move;

        public bool Jump => InputManager.Instance.Jump;

        public bool JumpPressedThisFixedFrame => InputManager.Instance.JumpPressedThisFixedFrame;
    }
}