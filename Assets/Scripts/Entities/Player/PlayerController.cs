using DashAttack.Assets.Scripts.Entities.Player.States;
using DashAttack.Assets.Scripts.Gameplay.Behaviours.Interfaces;
using DashAttack.Assets.Scripts.Utilities.StateMachine;
using DashAttack.Entities.Player.States;
using DashAttack.Gameplay.Behaviours.Concretes;
using DashAttack.Physics;
using DashAttack.Utilities.StateMachine;
using System;
using UnityEngine;

namespace DashAttack.Entities.Player
{
    public class PlayerController : MonoBehaviour, IStateMachine<PlayerStateEnum>
    {
        [SerializeField] private PlayerData data;
        [SerializeField] private bool logStateTransitions;

        private IPhysicsObject physicsObject;
        private PlayerContext context;
        private StateMachine<PlayerStateEnum> stateMachine;

        private Fall Fall { get; set; }

        private Jump Jump { get; set; }

        private Run Run { get; set; }

        private WallJump WallJump { get; set; }

        public void Subscribe(PlayerStateEnum state, StateEvent stateEvent, Action callBack)
        {
            stateMachine.Subscribe(state, stateEvent, callBack);
        }

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();
            context = new PlayerContext(physicsObject);
            stateMachine = new();
            stateMachine.LogTransition = logStateTransitions;

            Fall = new(data, context);
            Jump = new(data, context);
            Run = new(data, context);
            WallJump = new(data, context);

            AddState(new PlayerIdleState(data, context, this), Fall);
            AddState(new PlayerFallState(data, context, this), Fall);
            AddState(new PlayerWallSlidingState(data, context, this), Fall);

            AddState(new PlayerJumpingState(data, context, this), Jump);
            AddState(new PlayerWallClimbingState(data, context, this), Jump);

            // Constantly apply downward force to trigger ground detection
            AddState(new PlayerRunningState(data, context, this), Fall, Run);
            AddState(new PlayerRunningJumpingState(data, context, this), Jump, Run);
            AddState(new PlayerRunningFallingState(data, context, this), Fall, Run);

            AddState(new PlayerWallJumpingState(data, context, this), WallJump);

            stateMachine.Start(PlayerStateEnum.Idle);
        }

        private void FixedUpdate()
        {
            if (context.Collisions.Bottom || context.Collisions.Top)
            {
                context.VerticalVelocity = 0;
            }

            stateMachine.RunMachine();
            physicsObject.Move(context.HorizontalVelocity * context.DeltaTime, context.VerticalVelocity * context.DeltaTime);
        }

        private void Update()
        {
            context.Update();
        }

        private void AddState(PlayerState state, params IBehaviour[] behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                state.StateEntered += behaviour.Start;
                state.StateUpdated += behaviour.Update;
                state.StateLeft += behaviour.Stop;
            }

            stateMachine.AddState(state);
        }

        public void TransitionTo(PlayerStateEnum nextState)
            => stateMachine.TransitionTo(nextState);
    }
}