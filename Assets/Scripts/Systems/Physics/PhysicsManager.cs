using System;
using UnityEngine;

namespace TheRealDelep.Physics
{
    [DefaultExecutionOrder(1_000_000)]
    public class PhysicsManager : MonoBehaviour
    {
        public static PhysicsManager Instance { get; private set; }

        public static event Action EndOfFixedFrame;

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
