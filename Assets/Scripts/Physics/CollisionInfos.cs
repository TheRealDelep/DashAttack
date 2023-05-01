using DashAttack.Utilities.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DashAttack.Physics
{
    public struct CollisionInfos
    {
        public bool Left { get; private set; }

        public bool Right { get; private set; }

        public bool Top { get; private set; }

        public bool Bottom { get; private set; }

        public override string ToString()
            => $"Left: {Left}, Right: {Right}, Top: {Top}, Bottom: {Bottom}";

        public CollisionInfos(bool left = false, bool right = false, bool top = false, bool bottom = false)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static CollisionInfos FromHits(IEnumerable<RaycastHit2D> hits)
        {
            bool left = false;
            bool right = false;
            bool top = false;
            bool bottom = false;

            foreach (var hit in hits)
            {
                var horizontal = hit.normal.x.ToHorizontalDirection();
                var vertical = hit.normal.y.ToVerticalDirection();

                switch (horizontal)
                {
                    case HorizontalDirection.Left:
                        right = true;
                        break;
                    case HorizontalDirection.Right:
                        left = true;
                        break;
                }

                switch (vertical)
                {
                    case VerticalDirection.Down:
                        top = true;
                        break;
                    case VerticalDirection.Up:
                        bottom = true;
                        break;
                }
            }

            return new(left, right, top, bottom);
        }
    }
}