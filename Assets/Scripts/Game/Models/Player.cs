﻿using DashAttack.Game.Behaviours.Fall;
using DashAttack.Game.Behaviours.Jump;
using DashAttack.Game.Behaviours.Run;
using System;
using UnityEngine;

namespace DashAttack.Game.Models
{
    public class Player : MonoBehaviour, IRunData, IFallData, IJumpData
    {
        [SerializeField, Header("Run Data")]
        private float maxSpeed;

        [SerializeField]
        private float accelerationTime;

        [SerializeField]
        private float brakingTime;

        [SerializeField]
        private float turningTime;

        [SerializeField]
        private float airControlAmount;

        [SerializeField, Header("Jump Data")]
        private float jumpHeight;

        [SerializeField]
        private float jumpDistance;

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

        public float JumpVelocity { get; private set; }

        public float Gravity { get; private set; }

        private void OnValidate()
        {
            MaxSpeed = maxSpeed;
            AccelerationTime = accelerationTime;
            BrakingTime = brakingTime;
            TurningTime = turningTime;
            AirControlAmount = airControlAmount;

            JumpHeight = jumpHeight;
            JumpDistance = jumpDistance;

            ComputeJumpVelocity();
        }

        private void ComputeJumpVelocity()
        {
            var jumpTime = JumpDistance / MaxSpeed / 2;

            Gravity = (2 * JumpHeight) / Mathf.Pow(jumpTime, 2);
            JumpVelocity = Gravity * jumpTime;
        }
    }
}
