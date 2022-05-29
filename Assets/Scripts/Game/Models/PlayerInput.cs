using DashAttack.Game.Behaviours.Fall;
using DashAttack.Game.Behaviours.Run;
using DashAttack.Game.Managers;
using System.Linq;
using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Models
{
    public class PlayerInputs : IRunInput, IFallInput
    {
        private IPhysicsObject physicsObject;

        public float RunDirection => InputManager.Instance.Move;

        public bool CanFall => !physicsObject.CurrentCollisions.Any(h => Vector2.Dot(Vector2.up, h.normal) > 0.001f);

        public void Init(IPhysicsObject physicsObject)
        {
            this.physicsObject = physicsObject;
        }

        public void UpdateInputs()
        {
        }
    }

}
