using DashAttack.Game.Behaviours.Fall;
using DashAttack.Game.Behaviours.Run;
using UnityEngine;

namespace DashAttack.Game.Models
{
    public class Player : MonoBehaviour, IRunData, IFallData
    {
        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        private float accelerationTime;

        [SerializeField]
        private float brakingTime;

        [SerializeField]
        private float turningTime;

        [SerializeField]
        private float airControlAmount;

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

        public float Gravity => Physics2D.gravity.y;

        private void OnValidate()
        {
            MaxSpeed = maxSpeed;
        }
    }
}
