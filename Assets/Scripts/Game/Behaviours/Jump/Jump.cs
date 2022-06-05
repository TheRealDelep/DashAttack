using System.Linq;
using UnityEngine;
using static DashAttack.Game.Behaviours.Jump.JumpState;

namespace DashAttack.Game.Behaviours.Jump
{
    public class Jump : StateMachineBehaviour<IJumpData, IJumpInput, JumpState>
    {
        private float currentVelocity;

        private bool IsGrounded
            => physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.up);

        public override bool IsExecuting => CurrentState != Rest;

        private bool hasCollisionOnSide
            => physicsObject.CurrentCollisions.Any(h
                => h.normal == Vector2.left
                || h.normal == Vector2.right);

        public override void Reset() => stateMachine.TransitionTo(Rest);

        private float WallMultiplier
        {
            get
            {
                if (data.WallSlideMultiplier == 1 && data.WallClimbMultiplier == 1 || !hasCollisionOnSide)
                {
                    return 1;
                }

                return CurrentState switch
                {
                    Rising => data.WallClimbMultiplier,
                    Falling => data.WallSlideMultiplier,
                    _ => 1
                };
            }
        }

        protected override void InitStateMachine()
        {
            base.InitStateMachine();

            stateMachine.AddState(Rest, onStateEnter: OnRestEnter, onStateUpdate: OnRestUpdate);
            stateMachine.AddState(Rising, onStateEnter: OnRisingEnter, onStateUpdate: OnRisingUpdate);
            stateMachine.AddState(Falling, onStateEnter: OnFallingEnter, onStateUpdate: OnFallingUpdate);

            stateMachine.SubscribeAfterUpdate(AfterUpdate);

            stateMachine.Start(Rest);
        }

        private void OnRestEnter()
            => currentVelocity = 0;

        private void OnRestUpdate()
        {
            if (input.JumpPressedThisFixedFrame && IsGrounded)
            {
                stateMachine.TransitionTo(Rising);
            }
        }

        private void OnRisingEnter()
            => currentVelocity = data.JumpVelocity + data.Gravity * Time.fixedDeltaTime;

        private void OnRisingUpdate()
        {
            currentVelocity -= data.Gravity * WallMultiplier * Time.fixedDeltaTime;
            var hasCollisionUp = physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.down);
            if (hasCollisionUp || currentVelocity <= 0 || !input.Jump)
            {
                stateMachine.TransitionTo(Falling);
            }
        }

        private void OnFallingEnter()
        {
            currentVelocity -= data.Gravity * WallMultiplier * Time.fixedDeltaTime;
        }

        private void OnFallingUpdate()
        {
            if (IsGrounded)
            {
                stateMachine.TransitionTo(Rest);
            }

            currentVelocity -= data.Gravity * WallMultiplier * Time.fixedDeltaTime;
        }

        private void AfterUpdate()
        {
            if (CurrentState == Rest)
            {
                return;
            }

            physicsObject.Move(0, currentVelocity * Time.fixedDeltaTime);
        }
    }
}
