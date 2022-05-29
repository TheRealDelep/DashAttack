using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Behaviours.Fall
{
    public class Fall : IBehaviour<IFallData, IFallInput>
    {
        private float currentVelocity;

        private IPhysicsObject physicsObject;
        private IFallData data;
        private IFallInput input;

        public void Execute()
        {
            if (!input.CanFall)
            {
                currentVelocity = 0;
            }

            currentVelocity -= Mathf.Abs(data.Gravity) * Time.fixedDeltaTime * Time.fixedDeltaTime;
            physicsObject.Move(0, currentVelocity);
        }

        public void Init(IPhysicsObject physicsObject, IFallData data, IFallInput input)
        {
            this.physicsObject = physicsObject;
            this.data = data;
            this.input = input;
        }

        public void Reset()
            => currentVelocity = 0;
    }
}
