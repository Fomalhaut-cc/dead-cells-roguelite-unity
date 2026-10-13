using System;
using UnityEngine;

namespace Project.Generation
{
    [Serializable]
    public sealed class FlowNode
    {
        public string Id = "node";
        public RoomKind Kind = RoomKind.Combat;
        public string BiomeId = string.Empty;
    }

    /// <summary>
    /// 有向边。RequiredRuneId 为空表示这条概念图边没有门条件。
    /// 层内概念图通常不加符文；符文门放在岛屿路由上。
    /// </summary>
    [Serializable]
    public sealed class FlowEdge
    {
        public string FromId = string.Empty;
        public string ToId = string.Empty;
        public string RequiredRuneId = string.Empty;
    }

    /// <summary>
    /// 一个生物群系内部的概念图：多长、有哪些商店和 Boss，不包含具体几何。
    /// </summary>
    [CreateAssetMenu(fileName = "Flow", menuName = "Project/Generation/Flow Graph")]
    public sealed class FlowGraph : ScriptableObject
    {
        public string Id = "flow.unnamed";
        public FlowNode[] Nodes = new FlowNode[0];
        public FlowEdge[] Edges = new FlowEdge[0];
    }
}
