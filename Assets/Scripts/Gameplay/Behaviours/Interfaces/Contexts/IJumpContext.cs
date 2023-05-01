namespace DashAttack.Gameplay.Behaviours.Interfaces.Contexts
{
    public interface IJumpContext : IBehaviourContext
    {
        float VerticalVelocity { get; set; }
    }
}
