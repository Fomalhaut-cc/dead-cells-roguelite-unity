using System;
using UnityEngine;

namespace Project.Meta
{
    public enum UnlockKind
    {
        Item = 0,
        Mutation = 1,
        Rune = 2,
        Upgrade = 3
    }

    [Serializable]
    public sealed class UnlockEntry
    {
        public string Id = string.Empty;
        public string DisplayName = string.Empty;
        public UnlockKind Kind = UnlockKind.Item;
        public int Cost;
    }

    /// <summary>
    /// 解锁价目表。条目用字符串 Id，不直接引用装备资产，这样 Meta 不必依赖 Build。
    /// </summary>
    [CreateAssetMenu(fileName = "UnlockCatalog", menuName = "Project/Meta/Unlock Catalog")]
    public sealed class UnlockCatalog : ScriptableObject
    {
        public UnlockEntry[] Entries = new UnlockEntry[0];

        public UnlockEntry Find(string id)
        {
            if (Entries == null || string.IsNullOrEmpty(id))
                return null;

            for (var i = 0; i < Entries.Length; i++)
            {
                if (Entries[i] != null && Entries[i].Id == id)
                    return Entries[i];
            }

            return null;
        }
    }

    /// <summary>
    /// 花钱解锁。不负责写盘，调用方成功后自己 <see cref="MetaSaveService.Save"/>。
    /// </summary>
    public static class UnlockService
    {
        public static bool TryPurchase(MetaState meta, UnlockEntry entry, out string error)
        {
            if (meta == null)
                throw new ArgumentNullException(nameof(meta));
            if (entry == null || string.IsNullOrEmpty(entry.Id))
            {
                error = "解锁条目为空。";
                return false;
            }

            MetaSaveService.Normalize(meta);
            if (IsOwned(meta, entry))
            {
                error = "已经解锁。";
                return false;
            }

            if (!MetaCurrency.TrySpend(meta, entry.Cost))
            {
                error = "货币不足。";
                return false;
            }

            Grant(meta, entry);
            error = null;
            return true;
        }

        public static bool IsOwned(MetaState meta, UnlockEntry entry)
        {
            if (meta == null || entry == null)
                return false;

            MetaSaveService.Normalize(meta);
            switch (entry.Kind)
            {
                case UnlockKind.Item:
                    return meta.UnlockedItemIds.Contains(entry.Id);
                case UnlockKind.Mutation:
                    return meta.UnlockedMutationIds.Contains(entry.Id);
                case UnlockKind.Rune:
                    return meta.OwnedRuneIds.Contains(entry.Id);
                case UnlockKind.Upgrade:
                    return meta.PurchasedUpgradeIds.Contains(entry.Id);
                default:
                    return false;
            }
        }

        static void Grant(MetaState meta, UnlockEntry entry)
        {
            switch (entry.Kind)
            {
                case UnlockKind.Item:
                    meta.UnlockedItemIds.Add(entry.Id);
                    break;
                case UnlockKind.Mutation:
                    meta.UnlockedMutationIds.Add(entry.Id);
                    break;
                case UnlockKind.Rune:
                    meta.OwnedRuneIds.Add(entry.Id);
                    break;
                case UnlockKind.Upgrade:
                    meta.PurchasedUpgradeIds.Add(entry.Id);
                    break;
            }
        }
    }
}
