using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheRealDelep.Physics.Interfaces
{
    public interface IPhysicsObject
    {
        event Action<IEnumerable<RaycastHit2D>> OnCollisionEnter;

        event Action<IEnumerable<RaycastHit2D>> OnCollisionExit;

        IEnumerable<RaycastHit2D> CurrentCollisions { get; }

        Vector2 Position { get; }

        void Move(Vector2 movement);

        void Move(float x, float y);
    }
}
