using Unity.VisualScripting;

namespace DashAttack.Utilities.Enums
{
    public enum HorizontalDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
    }

    public static class HorizontalDirectionExtensions
    {
        public static int ToInt(this HorizontalDirection direction)
            => direction switch
            {
                HorizontalDirection.Left => -1,
                HorizontalDirection.Right => 1,
                _ => 0
            };

        public static HorizontalDirection ToHorizontalDirection(this int direction)
            => direction switch
            {
                < 0 => HorizontalDirection.Left,
                > 0 => HorizontalDirection.Right,
                _ => HorizontalDirection.None,
            };

        public static HorizontalDirection ToHorizontalDirection(this float direction)
            => direction switch
            {
                < 0 => HorizontalDirection.Left,
                > 0 => HorizontalDirection.Right,
                _ => HorizontalDirection.None,
            };

        public static bool IsEqual(this HorizontalDirection direction, float directionFloat)
            => directionFloat.ToHorizontalDirection() == direction;
    }
}