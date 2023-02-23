using DashAttack.Utilities.Enums;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IWallJumpContext : IBehaviourContext
    {
        HorizontalDirection RunInputDirection { get; }

        bool JumpInputDown { get; }

        float TimeSinceCollisionOnSide { get; }

        float TimeSinceJumpInputDown { get; }
    }
}