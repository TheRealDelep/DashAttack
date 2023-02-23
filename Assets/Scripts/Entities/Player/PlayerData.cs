﻿using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

using UnityEngine;

namespace DashAttack.Entities.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Player")]
    public class PlayerData : ScriptableObject, IRunData, IFallData, IJumpData, IWallJumpData
    {
        [Header("Run Data")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accelerationTime;
        [SerializeField] private float brakingTime;
        [SerializeField] private float turningTime;
        [SerializeField] private float airControlAmount;

        [Header("Jump Data")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDistance;
        [SerializeField] private float wallSlideMultiplier;
        [SerializeField] private float wallClimbMultiplier;
        [SerializeField] private float wallJumpDistance;
        [SerializeField] private float wallJumpTime;
        [SerializeField] private float earlyJumpBuffer;
        [SerializeField] private float lateJumpBuffer;

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

        // ========== JUMP PROPERTIES ==========

        public float WallSlideMultiplier
        {
            get => wallSlideMultiplier;
            private set => wallSlideMultiplier = Mathf.Clamp(value, 0, 1);
        }

        public float WallClimbMultiplier
        {
            get => wallClimbMultiplier;
            private set => wallClimbMultiplier = Mathf.Clamp(value, 0, 1);
        }

        public float EarlyJumpBuffer
        {
            get => earlyJumpBuffer;
            set => earlyJumpBuffer = Mathf.Clamp(value, 0, Mathf.Infinity);
        }

        public float LateJumpBuffer
        {
            get => lateJumpBuffer;
            set => lateJumpBuffer = Mathf.Clamp(value, 0, Mathf.Infinity);
        }

        public float JumpVelocity { get; private set; }

        public float Gravity { get; private set; }

        public Vector2 WallJumpVelocity { get; private set; }

        public Vector2 WallJumpDeceleration { get; private set; }

        public float AfterWallJumpVerticalVelocity { get; private set; }

        private float JumpHeight
        {
            get => jumpHeight;
            set => jumpHeight = Mathf.Clamp(value, 1, int.MaxValue);
        }

        private float JumpDistance
        {
            get => jumpDistance;
            set => jumpDistance = Mathf.Clamp(value, 1, int.MaxValue);
        }

        private float WallJumpDistance
        {
            get => wallJumpDistance;
            set => wallJumpDistance = value;
        }

        private float WallJumpTime
        {
            get => wallJumpTime;
            set => wallJumpTime = Mathf.Clamp(value, WallJumpDistance / (MaxSpeed * 5), WallJumpDistance / MaxSpeed);
        }

        private void OnValidate()
        {
            MaxSpeed = maxSpeed;
            AccelerationTime = accelerationTime;
            BrakingTime = brakingTime;
            TurningTime = turningTime;
            AirControlAmount = airControlAmount;

            JumpHeight = jumpHeight;
            JumpDistance = jumpDistance;
            WallSlideMultiplier = wallSlideMultiplier;
            WallClimbMultiplier = wallClimbMultiplier;
            WallJumpDistance = wallJumpDistance;
            WallJumpTime = wallJumpTime;
            EarlyJumpBuffer = earlyJumpBuffer;
            LateJumpBuffer = lateJumpBuffer;

            ComputeJumpVelocity();
            ComputeWallJumpVelocity();
        }

        private void ComputeJumpVelocity()
        {
            float jumpTime = JumpDistance / MaxSpeed / 2;

            Gravity = 2 * JumpHeight / Mathf.Pow(jumpTime, 2);
            JumpVelocity = Gravity * jumpTime;
        }

        private void ComputeWallJumpVelocity()
        {
            float turningForce = MaxSpeed / TurningTime * AirControlAmount;
            float timeTurning = MaxSpeed / turningForce;
            float distanceTurning = turningForce * Mathf.Pow(timeTurning, 2) / 2;

            float travelBackTime = timeTurning + ((WallJumpDistance - distanceTurning) / MaxSpeed);
            float wallJumpHeight = (Gravity * Mathf.Pow(travelBackTime, 2)) / 2;

            var wallJumpingTime = Mathf.Sqrt((2 * WallJumpDistance) / turningForce);

            WallJumpDeceleration = new Vector2(
                0,
                2 * wallJumpHeight / Mathf.Pow(WallJumpTime, 2));

            WallJumpVelocity = new Vector2(turningForce, WallJumpDeceleration.y) * wallJumpingTime;
        }
    }
}