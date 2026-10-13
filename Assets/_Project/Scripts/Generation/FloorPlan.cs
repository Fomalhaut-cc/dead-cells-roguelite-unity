using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Generation
{
    [Serializable]
    public sealed class PlacedRoom
    {
        public string NodeId = string.Empty;
        public string TemplateId = string.Empty;
        public RoomKind Kind = RoomKind.Combat;
        public Vector2Int GridOrigin;
        public bool TemplateMissing;
    }

    [Serializable]
    public sealed class RoomLink
    {
        public string FromNodeId = string.Empty;
        public string ToNodeId = string.Empty;
        public string FromDoorId = string.Empty;
        public string ToDoorId = string.Empty;
    }

    /// <summary>
    /// 一层的逻辑结果。还不是实例化后的场景物体。
    /// </summary>
    [Serializable]
    public sealed class FloorPlan
    {
        public int Seed;
        public string FlowId = string.Empty;
        public string PlacementMode = "LinearStub";
        public List<PlacedRoom> Rooms = new List<PlacedRoom>();
        public List<RoomLink> Links = new List<RoomLink>();
    }
}
