using System;

using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class WallJump : MovementBehaviourBase<IWallJumpData, IWallJumpContext>
    {
        private Vector2 currentVelocity;
        private float direction;

        public override Vector2 Velocity => currentVelocity;

        public WallJump(IWallJumpData data, IWallJumpContext context)
            : base(data, context)
        {
        }

        protected override void OnBehaviourStart()
        {
            direction = Context.Collisions.Left ? 1 : -1;
            currentVelocity = new Vector2(Data.ImpulseVelocity.x * direction, Data.ImpulseVelocity.y);
        }

        protected override void OnBehaviourEnd()
            => currentVelocity = Vector2.zero;

        public override void UpdateState()
        {
            bool isGrounded = Context.Collisions.Bottom;
            bool isOnWall = Context.Collisions.Left || Context.Collisions.Right;

            if (IsExecuting)
            {
                Vector2 deceleration = new(Data.Deceleration.x * direction, Data.Deceleration.y);
                currentVelocity -= deceleration * Context.DeltaTime;

                if (currentVelocity.x * -direction >= 0)
                {
                    IsExecuting = false;
                }
            }
            else if (Context.JumpInputDown && !isGrounded && isOnWall)
            {
                IsExecuting = true;
            }
        }
    }
}