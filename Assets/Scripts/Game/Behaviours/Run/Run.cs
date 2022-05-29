using UnityEngine;

using static DashAttack.Game.Behaviours.Run.RunState;

namespace DashAttack.Game.Behaviours.Run
{
    public class Run : StateMachineBehaviour<IRunData, IRunInput, RunState>
    {
        private float currentVelocity;
        private float lastFrameVelocity;
        private float lastFrameRunDirection;

        public override RunState CurrentState => stateMachine.CurrentState;

        private float CurrentVelocity
        {
            get => currentVelocity;
            set
            {
                lastFrameVelocity = currentVelocity;
                currentVelocity = Mathf.Clamp(value, -data.MaxSpeed, data.MaxSpeed);
            }
        }

        public override void Reset()
            => CurrentVelocity = 0;

        protected override void InitStateMachine()
        {
            stateMachine = new();

            stateMachine.AddState(Rest, onStateEnter: OnRestEnter, onStateUpdate: OnRestUpdate);
            stateMachine.AddState(Accelerating, onStateUpdate: OnAcceleratingUpdate);
            stateMachine.AddState(Braking, onStateUpdate: OnBrakingUpdate);
            stateMachine.AddState(Turning, onStateUpdate: OnTurningUpdate);
            stateMachine.AddState(AtMaxSpeed, onStateUpdate: OnMaxSpeedUpdate);

            stateMachine.Start(Rest);
        }

        private void OnRestEnter() => Reset();

        private void OnRestUpdate()
        {
            if (input.RunDirection != 0)
            {
                stateMachine.TransitionTo(Accelerating);
            }

            lastFrameRunDirection = input.RunDirection;
        }

        private void OnAcceleratingUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection) => Turning,
                _ when Mathf.Abs(CurrentVelocity) == data.MaxSpeed => AtMaxSpeed,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
                return;
            }

            CurrentVelocity += data.MaxSpeed / data.AccelerationTime * input.RunDirection * Time.fixedDeltaTime;
            physicsObject.Move(CurrentVelocity * Time.fixedDeltaTime, 0);

            lastFrameRunDirection = input.RunDirection;
        }

        private void OnBrakingUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                not 0 when Mathf.Sign(input.RunDirection) != Mathf.Sign(CurrentVelocity) => Accelerating,
                not 0 when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection) => Turning,
                _ when CurrentVelocity == 0 => Rest,
                _ when Mathf.Sign(CurrentVelocity) != Mathf.Sign(lastFrameVelocity) => Rest,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
            }

            CurrentVelocity -= data.MaxSpeed / data.BrakingTime * Mathf.Sign(CurrentVelocity) * Time.fixedDeltaTime;
            physicsObject.Move(currentVelocity * Time.fixedDeltaTime, 0);

            lastFrameRunDirection = input.RunDirection;
        }

        private void OnTurningUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection) => Accelerating,
                _ when Mathf.Sign(CurrentVelocity) != Mathf.Sign(lastFrameVelocity) => Accelerating,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
            }

            CurrentVelocity += data.MaxSpeed / data.TurningTime * input.RunDirection * Time.fixedDeltaTime;
            physicsObject.Move(currentVelocity * Time.fixedDeltaTime, 0);

            lastFrameRunDirection = input.RunDirection;
        }

        private void OnMaxSpeedUpdate()
        {
            RunState? nextState = input.RunDirection switch
            {
                0 => Braking,
                _ when Mathf.Sign(input.RunDirection) != Mathf.Sign(lastFrameRunDirection) => Turning,
                _ => null
            };

            if (nextState.HasValue)
            {
                stateMachine.TransitionTo(nextState.Value);
            }

            CurrentVelocity = data.MaxSpeed * input.RunDirection;
            physicsObject.Move(currentVelocity * Time.fixedDeltaTime, 0);

            lastFrameRunDirection = input.RunDirection;
        }
    }
}
