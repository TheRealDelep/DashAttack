﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DashAttack.Physics
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollisionDetector : MonoBehaviour, ICollisionDetector
    {
        private const float SkinWidth = .015f;

        [SerializeField] private int nbRays;

        [Header("Debug")]
        [SerializeField] private bool drawDebugRays;
        [SerializeField] private Color debugRayColor;

        private BoxCollider2D boxCollider;
        private RaycastHit2D[] hitBuffer;

        private int NbRays
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
            var space = GetRaySpacing(boxCollider.bounds.size.y);

            var firstRayPosition = boxCollider.attachedRigidbody.position + new Vector2(
                MathF.Sign(x) * (boxCollider.bounds.extents.x - SkinWidth),
                -(boxCollider.bounds.extents.y - SkinWidth));

            CastRays(
                firstRayPosition: firstRayPosition,
                axis: Vector2.up,
                direction: Vector2.right * Mathf.Sign(x),
                distance: Mathf.Abs(x) + SkinWidth,
                space: space);
        }

        private void CheckVertical(float y)
        {
            var space = GetRaySpacing(boxCollider.bounds.size.x);

            var firstRayPosition = boxCollider.attachedRigidbody.position + new Vector2(
                -(boxCollider.bounds.extents.x - SkinWidth),
                MathF.Sign(y) * (boxCollider.bounds.extents.y - SkinWidth));

            CastRays(
                firstRayPosition: firstRayPosition,
                axis: Vector2.right,
                direction: Vector2.up * Mathf.Sign(y),
                distance: Mathf.Abs(y),
                space: space,
                bufferStartIndex: nbRays);
        }

        private void CastRays(Vector2 firstRayPosition, Vector2 axis, Vector2 direction, float space, float distance, int bufferStartIndex = 0)
        {
            for (int i = 0; i < NbRays; i++)
            {
                if (distance == 0)
                {
                    hitBuffer[i + bufferStartIndex] = default;
                }

                var rayOrigin = firstRayPosition + (i * space * axis);
                var hit = Physics2D.Raycast(rayOrigin, direction, distance + SkinWidth);

                if (hit)
                {
                    hit.distance -= SkinWidth;
                }

                hitBuffer[i + bufferStartIndex] = hit;

#if UNITY_EDITOR
                if (drawDebugRays)
                {
                    Debug.DrawRay(rayOrigin, direction * distance, debugRayColor);
                }
#endif
            }
        }

        private float GetRaySpacing(float size)
            => (size - (SkinWidth * 2)) / (nbRays - 1);

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