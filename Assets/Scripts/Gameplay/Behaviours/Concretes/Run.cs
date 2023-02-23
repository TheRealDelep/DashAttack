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
                ? Data.AirControlAmount
                : 1;

        protected override void InitStates()
        {
            AddState(Rest, onStateEnter: OnRestEnter, onStateUpdate: OnRestUpdate);
            AddState(Accelerating, onStateUpdate: OnAcceleratingUpdate);
            AddState(Braking, onStateUpdate: OnBrakingUpdate);
            AddState(Turning, onStateEnter: OnTurningEnter, onStateUpdate: OnTurningUpdate);
            AddState(AtMaxSpeed, onStateUpdate: OnMaxSpeedUpdate);
        }

        private void OnRestEnter()
        {
            Context.HorizontalVelocity = 0;
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
                _ when Context.RunInputDirection != Context.LastFrameRunInputDirection
                    && Context.LastFrameRunInputDirection is not None
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
                TransitionTo(Rest);
            }
        }

        private void OnTurningEnter()
            => isTurningFrame = true;

        private void OnTurningUpdate()
        {
            RunState? nextState = Context.RunInputDirection switch
            {
                None => Braking,
                _ when Context.RunInputDirection == Context.LastFrameRunInputDirection
                    && Context.LastFrameRunInputDirection != 0
                    && !isTurningFrame => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                TransitionTo(nextState.Value);
                return;
            }

            Context.HorizontalVelocity += Data.MaxSpeed / Data.TurningTime * Context.RunInputDirection.ToInt() * AerialMod * Context.DeltaTime;
            isTurningFrame = false;
        }

        private void OnMaxSpeedUpdate()
        {
            RunState? nextState = Context.RunInputDirection switch
            {
                None => Braking,
                _ when !Context.RunInputDirection.IsEqual(Context.HorizontalVelocity) => Turning,
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