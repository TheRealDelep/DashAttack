﻿using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Managers;
using DashAttack.Physics;
using UnityEngine;

namespace DashAttack.Entities.Player
{
    public class PlayerContext : IRunContext, IFallContext, IJumpContext, IWallJumpContext
    {
        private readonly IPhysicsObject physicsObject;

        public PlayerContext(IPhysicsObject physicsObject)
        {
            this.physicsObject = physicsObject;
        }

        public float RunDirection => InputManager.Instance.Move;

        public bool JumpInput => InputManager.Instance.Jump;

        public bool JumpInputDown => InputManager.Instance.JumpPressedThisFixedFrame;

        public float DeltaTime => Time.fixedDeltaTime;

        public CollisionInfos Collisions => physicsObject.CollisionInfos;
    }
}