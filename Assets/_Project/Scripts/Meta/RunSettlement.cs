using Project.Core.Run;

namespace Project.Meta
{
    /// <summary>
    /// 死亡和安全点的边界。死亡只清局内；安全点和通关才把携带货币入账。
    /// </summary>
    public static class RunSettlement
    {
        public static void ApplyDeath(RunState run)
        {
            if (run == null)
                return;
            run.Clear();
        }

        public static void BankCarried(RunState run, MetaState meta)
        {
            if (run == null || meta == null)
                return;

            MetaSaveService.Normalize(meta);
            MetaCurrency.Deposit(meta, run.CarriedCurrency);
            run.CarriedCurrency = 0;

            if (run.CarriedBlueprintIds == null)
                return;

            for (var i = 0; i < run.CarriedBlueprintIds.Count; i++)
            {
                var id = run.CarriedBlueprintIds[i];
                if (!string.IsNullOrEmpty(id) && !meta.DiscoveredBlueprintIds.Contains(id))
                    meta.DiscoveredBlueprintIds.Add(id);
            }

            run.CarriedBlueprintIds.Clear();
        }
    }
}
