using DashAttack.Gameplay.Behaviours.Enums;
using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using DashAttack.Utilities.Enums;

using UnityEngine;

using static DashAttack.Gameplay.Behaviours.Enums.RunState;
using static DashAttack.Utilities.Enums.HorizontalDirection;

namespace DashAttack.Gameplay.Behaviours.Concretes
{
    public class Run : StateBehaviourBase<IRunData, IRunContext, RunState>
    {
        private bool isTurningFrame;

        public Run(IRunData data, IRunContext context)
            : base(data, context)
        {
        }

        protected override RunState EntryState => Rest;

        private float AerialMod
            => Context.Collisions.Bottom
                ? 1
                : Data.AirControlAmount;

        protected override void InitStates()
        {
            AddState(Rest, onStateUpdate: OnRestUpdate);
            AddState(Accelerating, onStateUpdate: OnAcceleratingUpdate);
            AddState(Braking, onStateUpdate: OnBrakingUpdate);
            AddState(Turning, onStateEnter: OnTurningEnter, onStateUpdate: OnTurningUpdate);
            AddState(AtMaxSpeed, onStateUpdate: OnMaxSpeedUpdate);

            stateMachine.SubscribeBeforeUpdate(() =>
            {
                if (Context.Collisions.Left || Context.Collisions.Right)
                {
                    Context.HorizontalVelocity = 0;
                    TransitionTo(Rest);
                }
            });

            stateMachine.LogTransitions = false;
        }

        private void OnRestUpdate()
        {
            if (Context.RunInputDirection is not None)
            {
                TransitionTo(Accelerating);
            }
        }

        private void OnAcceleratingUpdate()
        {
            RunState? nextState = Context.RunInputDirection switch
            {
                None => Braking,
                _ when Context.RunInputDirection != Context.LastFixedFrameRunInputDirection
                    && Context.LastFixedFrameRunInputDirection is not None
                    && PreviousState != Turning => Turning,
                _ when Mathf.Abs(Context.HorizontalVelocity) >= Data.MaxSpeed => AtMaxSpeed,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            Context.HorizontalVelocity += Data.MaxSpeed / Data.AccelerationTime * Context.RunInputDirection.ToInt() * AerialMod * Context.DeltaTime;
        }

        private void OnBrakingUpdate()
        {
            RunState? nextState = Context.RunInputDirection switch
            {
                None when Context.HorizontalVelocity == 0 => Rest,
                not None when Context.RunInputDirection.IsEqual(Context.HorizontalVelocity) => Accelerating,
                not None when !Context.RunInputDirection.IsEqual(Context.HorizontalVelocity) => Turning,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            Context.HorizontalVelocity -= Data.MaxSpeed / Data.BrakingTime * Mathf.Sign(Context.HorizontalVelocity) * AerialMod * Context.DeltaTime;

            if (Mathf.Sign(Context.HorizontalVelocity) != Mathf.Sign(Context.LastFrameHorizontalVelocity))
            {
                Context.HorizontalVelocity = 0;
                TransitionTo(Rest);
            }
        }

        private void OnTurningEnter()
            => isTurningFrame = true;

        private void OnTurningUpdate()
        {
            if (Context.RunInputDirection is None)
            {
                TransitionTo(Braking);
                return;
            }
            else
            {
                var currentDirection = Context.HorizontalVelocity == 0
                    ? Context.LastFrameHorizontalVelocity > 0 ? Left : Right
                    : Context.HorizontalVelocity > 0 ? Right : Left;

                if (!isTurningFrame && currentDirection == Context.RunInputDirection)
                {
                    TransitionTo(Accelerating);
                    return;
                }
            }

            Context.HorizontalVelocity += Data.MaxSpeed / Data.TurningTime * Context.RunInputDirection.ToInt() * AerialMod * Context.DeltaTime;
            isTurningFrame = false;
        }

        private void OnMaxSpeedUpdate()
        {
            RunState? nextState = Context.RunInputDirection switch
            {
                None => Braking,
                _ when Context.LastFixedFrameRunInputDirection != Context.RunInputDirection => Turning,
                _ when Mathf.Abs(Context.HorizontalVelocity) < Data.MaxSpeed => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }
        }
    }
}