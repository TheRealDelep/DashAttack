using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Behaviours.WallStick
{

    public class WallStick : BaseBehaviour<IWallStickData, ICharacterInputs>
    {
        private float elapsedTimeOnWall;

        private bool wallSticked;

        public override bool IsExecuting => wallSticked;

        public override void Init(IPhysicsObject physicsObject, IWallStickData data, ICharacterInputs input)
        {
            base.Init(physicsObject, data, input);
            this.physicsObject.OnCollisionEnter += hits =>
            {
                if (hits.Any(h => h.normal == Vector2.right || h.normal == Vector2.left) &&
                    !hits.Any(h => h.normal == Vector2.up))
                {
                    wallSticked = true;
                }
            };
        }

        public override void Update()
        {
            if (wallSticked)
            {
                elapsedTimeOnWall += Time.fixedDeltaTime;
                if (elapsedTimeOnWall >= data.WallStickTime)
                {
                    wallSticked = false;
                }
            }
        }
    }

}
