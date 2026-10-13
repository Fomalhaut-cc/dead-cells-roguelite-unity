using Project.Combat;
using Project.Core.Run;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// HUD 的数据面。不创建控件。uGUI 或 UI Toolkit 都可以来读 <see cref="Label"/>。
    /// </summary>
    public sealed class HudPresenter : MonoBehaviour
    {
        public string Label { get; private set; }

        public void Refresh(Health health, RunState run, RollAbility roll)
        {
            var hp = health == null ? "-" : health.Current + " / " + health.Max;
            var carried = run == null ? 0 : run.CarriedCurrency;
            var floor = run == null ? 0 : run.CurrentFloorIndex + 1;
            var vulnerable = roll != null && roll.IsInvulnerable ? "无敌帧" : "可受击";
            Label = "生命 " + hp + "\n携带货币 " + carried + "\n层 " + floor + "\n" + vulnerable;
        }
    }
}
