using DashAttack.Gameplay.Behaviours.Enums;
using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using System.Linq;
using UnityEngine;

using static DashAttack.Gameplay.Behaviours.Enums.RunState;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Run : StateMachineBehaviour<IRunData, IRunContext, RunState>
    {
        private float currentVelocity;
        private float lastFrameVelocity;
        private float lastFrameRunDirection;

        private bool isTurningFrame;

        public override RunState CurrentState => stateMachine.CurrentState;

        public override bool IsExecuting => CurrentState != Rest;

        private float CurrentVelocity
        {
            get => currentVelocity;
            set
            {
                lastFrameVelocity = currentVelocity;
                currentVelocity = Mathf.Clamp(value, -data.MaxSpeed, data.MaxSpeed);
            }
        }

        private float AerialMod
            => physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.up)
             ? data.AirControlAmount
             : 1;

        public override void Reset()
            => CurrentVelocity = 0;

        protected override void InitStateMachine()
        {
            base.InitStateMachine();

            stateMachine.AddState(Rest, onStateEnter: OnRestEnter, onStateUpdate: OnRestUpdate);
            stateMachine.AddState(Accelerating, onStateUpdate: OnAcceleratingUpdate);
            stateMachine.AddState(Braking, onStateUpdate: OnBrakingUpdate);
            stateMachine.AddState(Turning, onStateEnter: OnTurningEnter, onStateUpdate: OnTurningUpdate);
            stateMachine.AddState(AtMaxSpeed, onStateUpdate: OnMaxSpeedUpdate);

            stateMachine.SubscribeBeforeUpdate(BeforeStateUpdate);
            stateMachine.SubscribeAfterUpdate(AfterStateUpdate);

            stateMachine.Start(Rest);
        }

        private void BeforeStateUpdate()
        {
            if (physicsObject.CurrentCollisions.Any(h => h.normal == Vector2.left || h.normal == Vector2.right))
            {
                CurrentVelocity = 0;
            }
        }

        private void AfterStateUpdate()
        {
            if (CurrentVelocity != 0)
            {
                physicsObject.Move(CurrentVelocity * Time.fixedDeltaTime, 0);
            }

            lastFrameRunDirection = input.RunDirection;
        }

        private void OnRestEnter() => Reset();

        private void OnRestUpdate()
        {
            if (input.RunDirection != 0)
            {
                stateMachine.TransitionTo(Accelerating);
            }
        }

        private void OnAcceleratingUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection)
                    && lastFrameRunDirection != 0
                    && stateMachine.PreviousState != Turning => Turning,
                _ when Mathf.Abs(CurrentVelocity) == data.MaxSpeed => AtMaxSpeed,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity += data.MaxSpeed / data.AccelerationTime * input.RunDirection * AerialMod * Time.fixedDeltaTime;
        }

        private void OnBrakingUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                not 0 when Mathf.Sign(input.RunDirection) == Mathf.Sign(CurrentVelocity) => Accelerating,
                not 0 when Mathf.Sign(input.RunDirection) != Mathf.Sign(CurrentVelocity) => Turning,
                _ when CurrentVelocity == 0 => Rest,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity -= data.MaxSpeed / data.BrakingTime * Mathf.Sign(CurrentVelocity) * AerialMod * Time.fixedDeltaTime;

            if (Mathf.Sign(CurrentVelocity) != Mathf.Sign(lastFrameVelocity))
            {
                stateMachine.TransitionTo(Rest);
            }
        }

        private void OnTurningEnter()
            => isTurningFrame = true;

        private void OnTurningUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection)
                    && lastFrameRunDirection != 0
                    && !isTurningFrame => Accelerating,
                _ when Mathf.Sign(CurrentVelocity) != Mathf.Sign(lastFrameVelocity) => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity += data.MaxSpeed / data.TurningTime * input.RunDirection * AerialMod * Time.fixedDeltaTime;
            isTurningFrame = false;
        }

        private void OnMaxSpeedUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection) => Turning,
                _ when Mathf.Abs(CurrentVelocity) != data.MaxSpeed => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity = data.MaxSpeed * input.RunDirection;
        }
    }
}