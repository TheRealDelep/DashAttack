using System.Collections.Generic;
using UnityEngine;

namespace DashAttack.Physics
{
    public struct CollisionInfos
    {
        public bool Left { get; private set; }

        public bool Right { get; private set; }

        public bool Top { get; private set; }

        public bool Bottom { get; private set; }

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
                if (hit.normal == Vector2.right)
                {
                    left = true;
                }
                else if (hit.normal == Vector2.left)
                {
                    right = true;
                }
                else if (hit.normal == Vector2.down)
                {
                    top = true;
                }
                else if (hit.normal == Vector2.up)
                {
                    bottom = true;
                }
            }

            return new(left, right, top, bottom);
        }
    }
}