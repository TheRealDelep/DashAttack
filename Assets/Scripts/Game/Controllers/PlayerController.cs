using DashAttack.Game.Behaviours;
using DashAttack.Game.Behaviours.Fall;
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
        private IBehaviour<IRunData, IRunInput> run;

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();

            player = GetComponent<Player>();
            inputs = new PlayerInputs();
            inputs.Init(physicsObject);

            fall = new Fall();
            run = new Run();

            fall.Init(physicsObject, player, inputs);
            run.Init(physicsObject, player, inputs);
        }

        private void FixedUpdate()
        {
            fall.Execute();
            run.Execute();
        }
    }

}
