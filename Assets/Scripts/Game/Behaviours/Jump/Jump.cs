using System.Linq;
using UnityEngine;
using static DashAttack.Game.Behaviours.Jump.JumpState;

namespace DashAttack.Game.Behaviours.Jump
{
    public class Jump : StateMachineBehaviour<IJumpData, IJumpInput, JumpState>
    {
        private float currentVelocity;

        private bool IsGrounded => physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.up);

        public override bool IsExecuting => CurrentState != Rest;

        protected override void InitStateMachine()
        {
            base.InitStateMachine();

            stateMachine.AddState(Rest, onStateUpdate: OnRestUpdate);
            stateMachine.AddState(Rising, onStateEnter: OnRisingEnter, onStateUpdate: OnRisingUpdate);
            stateMachine.AddState(Falling, onStateEnter: OnFallingEnter, onStateUpdate: OnFallingUpdate);

            stateMachine.SubscribeAfterUpdate(AfterUpdate);

            stateMachine.Start(Rest);
        }

        void OnRestUpdate()
        {
            if (input.JumpPressedThisFixedFrame && IsGrounded)
            {
                stateMachine.TransitionTo(Rising);
            }
        }

        void OnRisingEnter()
            => currentVelocity = data.JumpVelocity + data.Gravity * Time.fixedDeltaTime;

        void OnRisingUpdate()
        {
            var hasCollisionUp = physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.down);
            if (hasCollisionUp || currentVelocity <= 0 || !input.Jump)
            {
                stateMachine.TransitionTo(Falling);
            }
        }

        void OnFallingEnter()
        {
            if (currentVelocity > 0)
            {
                currentVelocity = 0;
            }
        }

        void OnFallingUpdate()
        {
            if (IsGrounded)
            {
                stateMachine.TransitionTo(Rest);
            }
        }

        void AfterUpdate()
        {
            if (CurrentState != Rest)
            {
                currentVelocity -= data.Gravity * Time.fixedDeltaTime;
                physicsObject.Move(0, currentVelocity * Time.fixedDeltaTime);
            }
        }
    }
}
