using System.Collections.Generic;
using UnityEngine;

namespace TheRealDelep.Physics.Interfaces
{
    public interface ICollisionDetector
    {
        IEnumerable<RaycastHit2D> GetNearestCollisions(Vector2 movement);
    }
}