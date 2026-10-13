using UnityEngine;

namespace Project.Build
{
    /// <summary>
    /// 一件装备的静态定义。局内实例（词缀、等级）以后另做，不要把掉落状态写进资产。
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Project/Build/Item Definition")]
    public sealed class ItemDefinition : ScriptableObject
    {
        public string Id = "item.unnamed";
        public string DisplayName = "未命名";
        [TextArea(2, 4)] public string Description = string.Empty;
        public ItemRarity Rarity = ItemRarity.Common;
        public EquipmentSlot Slot = EquipmentSlot.Weapon;
        public ItemTag Tags = ItemTag.None;
        public int BaseDamage = 10;
        public float CooldownSeconds = 0.4f;
        public bool OccupiesBothWeaponSlots;
        public EffectTrigger[] Triggers = new EffectTrigger[0];
    }
}
