using System.Collections.Generic;

namespace Project.Core
{
    /// <summary>
    /// 本局随机数。实现必须只吃种子，不能读 <c>UnityEngine.Random</c>。
    /// </summary>
    public interface IRandomService
    {
        int Seed { get; }

        void Reseed(int seed);

        /// <summary>返回 <c>[minInclusive, maxExclusive)</c>。</summary>
        int NextInt(int minInclusive, int maxExclusive);

        /// <summary>返回 <c>[0, 1)</c>。</summary>
        float NextFloat();

        float Range(float minInclusive, float maxExclusive);

        T Pick<T>(IReadOnlyList<T> items);
    }
}
