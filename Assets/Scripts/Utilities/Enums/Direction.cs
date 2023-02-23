using UnityEngine;

namespace DashAttack.Utilities.Enums
{
    public enum Direction
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
    }

    public static class DirectionExtensions
    {
        public static Vector2 ToVector2(Direction direction)
            => direction switch
            {
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                Direction.Up => Vector2.up,
                Direction.Down => Vector2.down,
                _ => Vector2.zero
            };
    }
}