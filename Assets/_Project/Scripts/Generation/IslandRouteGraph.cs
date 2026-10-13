using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Generation
{
    /// <summary>
    /// 固定的岛屿有向图。不随单局种子改变。
    /// 节点类型是 <see cref="BiomeNode"/>，放在单独的文件里，因为这个类名要和文件名一致。
    /// </summary>
    [CreateAssetMenu(fileName = "IslandRoute", menuName = "Project/Generation/Island Route")]
    public sealed class IslandRouteGraph : ScriptableObject
    {
        public string Id = "route.starter";
        public BiomeNode[] Nodes = new BiomeNode[0];
        public BiomeEdge[] Edges = new BiomeEdge[0];

        public BiomeNode Find(string biomeId)
        {
            if (Nodes == null || string.IsNullOrEmpty(biomeId))
                return null;

            for (var i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i] != null && Nodes[i].Id == biomeId)
                    return Nodes[i];
            }

            return null;
        }

        public void CollectExits(string biomeId, List<BiomeEdge> results)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            results.Clear();
            if (Edges == null)
                return;

            for (var i = 0; i < Edges.Length; i++)
            {
                var edge = Edges[i];
                if (edge != null && edge.FromId == biomeId)
                    results.Add(edge);
            }
        }

        /// <summary>
        /// <paramref name="hasRune"/> 由 Meta 提供，例如 id =&gt; owned.Contains(id)。
        /// 空符文条件视为已满足。
        /// </summary>
        public bool CanTraverse(BiomeEdge edge, Func<string, bool> hasRune)
        {
            if (edge == null)
                return false;
            if (!RuneOpen(edge.RequiredRuneId, hasRune))
                return false;

            var destination = Find(edge.ToId);
            return destination == null || RuneOpen(destination.RequiredRuneId, hasRune);
        }

        static bool RuneOpen(string runeId, Func<string, bool> hasRune)
        {
            if (string.IsNullOrEmpty(runeId))
                return true;
            return hasRune != null && hasRune(runeId);
        }
    }
}
