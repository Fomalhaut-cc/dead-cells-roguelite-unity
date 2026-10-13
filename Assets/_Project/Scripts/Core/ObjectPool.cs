using System;
using System.Collections.Generic;

namespace Project.Core
{
    /// <summary>
    /// 非 Unity 对象的栈式池。子弹、飘字以后用它，不要在热路径上反复 new。
    /// 组件池（实例化 Prefab）Week 2 再包一层，这里不处理 <c>SetActive</c>。
    /// </summary>
    public sealed class ObjectPool<T> where T : class
    {
        readonly Stack<T> stack = new Stack<T>();
        readonly Func<T> create;
        readonly Action<T> onGet;
        readonly Action<T> onRelease;
        readonly int maxInactive;

        public ObjectPool(Func<T> create, Action<T> onGet, Action<T> onRelease, int maxInactive)
        {
            if (create == null)
                throw new ArgumentNullException(nameof(create));

            this.create = create;
            this.onGet = onGet;
            this.onRelease = onRelease;
            this.maxInactive = maxInactive < 1 ? 1 : maxInactive;
        }

        public int CountInactive
        {
            get { return stack.Count; }
        }

        public T Get()
        {
            var item = stack.Count > 0 ? stack.Pop() : create();
            if (onGet != null)
                onGet(item);
            return item;
        }

        public void Release(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (onRelease != null)
                onRelease(item);

            if (stack.Count < maxInactive)
                stack.Push(item);
        }
    }
}
