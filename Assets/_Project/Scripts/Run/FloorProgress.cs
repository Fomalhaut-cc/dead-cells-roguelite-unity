using System;

namespace Project.Core.Run
{
    /// <summary>
    /// 当前层清房进度。换层时重置，死亡时随场景一起丢掉，不写入存档。
    /// </summary>
    [Serializable]
    public sealed class FloorProgress
    {
        public int ClearedCombatRooms;
        public int TotalCombatRooms;
        public string CurrentRoomId = string.Empty;
        public bool BossReached;

        public void ResetForFloor(int combatRooms, string roomId)
        {
            ClearedCombatRooms = 0;
            TotalCombatRooms = combatRooms < 0 ? 0 : combatRooms;
            CurrentRoomId = roomId ?? string.Empty;
            BossReached = false;
        }
    }
}
