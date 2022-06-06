namespace DashAttack
{
    public interface IRunData
    {
        float MaxSpeed { get; }

        float AccelerationTime { get; }

        float BrakingTime { get; }

        float TurningTime { get; }

        float AirControlAmount { get; }
    }
}