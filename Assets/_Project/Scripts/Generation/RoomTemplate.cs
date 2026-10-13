using System;
using UnityEngine;

namespace Project.Generation
{
    /// <summary>
    /// 门锚点用房间本地坐标。Prefab 的原点要和这里一致。
    /// </summary>
    [Serializable]
    public struct DoorAnchor
    {
        public string AnchorId;
        public DoorDirection Direction;
        public Vector2 LocalPosition;
    }

    /// <summary>
    /// 手制房间的数据。几何放在 Prefab 里，这里只记门、出生点和用途。
    /// 工作流：灰盒摆房间 Prefab → 把门锚点抄进本资产 → 生成器以后按锚点对接。
    /// 当前生成器不会读锚点坐标。
    /// </summary>
    [CreateAssetMenu(fileName = "Room", menuName = "Project/Generation/Room Template")]
    public sealed class RoomTemplate : ScriptableObject
    {
        public string Id = "room.unnamed";
        public string BiomeId = string.Empty;
        public RoomKind Purpose = RoomKind.Combat;
        public Vector2Int SizeInTiles = new Vector2Int(16, 10);
        public DoorAnchor[] Doors = new DoorAnchor[0];
        public Vector2[] EnemySpawnPoints = new Vector2[0];
        public Vector2 PlayerSpawn;
        public GameObject RoomPrefab;
    }
}
