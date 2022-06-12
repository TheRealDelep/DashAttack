﻿using System;

using DashAttack.Gameplay.Behaviours.Concretes;
using DashAttack.Physics;

using UnityEngine;

using static DashAttack.Gameplay.Behaviours.Enums.BehaviourState;

namespace DashAttack.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData data;

        private float wallJumpDirection;
        
        private PlayerContext context;
        private IPhysicsObject physicsObject;

        private Fall Fall { get; set; }
        private Jump Jump { get; set; }
        private Run Run { get; set; }
        private WallJump WallJump { get; set; }

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();
            context = new PlayerContext(physicsObject);

            Fall = new Fall(data, context);
            Run = new Run(data, context);
            Jump = new Jump(data, context);
            WallJump = new WallJump(data, context);

            SubscribeStates();
        }

        private void FixedUpdate()
        {
            UpdateBehaviours();
            physicsObject.Move(ComputeVelocity() * context.DeltaTime);
        }

        private Vector2 ComputeVelocity()
        {
            if (WallJump.CurrentState is Executing)
            {
                return WallJump.Velocity;
            }
            
            var verticalVel = Jump.CurrentState is Executing
                ? Jump.Velocity
                : Fall.Velocity;
            
            return Run.Velocity + verticalVel;
        }

        private void SubscribeStates()
        {
            Jump.OnStateChange += (_, current) =>
            {
                if (current is Rest)
                {
                    Fall.TransitionTo(Rest);
                }
            };

            WallJump.OnStateChange += (_, current) =>
            {
                if (current is Rest)
                {
                    Jump.TransitionTo(Rest);
                    Fall.TransitionTo(Rest);
                }
            };
        }

        private void UpdateBehaviours()
        {
            Fall.UpdateState();
            Jump.UpdateState();
            Run.UpdateState();
            WallJump.UpdateState();
        }
    }
}