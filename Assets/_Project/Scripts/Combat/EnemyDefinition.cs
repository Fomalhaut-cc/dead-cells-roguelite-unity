using UnityEngine;

namespace Project.Combat
{
    public enum EnemyRole
    {
        Walker = 0,
        Shooter = 1,
        Elite = 2,
        Boss = 3
    }

    /// <summary>
    /// 敌人静态数值。没有行为树，也没有招式表。
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy", menuName = "Project/Combat/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        public string Id = "enemy.unnamed";
        public string DisplayName = "未命名敌人";
        public EnemyRole Role = EnemyRole.Walker;
        public int MaxHealth = 30;
        public int ContactDamage = 8;
    }
}
