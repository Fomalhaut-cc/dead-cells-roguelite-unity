using System;

namespace Project.Build
{
    public enum ItemRarity
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Legendary = 3
    }

    public enum EquipmentSlot
    {
        Weapon = 0,
        Skill = 1
    }

    /// <summary>
    /// 构筑标签。协同只认标签，不为每一对道具写分支。
    /// 数值是占位，效果代码还没接上。
    /// </summary>
    [Flags]
    public enum ItemTag
    {
        None = 0,
        Poison = 1 << 0,
        OnHit = 1 << 1,
        ProjectileCount = 1 << 2,
        Melee = 1 << 3,
        Ranged = 1 << 4,
        Burn = 1 << 5,
        Freeze = 1 << 6,
        Area = 1 << 7
    }
}
