using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using DashAttack.Utilities.Enums;
using UnityEngine;

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
            Context.HorizontalVelocity = (Context.RunInputDirection == HorizontalDirection.Right ? -1 : 1) * Data.WallJumpVelocity.x;
            Context.HorizontalVelocity += Data.WallJumpDeceleration.x * Mathf.Sign(Context.HorizontalVelocity) * Context.DeltaTime / 2;
            Context.VerticalVelocity = Data.WallJumpVelocity.y;
        }

        public override void Update()
        {
            Context.HorizontalVelocity -= Data.WallJumpDeceleration.x * Mathf.Sign(Context.HorizontalVelocity) * Context.DeltaTime;
            Context.VerticalVelocity -= Data.WallJumpDeceleration.y * Context.DeltaTime;
        }
    }
}