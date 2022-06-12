using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Fall : MovementBehaviourBase<IFallData, IFallContext>
    {
        private float currentVelocity;

        public override Vector2 Velocity => new(0, currentVelocity);

        private float WallMultiplier
            => Context.Collisions.Left || Context.Collisions.Right
                ? Data.WallSlideMultiplier
                : 1;

        public Fall(IFallData data, IFallContext input)
            : base(data, input)
        {
        }

        public override void UpdateState()
        {
            IsExecuting = true;
            
            if (Context.Collisions.Bottom)
            {
                currentVelocity = 0;
            }

            currentVelocity -= Mathf.Abs(Data.Gravity) * WallMultiplier * Context.DeltaTime;
        }

        protected override void OnBehaviourStart()
        {
            // Nothing to do here
        }

        protected override void OnBehaviourEnd()
            => currentVelocity = 0;
    }
}