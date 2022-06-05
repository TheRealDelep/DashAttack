namespace DashAttack.Game.Behaviours.Run
{
    public interface IRunInput : IBehaviourContext
    {
        float RunDirection { get; }
    }
}