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

        public override Vector2 Velocity
        {
            get => currentVelocity;
            set => currentVelocity = value;
        }
        
        private bool IsOnWall 
            => Context.Collisions.Left || Context.Collisions.Right || Context.TimeSinceCollisionOnSide < Data.LateJumpBuffer;

        private bool JumpInputDown
            => Context.JumpInputDown || Context.TimeSinceJumpInputDown < Data.EarlyJumpBuffer;

        public WallJump(IWallJumpData data, IWallJumpContext context)
            : base(data, context)
        {
        }

        protected override void OnBehaviourStart()
        {
            direction = Context.Collisions.Left ? 1 : -1;
            currentVelocity = new Vector2(Data.WallJumpVelocity.x * direction, Data.WallJumpVelocity.y);
        }

        protected override void OnBehaviourEnd()
            => currentVelocity = Vector2.zero;

        public override void UpdateState()
        {
            bool isGrounded = Context.Collisions.Bottom;

            if (IsExecuting)
            {
                Vector2 deceleration = new(Data.WallJumpDeceleration.x * direction, Data.WallJumpDeceleration.y);
                currentVelocity -= deceleration * Context.DeltaTime;

                if (Mathf.Abs(currentVelocity.x) <= Data.MaxSpeed + 0.0001f)
                {
                    IsExecuting = false;
                }
            }
            else if (JumpInputDown && !isGrounded && IsOnWall)
            {
                IsExecuting = true;
            }
        }
    }
}