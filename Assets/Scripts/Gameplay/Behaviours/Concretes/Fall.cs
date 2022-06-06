using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using System.Linq;
using UnityEngine;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Fall : BaseBehaviour<IFallData, IFallContext>
    {
        private float currentVelocity;

        public override bool IsExecuting => true;

        public override void Update()
        {
            if (physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.up))
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