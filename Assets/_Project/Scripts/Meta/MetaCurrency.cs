using System;

namespace Project.Meta
{
    /// <summary>
    /// 已入账货币的加减。携带中的数额在 RunState.CarriedCurrency，死亡时丢掉。
    /// </summary>
    public static class MetaCurrency
    {
        public static void Deposit(MetaState meta, int amount)
        {
            if (meta == null)
                throw new ArgumentNullException(nameof(meta));
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            meta.BankedCurrency += amount;
        }

        public static bool TrySpend(MetaState meta, int amount)
        {
            if (meta == null)
                throw new ArgumentNullException(nameof(meta));
            if (amount < 0)
                return false;
            if (meta.BankedCurrency < amount)
                return false;

            meta.BankedCurrency -= amount;
            return true;
        }
    }
}
