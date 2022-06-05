using DashAttack.Game.Behaviours.Fall;
using DashAttack.Game.Behaviours.Jump;
using DashAttack.Game.Behaviours.Run;
using DashAttack.Game.Behaviours.WallJump;
using DashAttack.Game.Behaviours.WallStick;
using UnityEngine;

namespace DashAttack.Game.Models
{
    public class Player : MonoBehaviour, IRunData, IFallData, IJumpData, IWallStickData, IWallJumpData
    {
        [Header("Run Data")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accelerationTime;
        [SerializeField] private float brakingTime;
        [SerializeField] private float turningTime;
        [SerializeField] private float airControlAmount;
        [SerializeField] private float wallStickTime;

        [Header("Jump Data")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDistance;
        [SerializeField] private float wallSlideMultiplier;
        [SerializeField] private float wallClimbMultiplier;
        [SerializeField] private float wallJumpDistance;

        // ========== RUN PROPERTIES ==========

        public float MaxSpeed
        {
            get => maxSpeed;
            private set => maxSpeed = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public float AccelerationTime
        {
            get => accelerationTime;
            private set => accelerationTime = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public float BrakingTime
        {
            get => brakingTime;
            private set => brakingTime = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public float TurningTime
        {
            get => turningTime;
            private set => turningTime = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public float AirControlAmount
        {
            get => airControlAmount;
            private set => airControlAmount = Mathf.Clamp(value, 0, 1);
        }

        public float WallStickTime
        {
            get => wallStickTime;
            set => wallStickTime = Mathf.Clamp(value, 0, int.MaxValue);
        }

        // ========== JUMP PROPERTIES ==========

        public float JumpHeight
        {
            get => jumpHeight;
            private set => jumpHeight = Mathf.Clamp(value, 1, int.MaxValue);
        }

        public float JumpDistance
        {
            get => jumpDistance;
            private set => jumpDistance = Mathf.Clamp(value, 1, int.MaxValue);
        }

        public float WallSlideMultiplier
        {
            get => wallSlideMultiplier;
            set => wallSlideMultiplier = Mathf.Clamp(value, 0, 1);
        }

        public float WallClimbMultiplier
        {
            get => wallClimbMultiplier;
            set => wallClimbMultiplier = Mathf.Clamp(value, 0, 1);
        }

        public float WallJumpDistance
        {
            get => wallJumpDistance;
            set => wallJumpDistance = value;
        }

        public float JumpVelocity { get; private set; }

        public float Gravity { get; private set; }

        public Vector2 ImpulseVelocity { get; private set; }

        public Vector2 Deceleration { get; private set; }

        private void OnValidate()
        {
            MaxSpeed = maxSpeed;
            AccelerationTime = accelerationTime;
            BrakingTime = brakingTime;
            TurningTime = turningTime;
            AirControlAmount = airControlAmount;

            JumpHeight = jumpHeight;
            JumpDistance = jumpDistance;
            WallStickTime = wallStickTime;
            WallSlideMultiplier = wallSlideMultiplier;
            WallClimbMultiplier = wallClimbMultiplier;
            WallJumpDistance = wallJumpDistance;

            ComputeJumpVelocity();
            ComputeWallJumpVelocity();
        }

        private void ComputeJumpVelocity()
        {
            var jumpTime = JumpDistance / MaxSpeed / 2;

            Gravity = 2 * JumpHeight / Mathf.Pow(jumpTime, 2);
            JumpVelocity = Gravity * jumpTime;
        }

        private void ComputeWallJumpVelocity()
        {
            var acceleration = MaxSpeed / AccelerationTime * AirControlAmount;
            var accelerationTime = MaxSpeed / acceleration;
            var distanceAccelerating = acceleration * Mathf.Pow(accelerationTime, 2) / 2;

            var timeAtMaxSpeed = (WallJumpDistance - distanceAccelerating) / MaxSpeed;
            var wallJumpTime = timeAtMaxSpeed + accelerationTime;

            Deceleration = new(2 * WallJumpDistance / Mathf.Pow(wallJumpTime, 2), Gravity);
            ImpulseVelocity = new(wallJumpTime * Deceleration.x, wallJumpTime * Gravity);
        }
    }
}