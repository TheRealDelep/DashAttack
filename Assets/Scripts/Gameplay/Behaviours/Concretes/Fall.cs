using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Fall : BehaviourBase<IFallData, IFallContext>
    {
        public Fall(IFallData data, IFallContext context)
            : base(data, context)
        {
        }

        public override void Update()
        {
            float gravity = Data.Gravity;

            if (Context.Collisions.Left || Context.Collisions.Right)
            {
                gravity *= Data.WallSlideMultiplier;
            }

            Context.VerticalVelocity -= gravity * Context.DeltaTime;
        }
    }
}