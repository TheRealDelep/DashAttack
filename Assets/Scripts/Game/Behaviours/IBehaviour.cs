﻿using DashAttack.Core.Physics.Interfaces;

namespace DashAttack.Game.Behaviours
{
    public interface IBehaviour
    {
        bool IsExecuting { get; }

        void Update();

        void Reset();
    }

    public interface IBehaviour<TData, TContext> : IBehaviour
        where TContext : IBehaviourContext
    {
        void Init(IPhysicsObject physicsObject, TData data, TContext input);
    }
}