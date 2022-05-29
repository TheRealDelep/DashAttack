using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Behaviours.Run
{
    public class Run : MonoBehaviour, IBehaviour<IRunData, IRunInput>
    {
        void IBehaviour<IRunData, IRunInput>.Run(IPhysicsObject physicsObject, IRunData data, IRunInput input)
        {
            physicsObject.Move(data.MaxSpeed * Time.fixedDeltaTime * input.RunDirection, 0);
        }

        public void Reset()
        {
        }
    }
}
