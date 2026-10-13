using System;
using System.Collections.Generic;
using Project.Core;

namespace Project.Core.Run
{
    public enum RunEndReason
    {
        None = 0,
        Death = 1,
        SafePoint = 2,
        Victory = 3,
        Abandon = 4
    }

    /// <summary>
    /// 单局数据。死亡、放弃、通关后都要 <see cref="Clear"/>。
    /// 永久解锁、符文、已入账货币不属于这里。
    /// </summary>
    [Serializable]
    public sealed class RunState
    {
        public int Seed;
        public int CurrentFloorIndex;
        public string CurrentBiomeId = string.Empty;
        public string CurrentRoomId = string.Empty;
        public int Gold;
        public int CarriedCurrency;
        public int ScrollBrutality;
        public int ScrollTactics;
        public int ScrollSurvival;
        public string[] WeaponIds = CreateSlots(LoadoutLimits.Weapons);
        public string[] SkillIds = CreateSlots(LoadoutLimits.Skills);
        public List<string> CarriedBlueprintIds = new List<string>();

        public void Clear()
        {
            CurrentFloorIndex = 0;
            CurrentBiomeId = string.Empty;
            CurrentRoomId = string.Empty;
            Gold = 0;
            CarriedCurrency = 0;
            ScrollBrutality = 0;
            ScrollTactics = 0;
            ScrollSurvival = 0;
            WeaponIds = CreateSlots(LoadoutLimits.Weapons);
            SkillIds = CreateSlots(LoadoutLimits.Skills);
            if (CarriedBlueprintIds == null)
                CarriedBlueprintIds = new List<string>();
            else
                CarriedBlueprintIds.Clear();
        }

        static string[] CreateSlots(int count)
        {
            var slots = new string[count];
            for (var i = 0; i < slots.Length; i++)
                slots[i] = string.Empty;
            return slots;
        }
    }
}
