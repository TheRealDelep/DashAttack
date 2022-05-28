using System.Threading.Tasks;
using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace TheRealDelep.Physics.Concretes
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsObject : MonoBehaviour, IPhysicsObject
    {
        public Vector2 Velocity { get; private set; }

        private Rigidbody2D rb;
        private ICollisionDetector collisionDetector;

        public void Move(float x, float y)
            => Velocity += new Vector2(x, y);

        private void UpdatePosition()
        {
            var collisions = collisionDetector.GetNearestCollisions(Velocity);
            foreach (var collision in collisions)
            {
                CorrectVelocity(collision);
            }

            rb.MovePosition(rb.position + Velocity);
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
