﻿namespace UniModules.UniCore.Runtime.CoroutineTools
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ObjectPool.Runtime;
    using ObjectPool.Runtime.Extensions;
    using UniRx;

    public static class CoroutineTools {

        public static void WaitCoroutine(this IEnumerator enumerator) {

            if (enumerator == null) return;
            var stack = ClassPool.Spawn < Stack < IEnumerator >> ();
            var wait = false;

            do {

                wait = enumerator.MoveNext();
                if (wait == false && stack.Count > 0) {
                    enumerator = stack.Pop();
                    wait = true;
                    continue;
                }

                if (wait == false)
                    continue;

                var current = enumerator.Current;
                var currentEnumerator = current as IEnumerator;

                if (currentEnumerator != enumerator && currentEnumerator != null) {
                    stack.Push(enumerator);
                    enumerator = currentEnumerator;
                }

            } while (wait == true);
            
            stack.DespawnObject(stack.Clear);

        }

        public static IEnumerator<T> WaitCoroutine<T>(this IEnumerator enumerator, Func<T> awaiterFunc) {

            var awaiter = ClassPool.Spawn<CoroutineIterator>();
            awaiter.Initialize(enumerator);
            while (awaiter.IsDone == false) {
                awaiter.MoveNext();
                yield return awaiterFunc();
            }
            awaiter.Despawn();
        }

        public static IEnumerator WaitCoroutines(this List<IEnumerator> enumerators) {

            var iterators = ClassPool.Spawn<List<CoroutineIterator>>();

            for (int i = 0; i < enumerators.Count; i++) {
                var it = ClassPool.Spawn<CoroutineIterator>();
                it.Initialize(enumerators[i]);
                iterators.Add(it);
            }   

            while (iterators.TrueForAll(x => x.IsDone) == false) {

                for (int i = 0; i < iterators.Count; i++) {
                    var it = iterators[i];
                    if(it.IsDone)continue;
                    it.MoveNext();
                }

                yield return null;
            }

            iterators.DespawnRecursive<List<CoroutineIterator>,CoroutineIterator>();
        }

        public static IEnumerator WaitCoroutinesAsync(this List<IEnumerator> enumerators)
        {
            var awaiters = ClassPool.Spawn<List<bool>>();

            for (int i = 0; i < enumerators.Count; i++)
            {
                var index = i;
                var enumerator = enumerators[index];
                awaiters.Add(false);
                Observable.FromCoroutine(x => enumerator).DoOnCompleted(() => awaiters[index] = true).RxSubscribe();
            }

            while (awaiters.TrueForAll(x => x) == false)
            {
                yield return null;
            }

            awaiters.DespawnCollection<List<bool>,bool>();
        }
    }
}
