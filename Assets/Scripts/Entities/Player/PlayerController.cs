using DashAttack.Gameplay.Behaviours.Concretes;
using DashAttack.Gameplay.Behaviours.Interfaces;
using DashAttack.Gameplay.Behaviours.Interfaces.Contexts;
using DashAttack.Gameplay.Behaviours.Interfaces.Datas;
using DashAttack.Physics;
using UnityEngine;

namespace DashAttack.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData player;

        private PlayerContext context;
        private IPhysicsObject physicsObject;

        private IBehaviour<IFallData, IFallContext> fall;
        private IBehaviour<IRunData, IRunContext> run;
        private IBehaviour<IJumpData, IJumpContext> jump;
        private IBehaviour<IWallStickData, IBehaviourContext> wallSitck;
        private IBehaviour<IWallJumpData, IWallJumpContext> wallJump;

        private bool enableFall => !jump.IsExecuting && !wallJump.IsExecuting;

        private bool enableRun => !wallSitck.IsExecuting && !wallJump.IsExecuting;

        private bool enableJump => !wallJump.IsExecuting;

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();

            context = new PlayerContext();

            fall = new Fall();
            run = new Run();
            jump = new Jump();
            wallSitck = new WallStick();
            wallJump = new WallJump();

            fall.Init(physicsObject, player, context);
            run.Init(physicsObject, player, context);
            jump.Init(physicsObject, player, context);
            wallSitck.Init(physicsObject, player, context);
            wallJump.Init(physicsObject, player, context);
        }

        private void FixedUpdate()
        {
            wallSitck.Update();

            if (enableRun)
            {
                run.Update();
            }

            if (enableJump)
            {
                jump.Update();
            }
            else
            {
                jump.Reset();
            }

            if (enableFall)
            {
                fall.Update();
            }
            else
            {
                fall.Reset();
            }

            wallJump.Update();
        }
    }
}