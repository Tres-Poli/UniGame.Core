﻿namespace UniModules.UniCore.Runtime.DataFlow
{
    using Interfaces;
    using ObjectPool.Runtime.Extensions;
    using ObjectPool.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public class LifeTimeModel : ILifeTimeModel, IPoolable
    {
        
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();

        public ILifeTime LifeTime => lifeTimeDefinition;


        /// <summary>
        /// cleanup item without despawn
        /// </summary>
        public void Release()
        {
            lifeTimeDefinition.Release();
            OnCleanUp();
        }
        
        /// <summary>
        /// custom cleanup action
        /// </summary>
        protected virtual void OnCleanUp(){}

        /// <summary>
        /// despawn movel
        /// </summary>
        public void Dispose() => lifeTimeDefinition.Terminate();
    }
}
