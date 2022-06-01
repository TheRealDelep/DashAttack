using UnityEngine;

namespace DashAttack.Game.Behaviours.Fall
{
    public class Fall : BaseBehaviour<IFallData, IFallInput>
    {
        private float currentVelocity;

        public override void Execute()
        {
            if (!input.CanFall)
            {
                currentVelocity = 0;
            }

            currentVelocity -= Mathf.Abs(data.Gravity) * Time.fixedDeltaTime * Time.fixedDeltaTime;
            physicsObject.Move(0, currentVelocity);
        }

        public override void Reset()
            => currentVelocity = 0;
    }
}
