using UnityEngine;

namespace Project.Core
{
    /// <summary>
    /// 暂停开关。三选一和结算打开时调用 <see cref="Pause"/>。
    /// 会把 <c>Time.timeScale</c> 设为 0，物理和普通动画一起停。
    /// </summary>
    public sealed class GameClock
    {
        public bool IsPaused { get; private set; }

        public float DeltaTime
        {
            get { return IsPaused ? 0f : Time.deltaTime; }
        }

        public float UnscaledDeltaTime
        {
            get { return Time.unscaledDeltaTime; }
        }

        public void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
    }
}
