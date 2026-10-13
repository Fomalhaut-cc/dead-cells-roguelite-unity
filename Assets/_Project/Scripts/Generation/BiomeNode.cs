using System;

namespace Project.Generation
{
    /// <summary>
    /// 全岛路由上的一个生物群系。层内房间拼装看 <see cref="FloorFlowId"/> 指向的概念图。
    /// </summary>
    [Serializable]
    public sealed class BiomeNode
    {
        public string Id = "biome";
        public string DisplayName = "未命名区域";
        public string FloorFlowId = string.Empty;
        public string RequiredRuneId = string.Empty;
    }

    /// <summary>
    /// 岛屿图上的一条有向边。符文 Id 为空表示不设门。
    /// </summary>
    [Serializable]
    public sealed class BiomeEdge
    {
        public string FromId = string.Empty;
        public string ToId = string.Empty;
        public string RequiredRuneId = string.Empty;
    }
}
