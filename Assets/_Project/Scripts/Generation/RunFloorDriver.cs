using Project.Core;
using Project.Core.Run;
using UnityEngine;

namespace Project.Generation
{
    /// <summary>
    /// 把 <see cref="FloorGenerator"/> 接到当前局。不会在 Start 时自动生成。
    /// 在组件菜单里选 Generate Stub Floor 可在编辑器里看日志。
    /// </summary>
    public sealed class RunFloorDriver : MonoBehaviour
    {
        [SerializeField] FlowGraph flow;
        [SerializeField] RoomTemplate[] catalog = new RoomTemplate[0];

        readonly FloorGenerator generator = new FloorGenerator();

        public FloorPlan LastPlan { get; private set; }

        public FloorPlan GenerateFor(RunState run)
        {
            if (run == null)
                throw new System.ArgumentNullException(nameof(run));
            if (flow == null)
                throw new System.InvalidOperationException("RunFloorDriver 没有指定 FlowGraph。");

            LastPlan = generator.Generate(run.Seed, flow, catalog);
            string report;
            var ok = ConnectivityCheck.TryExplain(LastPlan, out report);
            Debug.Log(ok ? "楼层草稿通过：" + report : "楼层草稿未通过：" + report, this);
            return LastPlan;
        }

        [ContextMenu("Generate Stub Floor")]
        void GenerateFromMenu()
        {
            RunState run;
            if (!ServiceLocator.TryResolve<RunState>(out run))
            {
                run = new RunState();
                run.Seed = 1;
            }

            GenerateFor(run);
        }
    }
}
