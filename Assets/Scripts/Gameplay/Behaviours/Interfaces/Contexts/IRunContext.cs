using DashAttack.Utilities.Enums;

namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IRunContext : IBehaviourContext
    {
        float HorizontalVelocity { get; set; }

        float LastFrameHorizontalVelocity { get; }

        HorizontalDirection RunInputDirection { get; }

        HorizontalDirection LastFrameRunInputDirection { get; }
    }
}