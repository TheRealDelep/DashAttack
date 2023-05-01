using DashAttack.Utilities.Enums;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IWallJumpContext : IBehaviourContext
    {
        HorizontalDirection RunInputDirection { get; }
        public float TimeSinceJumpInputDown { get; }

        float HorizontalVelocity { get; set; }
        float VerticalVelocity { get; set; }
    }
}
