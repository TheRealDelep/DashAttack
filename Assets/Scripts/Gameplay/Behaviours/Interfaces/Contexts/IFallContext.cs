namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IFallContext : IBehaviourContext
    {
        float VerticalVelocity { get; set; }
    }
}