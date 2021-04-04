﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniModules.UniCore.Tests.IntegrationTests.ClassPoolTests
{
    using System.Collections;
    using System.Collections.Generic;
    using Runtime.ObjectPool.Runtime;
    using Runtime.ObjectPool.Runtime.Extensions;

    public class ClassPoolPrefomanceTest : MonoBehaviour
    {
        private List<List<int>> items = new List<List<int>>();
        public int poolSize = 10000;
        public bool disableSpawn = false;
        
        // Start is called before the first frame update
        private void Start()
        {
            OnUpdate().ToUniTask();
        }

        private IEnumerator OnUpdate()
        {
            while (isActiveAndEnabled) {

                for (int i = 0; i < poolSize; i++) {
                   
                    var item = disableSpawn ? new List<int>() : 
                        ClassPool.Spawn<List<int>>();
                    items.Add(item);
                    
                }

                yield return null;

                if (!disableSpawn) {
                    for (int i = 0; i < items.Count; i++) {

                        var item = items[i];
                        item.Despawn();
                    
                    }
                }
  
                items.Clear();

                yield return null;

            }
        }
    }
}
