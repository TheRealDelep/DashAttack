using System.Collections.Generic;
using UnityEngine;

namespace DashAttack
{
    public interface ICollisionDetector
    {
        IEnumerable<RaycastHit2D> GetNearestCollisions(Vector2 movement);
    }
}