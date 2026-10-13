using System.Collections.Generic;
using System.Text;

namespace Project.Generation
{
    /// <summary>
    /// 沿有向链接做 BFS。要求存在起点和 Boss，且每个节点都能从某个起点走到。
    /// 不检查门的坐标是否对得上。
    /// </summary>
    public static class ConnectivityCheck
    {
        public static bool TryExplain(FloorPlan plan, out string report)
        {
            if (plan == null)
            {
                report = "没有楼层计划。";
                return false;
            }

            var rooms = plan.Rooms ?? new List<PlacedRoom>();
            var starts = new List<string>();
            var bosses = new List<string>();
            var known = new HashSet<string>();
            for (var i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (room == null || string.IsNullOrEmpty(room.NodeId) || !known.Add(room.NodeId))
                    continue;
                if (room.Kind == RoomKind.Start)
                    starts.Add(room.NodeId);
                if (room.Kind == RoomKind.Boss)
                    bosses.Add(room.NodeId);
            }

            var builder = new StringBuilder();
            if (starts.Count == 0)
            {
                report = "没有起点节点。";
                return false;
            }

            if (bosses.Count == 0)
            {
                report = "没有 Boss 节点。";
                return false;
            }

            var adjacency = new Dictionary<string, List<string>>();
            var links = plan.Links ?? new List<RoomLink>();
            for (var i = 0; i < links.Count; i++)
            {
                var link = links[i];
                if (link == null || string.IsNullOrEmpty(link.FromNodeId) || string.IsNullOrEmpty(link.ToNodeId))
                    continue;
                List<string> next;
                if (!adjacency.TryGetValue(link.FromNodeId, out next))
                {
                    next = new List<string>();
                    adjacency.Add(link.FromNodeId, next);
                }

                next.Add(link.ToNodeId);
            }

            var reached = new HashSet<string>();
            var queue = new Queue<string>();
            for (var i = 0; i < starts.Count; i++)
            {
                if (reached.Add(starts[i]))
                    queue.Enqueue(starts[i]);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                List<string> next;
                if (!adjacency.TryGetValue(current, out next))
                    continue;
                for (var i = 0; i < next.Count; i++)
                {
                    if (reached.Add(next[i]))
                        queue.Enqueue(next[i]);
                }
            }

            var unreachable = new List<string>();
            foreach (var id in known)
            {
                if (!reached.Contains(id))
                    unreachable.Add(id);
            }

            var bossesReached = true;
            for (var i = 0; i < bosses.Count; i++)
            {
                if (!reached.Contains(bosses[i]))
                    bossesReached = false;
            }

            builder.Append("起点 ");
            builder.Append(starts.Count);
            builder.Append("，到达 ");
            builder.Append(reached.Count);
            builder.Append('/');
            builder.Append(known.Count);
            if (unreachable.Count > 0)
            {
                unreachable.Sort(System.StringComparer.Ordinal);
                builder.Append("，不可达：");
                builder.Append(string.Join(", ", unreachable.ToArray()));
            }

            report = builder.ToString();
            return bossesReached && unreachable.Count == 0;
        }
    }
}
