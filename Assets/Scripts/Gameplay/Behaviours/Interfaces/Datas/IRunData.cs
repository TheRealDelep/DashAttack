namespace DashAttack.Gameplay.Behaviours.Interfaces.Datas
{
    public interface IRunData : IMovementBehaviourData
    {
        float MaxSpeed { get; }

        float AccelerationTime { get; }

        float BrakingTime { get; }

        float TurningTime { get; }

        float AirControlAmount { get; }
    }
}