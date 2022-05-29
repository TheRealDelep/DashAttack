namespace DashAttack.Game.Behaviours.Fall
{
    public interface IFallInput : ICharacterInputs
    {
        bool CanFall { get; }
    }
}