﻿using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Managers;
using DashAttack.Physics;
using DashAttack.Utilities.Enums;

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

        public float HorizontalVelocity { get; set; }

        public float LastFrameHorizontalVelocity { get; private set; }

        public HorizontalDirection RunInputDirection => InputManager.Instance.Move.ToHorizontalDirection();

        public HorizontalDirection LastFixedFrameRunInputDirection => InputManager.Instance.LastFixedFrameMove.ToHorizontalDirection();

        public float TimeSinceRunInput { get; private set; }

        public float VerticalVelocity { get; set; }

        public bool JumpInput => InputManager.Instance.Jump;

        public bool JumpInputDown => InputManager.Instance.JumpPressedThisFixedFrame;

        public bool LastFixedFrameJumpInput { get; private set; }

        public float DeltaTime => Time.fixedDeltaTime;

        public CollisionInfos Collisions => physicsObject.CollisionInfos;

        public bool RunningIntoWall =>
            (Collisions.Right && RunInputDirection is HorizontalDirection.Right) ||
            (Collisions.Left && RunInputDirection is HorizontalDirection.Left);

        public bool EndOfJump =>
            !JumpInputDown && (
                Collisions.Top ||
                VerticalVelocity < Mathf.Epsilon ||
                !JumpInput);

        public bool ExitingWall => !(Collisions.Right || Collisions.Left) || RunInputDirection == HorizontalDirection.None;

        public bool HasSideCollision => HorizontalVelocity.ToHorizontalDirection() == HorizontalDirection.Right
                ? Collisions.Right
                : Collisions.Left;

        public float TimeSinceJumpInputDown { get; private set; }

        public float TimeSinceCollisionBelow { get; private set; }

        public float TimeSinceCollisionOnSide { get; private set; }

        public void Update()
        {
            TimeSinceJumpInputDown += DeltaTime;
            TimeSinceRunInput += DeltaTime;

            TimeSinceCollisionBelow += DeltaTime;
            TimeSinceCollisionOnSide += DeltaTime;

            LastFrameHorizontalVelocity = HorizontalVelocity;

            if (JumpInputDown)
            {
                TimeSinceJumpInputDown = 0;
            }

            if (RunInputDirection is not HorizontalDirection.None)
            {
                TimeSinceRunInput = 0;
            }

            if (Collisions.Bottom)
            {
                TimeSinceCollisionBelow = 0;
            }

            if (Collisions.Left || Collisions.Right)
            {
                TimeSinceCollisionOnSide = 0;
            }
        }
    }
}
