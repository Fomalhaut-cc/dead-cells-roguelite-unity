using UnityEngine;

namespace Project.Build
{
    /// <summary>
    /// 被动突变的静态定义。没有效果逻辑。安全点最多装备几个，由 Meta 以后决定。
    /// </summary>
    [CreateAssetMenu(fileName = "Mutation", menuName = "Project/Build/Mutation Definition")]
    public sealed class MutationDefinition : ScriptableObject
    {
        public string Id = "mutation.unnamed";
        public string DisplayName = "未命名突变";
        [TextArea(2, 4)] public string Description = string.Empty;
        public ItemTag RequiredTags = ItemTag.None;
        public int MaxRank = 1;
    }
}
