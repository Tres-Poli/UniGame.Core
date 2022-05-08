﻿namespace UniModules.UniCore.Runtime.Common
{
    using System;
    using ObjectPool.Runtime.Extensions;
    using ObjectPool.Runtime.Interfaces;
    using UniGame.Core.Runtime.Interfaces;

    public class DisposableAction<TArg> : IDisposableItem,IPoolable
    {
        private Action<TArg> _onDisposed;
        private TArg _arg;
    
        public bool IsComplete { get; protected set; }
    
        public void Initialize(Action<TArg> action, TArg arg)
        {
            IsComplete = false;
            _onDisposed = action;
            _arg = arg;
        }

        public void Complete()
        {
            IsComplete = true;
            _onDisposed = null;
            _arg = default(TArg);
        }
    
        public void Dispose()
        {
            if (!IsComplete)
                _onDisposed?.Invoke(_arg);
            
            //return to pool
            this.DespawnWithRelease();
        }

        public void Release() => Complete();

    }
}
