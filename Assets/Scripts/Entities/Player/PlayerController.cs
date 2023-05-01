using DashAttack.Assets.Scripts.Entities.Player.States;
using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player.States;
using DashAttack.Gameplay.Behaviours.Concretes;
using DashAttack.Gameplay.Behaviours.Enums;
using DashAttack.Physics;
using DashAttack.Utilities.Enums;
using DashAttack.Utilities.StateMachine;
using System;
using UnityEngine;

using static DashAttack.Entities.Player.PlayerStateEnum;
using static DashAttack.Utilities.StateMachine.StateEvent;

namespace DashAttack.Entities.Player
{
    public class PlayerController : MonoBehaviour, IStateMachine<PlayerStateEnum>
    {
        [SerializeField] private PlayerData data;
        [SerializeField] private bool logStateTransitions;

        private IPhysicsObject physicsObject;
        private PlayerContext context;
        private StateMachine<PlayerStateEnum> stateMachine;

        private Run Run { get; set; }

        private float DownwardForce => stateMachine.CurrentState switch
        {
            WallSliding => data.Gravity * data.WallSlideMultiplier,
            WallClimbing => data.Gravity * data.WallClimbMultiplier,
            _ => data.Gravity
        };

        public void Subscribe(PlayerStateEnum state, StateEvent stateEvent, Action callBack)
        {
            stateMachine.Subscribe(state, stateEvent, callBack);
        }

        public void SubscribeRun(RunState state, StateEvent stateEvent, Action callBack)
        {
            Run.Subscribe(state, stateEvent, callBack);
        }

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();
            context = new PlayerContext(physicsObject);
            stateMachine = new();
            stateMachine.LogTransitions = logStateTransitions;

            Run = new(data, context);

            stateMachine.AddState(new PlayerIdleState(data, context, this));
            stateMachine.AddState(new PlayerFallState(data, context, this));
            stateMachine.AddState(new PlayerWallSlidingState(data, context, this));

            stateMachine.AddState(new PlayerJumpingState(data, context, this));
            stateMachine.AddState(new PlayerWallClimbingState(data, context, this));

            stateMachine.AddState(new PlayerRunningState(data, context, this));
            stateMachine.AddState(new PlayerRunningJumpingState(data, context, this));
            stateMachine.AddState(new PlayerRunningFallingState(data, context, this));

            stateMachine.AddState(new PlayerWallJumpingState(data, context, this));

            stateMachine.Start(Idle);

            SubscribeStates();
        }

        private void FixedUpdate()
        {
            if (context.Collisions.Bottom || context.Collisions.Top)
            {
                context.VerticalVelocity = 0;
            }

            stateMachine.RunMachine();

            if (stateMachine.CurrentState is Running or RunningFalling or RunningJumping)
            {
                Run.Update();
            }

            if (stateMachine.CurrentState is WallJumping)
            {
                context.HorizontalVelocity -= data.WallJumpDeceleration.x * Mathf.Sign(context.HorizontalVelocity) * context.DeltaTime;
                context.VerticalVelocity -= data.WallJumpDeceleration.y * context.DeltaTime;
            }
            else
            {
                context.VerticalVelocity -= DownwardForce * context.DeltaTime;
            }

            physicsObject.Move(context.HorizontalVelocity * context.DeltaTime, context.VerticalVelocity * context.DeltaTime);
        }

        private void SubscribeStates()
        {
            Subscribe(Jumping, OnEnter, Jump);
            Subscribe(RunningJumping, OnEnter, Jump);

            Subscribe(Falling, OnEnter, CutRemainingVelocity);
            Subscribe(RunningFalling, OnEnter, CutRemainingVelocity);

            Subscribe(WallJumping, OnEnter, () =>
            {
                context.HorizontalVelocity = (context.RunInputDirection == HorizontalDirection.Right ? -1 : 1) * data.WallJumpVelocity.x;
                context.HorizontalVelocity += data.WallJumpDeceleration.x * Mathf.Sign(context.HorizontalVelocity) * context.DeltaTime;
                context.VerticalVelocity = data.WallJumpVelocity.y + (DownwardForce * context.DeltaTime);
            });

            void Jump()
            {
                if (stateMachine.PreviousState is not (Jumping or RunningJumping or WallJumping))
                {
                    context.VerticalVelocity += data.JumpVelocity + (DownwardForce * context.DeltaTime);
                }
            }

            void CutRemainingVelocity()
            {
                context.VerticalVelocity = Mathf.Clamp(context.VerticalVelocity, context.VerticalVelocity, 0);
            }
        }

        private void Update()
        {
            context.Update();
        }

        public void TransitionTo(PlayerStateEnum nextState)
            => stateMachine.TransitionTo(nextState);
    }
}
