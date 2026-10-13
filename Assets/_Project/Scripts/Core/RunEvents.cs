using System;
using Project.Core.Run;
using UnityEngine;

namespace Project.Core
{
    /// <summary>
    /// 跨场景的局内事件。Core 不能引用 Meta，所以存档监听挂在这边。
    /// 回调里仍能读到清空前的 <see cref="RunState"/>。
    /// </summary>
    public static class RunEvents
    {
        public static event Action<RunEndReason, RunState> Ended;

        public static void Raise(RunEndReason reason, RunState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var handler = Ended;
            if (handler != null)
                handler(reason, state);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetOnDomainReload()
        {
            Ended = null;
        }
    }
}
