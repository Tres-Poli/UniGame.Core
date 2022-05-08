﻿using UniModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;

namespace UniGame.Core.Runtime.Common
{
    using System;
    using UniModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public class LifeTimedAction : IPoolable,ILifeTimedAction
    {
        private Action _action;
        private ILifeTime _lifeTime;
        
        public void Initialize(ILifeTime lifetime, Action action, Action onLifeTimeFinished = null)
        {
            _lifeTime = lifetime;
            _action = action;
            
            if (onLifeTimeFinished != null)
                lifetime.AddCleanUpAction(onLifeTimeFinished);
        }

        public void Invoke()
        {
            if (_lifeTime.IsTerminated)
                return;
            _action?.Invoke();
        }
        
        public void Release()
        {
            _lifeTime = null;
            _action = null;
        }

        public void Dispose() => this.DespawnWithRelease();
    }
}
