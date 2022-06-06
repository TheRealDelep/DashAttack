using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DashAttack
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsObject : MonoBehaviour, IPhysicsObject
    {
        public event Action<IEnumerable<RaycastHit2D>> OnCollisionEnter;
        public event Action<IEnumerable<RaycastHit2D>> OnCollisionExit;

        private Rigidbody2D rb;
        private ICollisionDetector collisionDetector;

        public Vector2 Velocity { get; private set; }

        public IEnumerable<RaycastHit2D> CurrentCollisions { get; private set; } = Enumerable.Empty<RaycastHit2D>();

        public Vector2 Position
            => rb.velocity;

        public void Move(Vector2 movement)
            => Velocity += movement;

        public void Move(float x, float y)
            => Move(new(x, y));

        private void UpdatePosition()
        {
            var hits = collisionDetector.GetNearestCollisions(Velocity);
            foreach (var hit in hits)
            {
                CorrectVelocity(hit);
            }

            rb.MovePosition(rb.position + Velocity);
            UpdateCollisions(hits);

            Velocity = Vector2.zero;
        }

        private void CorrectVelocity(RaycastHit2D collision)
        {
            var dotX = Vector2.Dot(Vector2.left * Mathf.Sign(Velocity.x), collision.normal);
            var dotY = Vector2.Dot(Vector2.down * Mathf.Sign(Velocity.y), collision.normal);

            Velocity = new Vector2(
                dotX < 0.001f ? Velocity.x : collision.distance * Mathf.Sign(Velocity.x),
                dotY < 0.001f ? Velocity.y : collision.distance * Mathf.Sign(Velocity.y));
        }

        private void UpdateCollisions(IEnumerable<RaycastHit2D> hits)
        {
            var newCollisions = hits.Except(CurrentCollisions);
            var oldCollisions = CurrentCollisions.Except(hits);

            CurrentCollisions = hits;

            OnCollisionEnter?.Invoke(newCollisions);
            OnCollisionExit?.Invoke(oldCollisions);
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            collisionDetector = GetComponent<ICollisionDetector>();
        }

        private void OnEnable()
        {
            PhysicsManager.EndOfFixedFrame += UpdatePosition;
        }

        private void OnDisable()
        {
            PhysicsManager.EndOfFixedFrame -= UpdatePosition;
        }
    }
}