using System;

using DashAttack.Assets.Scripts.Gameplay.Behaviours.Interfaces;
using DashAttack.Gameplay.Behaviours.Interfaces;

namespace DashAttack.Gameplay.Behaviours
{
    public abstract class BehaviourBase<TData, TContext> : IBehaviour
        where TData : IBehaviourData
        where TContext : IBehaviourContext
    {
        protected TData Data;
        protected TContext Context;

        protected BehaviourBase(TData data, TContext context)
        {
            Data = data;
            Context = context;
        }

        public virtual void Update()
        { }
    }
}
