using Project.Build;
using UnityEngine;

namespace Project.Combat
{
    public enum CombatTeam
    {
        Player = 0,
        Enemy = 1
    }

    /// <summary>
    /// 一次命中的数据。没有追伤、破招或致命保护。
    /// </summary>
    public struct DamageInfo
    {
        public int Amount;
        public string SourceId;
        public ItemTag Tags;
        public Vector2 Knockback;
        public bool IsCritical;
        public CombatTeam Team;
    }
}
