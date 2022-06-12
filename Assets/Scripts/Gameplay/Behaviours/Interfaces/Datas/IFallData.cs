namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IFallData : IMovementBehaviourData
    {
        float Gravity { get; }
        float WallSlideMultiplier { get; }
    }
}