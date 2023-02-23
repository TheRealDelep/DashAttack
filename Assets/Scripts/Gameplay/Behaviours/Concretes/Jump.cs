using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Jump : BehaviourBase<IJumpData, IJumpContext>
    {
        private bool isFirstFrame;

        private float WallMultiplier
            => Context.Collisions.Left && Context.Collisions.Right
                ? Data.WallClimbMultiplier
                : 1;

        public Jump(IJumpData data, IJumpContext context)
            : base(data, context)
        {
        }

        public override void Update()
        {
            if (isFirstFrame)
            {
                isFirstFrame = false;
                return;
            }

            Context.VerticalVelocity -= Data.Gravity * WallMultiplier * Context.DeltaTime;
        }

        public override void Start()
        {
            if (Context.VerticalVelocity > 0)
            {
                return;
            }

            isFirstFrame = true;
            Context.VerticalVelocity += Data.JumpVelocity;
        }
    }
}
