using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core
{
    /// <summary>
    /// 主线程上的类型服务表。故意做成这么小，方便 Week 1 把随机数、时钟和存档挂上。
    /// 不要在这里加生命周期、作用域或自动注入。
    /// </summary>
    public static class ServiceLocator
    {
        static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            Services[typeof(T)] = service;
        }

        public static bool TryResolve<T>(out T service) where T : class
        {
            object raw;
            if (Services.TryGetValue(typeof(T), out raw))
            {
                service = raw as T;
                return service != null;
            }

            service = null;
            return false;
        }

        public static T Resolve<T>() where T : class
        {
            T service;
            if (!TryResolve(out service))
                throw new InvalidOperationException("Service not registered: " + typeof(T).Name);

            return service;
        }

        public static void Unregister<T>() where T : class
        {
            Services.Remove(typeof(T));
        }

        public static void Clear()
        {
            Services.Clear();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetOnDomainReload()
        {
            Clear();
        }
    }
}
