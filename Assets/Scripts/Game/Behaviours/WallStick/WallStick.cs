﻿using System.Linq;
using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Behaviours.WallStick
{

    public class WallStick : BaseBehaviour<IWallStickData, ICharacterInputs>
    {
        private float elapsedTimeOnWall;

        private bool wallSticked;

        public override bool IsExecuting => wallSticked;
        private float wallDirection;

        public override void Init(IPhysicsObject physicsObject, IWallStickData data, ICharacterInputs input)
        {
            base.Init(physicsObject, data, input);
            this.physicsObject.OnCollisionEnter += hits =>
            {
                var collisionOnSide = hits.FirstOrDefault(h
                    => h.normal == Vector2.right || h.normal == Vector2.left);

                if (collisionOnSide && !physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.up))
                {
                    wallSticked = true;
                    wallDirection = -collisionOnSide.normal.x;
                }
            };

            this.physicsObject.OnCollisionExit += _ =>
            {
                var collisionOnSide = physicsObject.CurrentCollisions.FirstOrDefault(h
                    => h.normal == Vector2.right || h.normal == Vector2.left);

                if (collisionOnSide && physicsObject.CurrentCollisions.All(h => h.normal != Vector2.up))
                {
                    wallSticked = true;
                    wallDirection = -collisionOnSide.normal.x;
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
                    elapsedTimeOnWall = 0;
                    return;
                }

                physicsObject.Move(wallDirection, 0);
            }
        }
    }
}
