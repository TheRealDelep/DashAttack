using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using System.Linq;
using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class WallJump : BaseBehaviour<IWallJumpData, IWallJumpContext>
    {
        private Vector2 currentVelocity;
        private bool isWallJumping;
        private float direction;

        public override bool IsExecuting => isWallJumping;

        private bool IsOnWall => physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.left || h.normal == Vector2.right);

        private bool isGrounded => physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.up);

        public override void Reset()
        {
            isWallJumping = false;
            currentVelocity = Vector2.zero;
        }

        public override void Update()
        {
            if (isWallJumping)
            {
                currentVelocity -= new Vector2(data.Deceleration.x * direction, data.Deceleration.y) * Time.fixedDeltaTime;

                if (currentVelocity.x * -direction >= 0)
                {
                    Reset();
                }
            }
            else if (input.JumpPressedThisFixedFrame && IsOnWall && !isGrounded)
            {
                isWallJumping = true;
                direction = physicsObject.CurrentCollisions.First(h => h.normal == Vector2.left || h.normal == Vector2.right).normal.x;
                currentVelocity = new(data.ImpulseVelocity.x * direction, data.ImpulseVelocity.y);
            }

            if (currentVelocity != Vector2.zero)
            {
                physicsObject.Move(currentVelocity * Time.fixedDeltaTime);
            }
        }
    }
}