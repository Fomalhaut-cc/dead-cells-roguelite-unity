using Project.Core;
using UnityEngine;

namespace Project.Core.Run
{
    /// <summary>
    /// 单局流程占位。只改 <see cref="RunState"/>，不碰元进度。
    /// 事件在 <see cref="RunState.Clear"/> 之前发出，监听方能读到本局结算数字。
    /// </summary>
    public sealed class RunController : MonoBehaviour
    {
        public RunState State { get; private set; }
        public FloorProgress Progress { get; private set; }

        void Awake()
        {
            State = new RunState();
            Progress = new FloorProgress();
            ServiceLocator.Register(State);
            ServiceLocator.Register(this);
        }

        void OnDestroy()
        {
            RunController current;
            if (ServiceLocator.TryResolve<RunController>(out current) && current == this)
                ServiceLocator.Unregister<RunController>();
        }

        public void BeginRun(int seed, string startBiomeId)
        {
            State.Clear();
            Progress.ResetForFloor(0, string.Empty);
            State.Seed = seed;
            State.CurrentBiomeId = startBiomeId ?? string.Empty;

            IRandomService random;
            if (ServiceLocator.TryResolve(out random))
                random.Reseed(seed);
            else
                ServiceLocator.Register<IRandomService>(new SeededRandom(seed));
        }

        public void NotifyDeath()
        {
            RunEvents.Raise(RunEndReason.Death, State);
            State.Clear();
            Progress.ResetForFloor(0, string.Empty);
        }

        public void NotifyAbandon()
        {
            RunEvents.Raise(RunEndReason.Abandon, State);
            State.Clear();
            Progress.ResetForFloor(0, string.Empty);
        }

        /// <summary>
        /// 安全点。不结束本局。入账由 Meta 监听完成。
        /// </summary>
        public void NotifySafePoint()
        {
            RunEvents.Raise(RunEndReason.SafePoint, State);
        }

        public void NotifyVictory()
        {
            RunEvents.Raise(RunEndReason.Victory, State);
            State.Clear();
            Progress.ResetForFloor(0, string.Empty);
        }
    }
}
