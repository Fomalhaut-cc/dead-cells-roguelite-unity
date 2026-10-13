using System;
using System.Collections.Generic;

namespace Project.Build
{
    public enum EffectTriggerKind
    {
        Passive = 0,
        OnHit = 1,
        OnKill = 2,
        OnRoll = 3,
        OnDamaged = 4
    }

    /// <summary>
    /// 一条还没实现的触发器。Week 2 再按 <see cref="EffectId"/> 找处理器。
    /// </summary>
    [Serializable]
    public sealed class EffectTrigger
    {
        public string EffectId = string.Empty;
        public EffectTriggerKind Kind = EffectTriggerKind.Passive;
        public ItemTag RequiredTags = ItemTag.None;
    }

    /// <summary>
    /// 只数匹配条数，不改血量、不生成子弹。
    /// </summary>
    public static class EffectDispatcher
    {
        public static int CountMatches(ItemTag sourceTags, EffectTriggerKind kind, IReadOnlyList<EffectTrigger> triggers)
        {
            if (triggers == null)
                return 0;

            var count = 0;
            for (var i = 0; i < triggers.Count; i++)
            {
                var trigger = triggers[i];
                if (trigger == null || trigger.Kind != kind)
                    continue;
                if (trigger.RequiredTags != ItemTag.None && (sourceTags & trigger.RequiredTags) == 0)
                    continue;
                count++;
            }

            return count;
        }
    }
}
