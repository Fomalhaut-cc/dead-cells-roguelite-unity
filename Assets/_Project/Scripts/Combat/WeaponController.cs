using Project.Build;
using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 攻击入口。知道当前武器定义，但不会播动画，也不会生成刀光。
    /// Week 1：近战打开子节点 Hitbox 几帧，远程调用 <see cref="Projectile.Launch"/>。
    /// </summary>
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] ItemDefinition primary;
        [SerializeField] ItemDefinition skillA;
        [SerializeField] ItemDefinition skillB;

        public ItemDefinition Primary { get { return primary; } }

        public void ConfigurePrimary(ItemDefinition weapon)
        {
            primary = weapon;
        }

        public void ConfigureSkill(int slotIndex, ItemDefinition skill)
        {
            if (slotIndex == 0)
                skillA = skill;
            else if (slotIndex == 1)
                skillB = skill;
        }

        public void TryPrimaryAttack()
        {
        }

        public ItemDefinition GetSkill(int slotIndex)
        {
            if (slotIndex == 0)
                return skillA;
            if (slotIndex == 1)
                return skillB;
            return null;
        }

        public void TrySkill(int slotIndex)
        {
            if (GetSkill(slotIndex) == null)
                return;
        }
    }
}
