using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Behaviours.Fall
{
    public class Fall : IBehaviour<IFallData, IFallInput>
    {
        private float currentVelocity;

        public void Run(IPhysicsObject physicsObject, IFallData data, IFallInput input)
        {
            if (!input.CanFall)
            {
                currentVelocity = 0;
            }

            currentVelocity -= Mathf.Abs(data.Gravity) * Time.fixedDeltaTime * Time.fixedDeltaTime;
            physicsObject.Move(0, currentVelocity);
        }

        public void Reset()
            => currentVelocity = 0;
    }
}
