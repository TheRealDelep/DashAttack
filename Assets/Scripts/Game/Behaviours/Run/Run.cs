using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Behaviours.Run
{
    public class Run : MonoBehaviour, IBehaviour<IRunData, IRunInput>
    {
        private float currentVelocity;

        void IBehaviour<IRunData, IRunInput>.Run(IPhysicsObject physicsObject, IRunData data, IRunInput input)
        {

        }

        public void Reset()
        {
        }
    }

    public enum RunState
    {
        Rest,
        Accelerating,
        Braking,
        Turning,
        AtMaxSpeed
    }
}
