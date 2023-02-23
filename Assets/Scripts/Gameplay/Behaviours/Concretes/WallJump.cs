using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using DashAttack.Utilities.Enums;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class WallJump : BehaviourBase<IWallJumpData, IWallJumpContext>
    {
        public WallJump(IWallJumpData data, IWallJumpContext context)
            : base(data, context)
        {
        }

        public override void Start()
        {
            base.Start();
            Context.HorizontalVelocity = Context.RunInputDirection == HorizontalDirection.Right ? -1 : 1 * Data.WallJumpVelocity.x;
            Context.VerticalVelocity = Data.WallJumpVelocity.y;
        }
    }
}