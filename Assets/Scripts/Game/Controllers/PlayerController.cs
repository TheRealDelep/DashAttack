using DashAttack.Game.Behaviours;
using DashAttack.Game.Behaviours.Fall;
using DashAttack.Game.Behaviours.Jump;
using DashAttack.Game.Behaviours.Run;
using DashAttack.Game.Models;
using TheRealDelep.Physics.Interfaces;
using UnityEngine;

namespace DashAttack.Game.Controllers
{

    public class PlayerController : MonoBehaviour
    {
        private IPhysicsObject physicsObject;
        private Player player;
        private PlayerInputs inputs;

        private IBehaviour<IFallData, IFallInput> fall;
        private IStateMachineBehaviour<IRunData, IRunInput, RunState> run;
        private IStateMachineBehaviour<IJumpData, IJumpInput, JumpState> jump;

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();

            player = GetComponent<Player>();
            inputs = new PlayerInputs();
            inputs.Init(physicsObject);

            fall = new Fall();
            run = new Run();
            jump = new Jump();

            fall.Init(physicsObject, player, inputs);
            run.Init(physicsObject, player, inputs);
            jump.Init(physicsObject, player, inputs);
        }

        private void FixedUpdate()
        {
            run.Execute();
            jump.Execute();

            if (jump.CurrentState == JumpState.Rest)
            {
                fall.Execute();
            }
        }
    }

}
