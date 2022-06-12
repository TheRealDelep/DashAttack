using System;

using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Jump : MovementBehaviourBase<IJumpData, IJumpContext>
    {
        private float currentVelocity;

        public override Vector2 Velocity
        {
            get => new(0, currentVelocity);
            set => currentVelocity = value.y;
        }

        private float WallMultiplier
            => Context.Collisions.Left && Context.Collisions.Right
                ? Data.WallClimbMultiplier
                : 1;

        private bool IsGrounded 
            => Context.Collisions.Bottom || Context.TimeSinceCollisionBelow < Data.LateJumpBuffer;

        private bool JumpInputDown
            => Context.JumpInputDown || Context.TimeSinceJumpInputDown < Data.EarlyJumpBuffer;

        public Jump(IJumpData data, IJumpContext context)
            : base(data, context)
        {
        }

        public override void UpdateState()
        {
            if (IsExecuting)
            {
                currentVelocity -= Data.Gravity * WallMultiplier * Context.DeltaTime;
                
                if (Context.Collisions.Top || currentVelocity <= 0.0001f || !Context.JumpInput)
                {
                    IsExecuting = false;
                }
            }
            else if (JumpInputDown && IsGrounded)
            {
                IsExecuting = true;
            }
        }

        protected override void OnBehaviourStart()
            => currentVelocity = Data.JumpVelocity;

        protected override void OnBehaviourEnd()
            => currentVelocity = 0;
    }
}