using Project.Core;
using Project.Core.Run;
using UnityEngine;

namespace Project.Meta
{
    /// <summary>
    /// 读取 JSON 存档，并在安全点或通关时入账。死亡不写盘。
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public sealed class MetaBootstrap : MonoBehaviour
    {
        public MetaState State { get; private set; }

        void Awake()
        {
            State = MetaSaveService.LoadOrCreate();
            ServiceLocator.Register(State);
        }

        void OnEnable()
        {
            RunEvents.Ended += OnRunEnded;
        }

        void OnDisable()
        {
            RunEvents.Ended -= OnRunEnded;
        }

        void OnRunEnded(RunEndReason reason, RunState run)
        {
            if (reason != RunEndReason.SafePoint && reason != RunEndReason.Victory)
                return;

            RunSettlement.BankCarried(run, State);
            MetaSaveService.Save(State);
        }
    }
}
