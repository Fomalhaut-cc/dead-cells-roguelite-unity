using Project.Core;
using Project.Core.Run;

namespace Project.Build
{
    /// <summary>
    /// 运行时的 2 武器 + 2 技能。双手武器占满两个武器槽。
    /// 字符串 Id 写回 <see cref="RunState"/>，定义资产留在本类。
    /// </summary>
    public sealed class InventorySlots
    {
        readonly ItemDefinition[] weapons = new ItemDefinition[LoadoutLimits.Weapons];
        readonly ItemDefinition[] skills = new ItemDefinition[LoadoutLimits.Skills];

        public ItemDefinition GetWeapon(int index)
        {
            if (index < 0 || index >= weapons.Length)
                return null;
            return weapons[index];
        }

        public ItemDefinition GetSkill(int index)
        {
            if (index < 0 || index >= skills.Length)
                return null;
            return skills[index];
        }

        public bool TrySetWeapon(int index, ItemDefinition item, out string error)
        {
            if (index < 0 || index >= weapons.Length)
            {
                error = "武器槽下标越界。";
                return false;
            }

            if (item == null)
            {
                weapons[index] = null;
                error = null;
                return true;
            }

            if (item.Slot != EquipmentSlot.Weapon)
            {
                error = "这件物品不是武器。";
                return false;
            }

            if (item.OccupiesBothWeaponSlots)
            {
                weapons[0] = item;
                weapons[1] = null;
                error = null;
                return true;
            }

            if (weapons[0] != null && weapons[0].OccupiesBothWeaponSlots)
                weapons[0] = null;

            weapons[index] = item;
            error = null;
            return true;
        }

        public bool TrySetSkill(int index, ItemDefinition item, out string error)
        {
            if (index < 0 || index >= skills.Length)
            {
                error = "技能槽下标越界。";
                return false;
            }

            if (item == null)
            {
                skills[index] = null;
                error = null;
                return true;
            }

            if (item.Slot != EquipmentSlot.Skill)
            {
                error = "这件物品不是技能。";
                return false;
            }

            skills[index] = item;
            error = null;
            return true;
        }

        public void CopyIdsTo(RunState run)
        {
            if (run == null)
                return;

            Copy(weapons, run.WeaponIds);
            Copy(skills, run.SkillIds);
        }

        static void Copy(ItemDefinition[] source, string[] destination)
        {
            if (destination == null)
                return;

            var count = source.Length < destination.Length ? source.Length : destination.Length;
            for (var i = 0; i < count; i++)
                destination[i] = source[i] != null && source[i].Id != null ? source[i].Id : string.Empty;
        }
    }
}
