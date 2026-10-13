using System;
using System.Collections.Generic;

namespace Project.Meta
{
    /// <summary>
    /// 跨局存档。死亡不清这里。
    /// Version 从 1 开始。加字段时递增，并在 <see cref="MetaSaveService"/> 里写迁移。
    /// BankedCurrency 是已入账的元货币。设计类比是「带到安全点才算数的细胞」，代码和资源名不用作品专名。
    /// </summary>
    [Serializable]
    public sealed class MetaState
    {
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public int BankedCurrency;
        public List<string> UnlockedItemIds = new List<string>();
        public List<string> UnlockedMutationIds = new List<string>();
        public List<string> OwnedRuneIds = new List<string>();
        public List<string> DiscoveredBlueprintIds = new List<string>();
        public List<string> PurchasedUpgradeIds = new List<string>();
        public int DifficultyMarksOwned;
        public int ForgeQualityLevel;

        public bool OwnsRune(string runeId)
        {
            return !string.IsNullOrEmpty(runeId) && OwnedRuneIds != null && OwnedRuneIds.Contains(runeId);
        }
    }
}
