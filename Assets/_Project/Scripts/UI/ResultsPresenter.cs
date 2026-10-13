using Project.Core.Run;
using Project.Meta;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// 死亡或通关文案。不切场景，不写存档。
    /// </summary>
    public sealed class ResultsPresenter : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string Summary { get; private set; }

        public void ShowDeath(RunState run, MetaState meta)
        {
            IsOpen = true;
            Summary = Build(run, meta, "死了。携带的货币没有入账。");
        }

        public void ShowVictory(RunState run, MetaState meta)
        {
            IsOpen = true;
            Summary = Build(run, meta, "通关。携带的货币会入账。");
        }

        public void Close()
        {
            IsOpen = false;
        }

        static string Build(RunState run, MetaState meta, string headline)
        {
            var seed = run == null ? 0 : run.Seed;
            var floor = run == null ? 0 : run.CurrentFloorIndex + 1;
            var carried = run == null ? 0 : run.CarriedCurrency;
            var banked = meta == null ? 0 : meta.BankedCurrency;
            return headline + "\n种子 " + seed + "\n到达层 " + floor + "\n携带 " + carried + "\n已入账 " + banked;
        }
    }
}
