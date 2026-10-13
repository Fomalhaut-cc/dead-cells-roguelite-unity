using System;
using System.Collections.Generic;
using Project.Core;

namespace Project.Generation
{
    /// <summary>
    /// 按概念图顺序挑房间，并排成一行。不做门对门的几何求解，失败也不重试。
    /// 同一种子、同一概念图、同一目录顺序会得到同一份 <see cref="FloorPlan"/>。
    /// </summary>
    public sealed class FloorGenerator
    {
        public FloorPlan Generate(int seed, FlowGraph flow, IReadOnlyList<RoomTemplate> catalog)
        {
            if (flow == null)
                throw new ArgumentNullException(nameof(flow));

            var random = new SeededRandom(seed);
            var plan = new FloorPlan
            {
                Seed = seed,
                FlowId = flow.Id ?? string.Empty,
                PlacementMode = "LinearStub"
            };

            var nodes = flow.Nodes ?? new FlowNode[0];
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                if (node == null)
                    continue;

                var template = PickTemplate(random, catalog, node);
                plan.Rooms.Add(new PlacedRoom
                {
                    NodeId = node.Id ?? string.Empty,
                    TemplateId = template != null ? template.Id : "missing." + node.Kind,
                    Kind = node.Kind,
                    GridOrigin = new UnityEngine.Vector2Int(i * 24, 0),
                    TemplateMissing = template == null
                });
            }

            var edges = flow.Edges ?? new FlowEdge[0];
            for (var i = 0; i < edges.Length; i++)
            {
                var edge = edges[i];
                if (edge == null)
                    continue;

                plan.Links.Add(new RoomLink
                {
                    FromNodeId = edge.FromId ?? string.Empty,
                    ToNodeId = edge.ToId ?? string.Empty,
                    FromDoorId = "right",
                    ToDoorId = "left"
                });
            }

            return plan;
        }

        static RoomTemplate PickTemplate(IRandomService random, IReadOnlyList<RoomTemplate> catalog, FlowNode node)
        {
            if (catalog == null || catalog.Count == 0)
                return null;

            var matches = new List<RoomTemplate>();
            for (var i = 0; i < catalog.Count; i++)
            {
                var template = catalog[i];
                if (template == null || template.Purpose != node.Kind)
                    continue;
                if (!string.IsNullOrEmpty(template.BiomeId) &&
                    !string.IsNullOrEmpty(node.BiomeId) &&
                    template.BiomeId != node.BiomeId)
                    continue;
                matches.Add(template);
            }

            if (matches.Count == 0)
                return null;

            return random.Pick(matches);
        }
    }
}
