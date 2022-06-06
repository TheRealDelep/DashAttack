using System;
using UnityEngine;

namespace DashAttack
{
    [DefaultExecutionOrder(1_000_000)]
    public class PhysicsManager : MonoBehaviour
    {
        public static event Action EndOfFixedFrame;
        public static event Action StartOfFixedFrame;

        public static PhysicsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(this);
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void FixedUpdate()
        {
            EndOfFixedFrame?.Invoke();
        }
    }
}