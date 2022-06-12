using DashAttack.Gameplay.Behaviours.Enums;
using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

using UnityEngine;

using static DashAttack.Gameplay.Behaviours.Enums.RunState;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Run : StateMovementBehaviourBase<IRunData, IRunContext, RunState>
    {
        private float currentVelocity;
        private float lastFrameVelocity;
        private float lastFrameRunDirection;

        private bool isTurningFrame;

        private Vector2 velocity;

        public override Vector2 Velocity
        {
            get => new(CurrentVelocity, 0);
            set => CurrentVelocity = value.x;
        }
        
        private float CurrentVelocity
        {
            get => currentVelocity;
            set
            {
                lastFrameVelocity = currentVelocity;
                currentVelocity = Mathf.Clamp(value, -Data.MaxSpeed, Data.MaxSpeed);
            }
        }

        private float AerialMod
            => Context.Collisions.Bottom
                ? Data.AirControlAmount
                : 1;

        public Run(IRunData data, IRunContext context)
            : base(data, context)
        {
            AddState(Rest, onStateEnter: OnRestEnter, onStateUpdate: OnRestUpdate);
            AddState(Accelerating, onStateUpdate: OnAcceleratingUpdate);
            AddState(Braking, onStateUpdate: OnBrakingUpdate);
            AddState(Turning, onStateEnter: OnTurningEnter, onStateUpdate: OnTurningUpdate);
            AddState(AtMaxSpeed, onStateUpdate: OnMaxSpeedUpdate);

            SubscribeBeforeUpdate(BeforeStateUpdate);
            SubscribeAfterUpdate(AfterStateUpdate);

            Start(Rest);

            OnStateChange += (previous, next) => Debug.Log($"Transition From {previous} to {next} @ {Time.time}, CurrentVelocity:{CurrentVelocity}");
        }

        private void BeforeStateUpdate()
        {
            if (Context.Collisions.Left || Context.Collisions.Right)
            {
                CurrentVelocity = 0;
            }
        }

        private void AfterStateUpdate()
        {
            lastFrameRunDirection = Context.RunDirection;
        }

        private void OnRestEnter()
            => CurrentVelocity = 0;

        private void OnRestUpdate()
        {
            if (Context.RunDirection != 0)
            {
                TransitionTo(Accelerating);
            }
        }

        private void OnAcceleratingUpdate()
        {
            RunState? nextState = Context.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(Context.RunDirection) != Mathf.Sign(lastFrameRunDirection)
                    && lastFrameRunDirection != 0
                    && PreviousState != Turning => Turning,
                _ when Mathf.Abs(CurrentVelocity) >= Data.MaxSpeed => AtMaxSpeed,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity += Data.MaxSpeed / Data.AccelerationTime * Context.RunDirection * AerialMod *
                Context.DeltaTime;
        }

        private void OnBrakingUpdate()
        {
            RunState? nextState = Context.RunDirection switch
            {
                not 0 when Mathf.Sign(Context.RunDirection) == Mathf.Sign(CurrentVelocity) => Accelerating,
                not 0 when Mathf.Sign(Context.RunDirection) != Mathf.Sign(CurrentVelocity) => Turning,
                _ when CurrentVelocity == 0 => Rest,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity -= Data.MaxSpeed / Data.BrakingTime * Mathf.Sign(CurrentVelocity) * AerialMod * Context.DeltaTime;

            if (Mathf.Sign(CurrentVelocity) != Mathf.Sign(lastFrameVelocity))
            {
                TransitionTo(Rest);
            }
        }

        private void OnTurningEnter()
            => isTurningFrame = true;

        private void OnTurningUpdate()
        {
            RunState? nextState = Context.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(Context.RunDirection) != Mathf.Sign(lastFrameRunDirection)
                    && lastFrameRunDirection != 0
                    && !isTurningFrame => Accelerating,
                _ when Mathf.Sign(CurrentVelocity) != Mathf.Sign(lastFrameVelocity) && !isTurningFrame => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity += Data.MaxSpeed / Data.TurningTime * Context.RunDirection * AerialMod * Context.DeltaTime;
            isTurningFrame = false;
        }

        private void OnMaxSpeedUpdate()
        {
            RunState? nextState = Context.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(Context.RunDirection) != Mathf.Sign(lastFrameRunDirection) => Turning,
                _ when Mathf.Abs(CurrentVelocity) < Data.MaxSpeed => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity = Data.MaxSpeed * Context.RunDirection;
        }
    }
}