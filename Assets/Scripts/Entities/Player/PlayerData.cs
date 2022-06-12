using DashAttack.Gameplay.Behaviours.Interfaces.Datas;

using UnityEngine;

namespace DashAttack.Entities.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Player")]
    public class PlayerData : ScriptableObject, IRunData, IFallData, IJumpData, IWallStickData, IWallJumpData
    {
        [Header("Run Data")][SerializeField] private float maxSpeed;
        [SerializeField] private float accelerationTime;
        [SerializeField] private float brakingTime;
        [SerializeField] private float turningTime;
        [SerializeField] private float airControlAmount;
        [SerializeField] private float wallStickTime;

        [Header("Jump Data")][SerializeField] private float jumpHeight;
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
            private set => wallStickTime = Mathf.Clamp(value, 0, int.MaxValue);
        }

        // ========== JUMP PROPERTIES ==========

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

        private float WallJumpDistance
        {
            get => wallJumpDistance;
            set => wallJumpDistance = value;
        }

        public float JumpVelocity { get; private set; }

        public float Gravity { get; private set; }

        public Vector2 WallJumpVelocity { get; private set; }

        public Vector2 WallJumpDeceleration { get; private set; }

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
            float jumpTime = JumpDistance / MaxSpeed / 2;

            Gravity = 2 * JumpHeight / Mathf.Pow(jumpTime, 2);
            JumpVelocity = Gravity * jumpTime;
        }

        private void ComputeWallJumpVelocity()
        {
            float turningForce = MaxSpeed / TurningTime * AirControlAmount;
            float timeTurning = MaxSpeed / turningForce;
            float distanceTurning = turningForce * Mathf.Pow(timeTurning, 2) / 2;

            float wallJumpingDistance = WallJumpDistance - distanceTurning;
            float horizontalVelocity = MaxSpeed * 1.5f;
            float wallJumpingTime = wallJumpingDistance / ((MaxSpeed + horizontalVelocity) / 2);

            float horizontalDeceleration = (horizontalVelocity - MaxSpeed) / wallJumpingTime;
            
            WallJumpVelocity = new Vector2(horizontalVelocity, 0);
            WallJumpDeceleration = new Vector2(horizontalDeceleration, 0);
        }
    }
}