﻿using UnityEngine;

namespace DashAttack.Utilities.Enums
{
    public enum VerticalDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
    }

    public static class VerticalDirectionExtensions
    {
        public static int ToInt(this VerticalDirection direction)
            => direction switch
            {
                VerticalDirection.Down => -1,
                VerticalDirection.Up => 1,
                _ => 0
            };

        public static VerticalDirection ToVerticalDirection(this int direction)
            => direction switch
            {
                0 => VerticalDirection.None,
                > 0 => VerticalDirection.Up,
                < 0 => VerticalDirection.Down
            };

        public static VerticalDirection ToVerticalDirection(this float direction)
        {
            if (direction.IsCloseToZero())
            {
                return VerticalDirection.None;
            }

            return direction < 0 ? VerticalDirection.Down : VerticalDirection.Up;
        }
    }
}