using System.Collections.Generic;
using UnityEngine;

namespace DashAttack.Physics
{
    public interface ICollisionDetector
    {
        IEnumerable<RaycastHit2D> GetNearestCollisions(Vector2 movement);
    }
}