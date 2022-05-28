using System;
using System.Collections.Generic;
using System.Linq;
using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace TheRealDelep.Physics.Concretes
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollisionDetector : MonoBehaviour, ICollisionDetector
    {
        private const float skinWidth = .015f;

        [SerializeField]
        private int nbRays;

        [Header("Debug"), SerializeField]
        private bool drawDebugRays;

        [SerializeField]
        private Color debugRayColor;

        private BoxCollider2D boxCollider;
        private RaycastHit2D[] hitBuffer;


        public int NbRays
        {
            get => nbRays;
            set
            {
                nbRays = Mathf.Clamp(value, 2, int.MaxValue);
                hitBuffer = new RaycastHit2D[(nbRays * 2) + 1];
            }
        }

        public IEnumerable<RaycastHit2D> GetNearestCollisions(Vector2 movement)
        {
            CheckHorizontal(movement.x);
            CheckVertical(movement.y);

            var hits = hitBuffer.Where(h => h.collider is not null);

            if (!hits.Any())
            {
                return Enumerable.Empty<RaycastHit2D>();
            }

            return hits
                .GroupBy(h => h.normal)
                .Select(group => group.OrderBy(h => h.distance).First())
                .ToList();
        }

        private void CheckHorizontal(float x)
        {
            if (x == 0)
            {
                return;
            }

            var space = GetRaySpacing(boxCollider.bounds.size.y);

            var firstRayPosition = boxCollider.attachedRigidbody.position + new Vector2(
                MathF.Sign(x) * (boxCollider.bounds.extents.x - skinWidth),
                -(boxCollider.bounds.extents.y - skinWidth));

            CastRays(
                firstRayPosition: firstRayPosition,
                axis: Vector2.up,
                direction: Vector2.right * Mathf.Sign(x),
                distance: Mathf.Abs(x) + skinWidth,
                space: space);
        }

        private void CheckVertical(float y)
        {
            if (y == 0)
            {
                return;
            }

            var space = GetRaySpacing(boxCollider.bounds.size.x);

            var firstRayPosition = boxCollider.attachedRigidbody.position + new Vector2(
                -(boxCollider.bounds.extents.x - skinWidth),
                MathF.Sign(y) * (boxCollider.bounds.extents.y - skinWidth));

            CastRays(
                firstRayPosition: firstRayPosition,
                axis: Vector2.right,
                direction: Vector2.up * Mathf.Sign(y),
                distance: Mathf.Abs(y),
                space: space);
        }

        private void CheckDiagonal(Vector2 movement)
        {
            if (movement.x == 0 || movement.y == 0)
            {
                return;
            }
        }

        private void CastRays(Vector2 firstRayPosition, Vector2 axis, Vector2 direction, float space, float distance, int bufferStartIndex = 0)
        {
            for (int i = 0; i < NbRays; i++)
            {
                var rayOrigin = firstRayPosition + (i * space * axis);
                var hit = Physics2D.Raycast(rayOrigin, direction, distance);

                if (hit)
                {
                    hit.distance -= skinWidth;
                }

                hitBuffer[i + bufferStartIndex] = hit;

#if UNITY_EDITOR
                //if (hit)
                //{
                //    Debug.Log("Hit");
                //}

                //if (drawDebugRays)
                //{
                //    Debug.DrawRay(rayOrigin, direction * distance, debugRayColor);
                //}
#endif
            }
        }

        private float GetRaySpacing(float size)
            => (size - (skinWidth * 2)) / (nbRays - 1);

        private void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnValidate()
        {
            NbRays = nbRays;
        }
    }
}