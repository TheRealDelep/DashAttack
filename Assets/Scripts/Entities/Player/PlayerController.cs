using System;

using DashAttack.Gameplay.Behaviours.Concretes;
using DashAttack.Gameplay.Behaviours.Enums;
using DashAttack.Physics;

using UnityEditor.Timeline;

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
            context.Update();
            UpdateBehaviours();
            physicsObject.Move(ComputeVelocity() * context.DeltaTime);
        }

        private void Update()
        {
            context.Update();
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
                if (current is Executing)
                {
                    wallJumpDirection = Mathf.Sign(WallJump.Velocity.x);
                    return;
                }

                Fall.TransitionTo(Rest);

                Jump.TransitionTo(Executing);
                Jump.Velocity = new Vector2(0, data.AfterWallJumpVerticalVelocity);

                Run.Velocity = new Vector2(data.MaxSpeed * wallJumpDirection, 0);
                var nextRunState = Mathf.Sign(context.RunDirection) == -wallJumpDirection
                    ? RunState.Turning
                    : RunState.AtMaxSpeed;

                Run.TransitionTo(nextRunState);
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