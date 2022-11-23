using UniGame.Core.Runtime.ObjectPool;

namespace UniGame.Core.Runtime
{
    using System.Collections.Generic;

    public interface IUnorderedCollection<T> : IPoolable where T : class
    {
        int Count { get; }
        IEnumerable<T> GetItems();

        /// <summary>
        /// add item to collection and return unique id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Add(T item);

        T Remove(int itemId);
        void Clear();
    }
}