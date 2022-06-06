﻿using UnityEngine;

namespace DashAttack
{
    public class PlayerController : MonoBehaviour
    {
        private IPhysicsObject physicsObject;
        private Player player;
        private PlayerInputs inputs;

        private IBehaviour<IFallData, IFallInput> fall;
        private IBehaviour<IRunData, IRunInput> run;
        private IBehaviour<IJumpData, IJumpInput> jump;
        private IBehaviour<IWallStickData, IBehaviourContext> wallSitck;
        private IBehaviour<IWallJumpData, IWallJumpInput> wallJump;

        private bool enableFall => !jump.IsExecuting && !wallJump.IsExecuting;

        private bool enableRun => !wallSitck.IsExecuting && !wallJump.IsExecuting;

        private bool enableJump => !wallJump.IsExecuting;

        private void Start()
        {
            physicsObject = GetComponent<IPhysicsObject>();

            player = GetComponent<Player>();
            inputs = new PlayerInputs();

            fall = new Fall();
            run = new Run();
            jump = new Jump();
            wallSitck = new WallStick();
            wallJump = new WallJump();

            fall.Init(physicsObject, player, inputs);
            run.Init(physicsObject, player, inputs);
            jump.Init(physicsObject, player, inputs);
            wallSitck.Init(physicsObject, player, inputs);
            wallJump.Init(physicsObject, player, inputs);
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