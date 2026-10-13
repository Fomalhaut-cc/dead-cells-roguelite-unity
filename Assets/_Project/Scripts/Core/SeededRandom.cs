using System;
using System.Collections.Generic;

namespace Project.Core
{
    /// <summary>
    /// xorshift32。同一种子、同一调用顺序，结果与 .NET 版本无关。
    /// 取模用了拒绝采样，避免小范围时的偏差。不是加密随机。
    /// </summary>
    public sealed class SeededRandom : IRandomService
    {
        uint state;

        public int Seed { get; private set; }

        public SeededRandom(int seed)
        {
            Reseed(seed);
        }

        public void Reseed(int seed)
        {
            Seed = seed;
            state = unchecked((uint)seed);
            if (state == 0)
                state = 1;
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
                throw new ArgumentException("maxExclusive must be greater than minInclusive.");

            var range = (uint)(maxExclusive - minInclusive);
            var limit = uint.MaxValue - (uint.MaxValue % range);
            uint value;
            do
            {
                value = NextUInt();
            }
            while (value >= limit);

            return minInclusive + (int)(value % range);
        }

        public float NextFloat()
        {
            return (NextUInt() & 0x00FFFFFFu) / 16777216f;
        }

        public float Range(float minInclusive, float maxExclusive)
        {
            if (maxExclusive <= minInclusive)
                throw new ArgumentException("maxExclusive must be greater than minInclusive.");

            return minInclusive + (maxExclusive - minInclusive) * NextFloat();
        }

        public T Pick<T>(IReadOnlyList<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (items.Count == 0)
                throw new InvalidOperationException("Cannot pick from an empty list.");

            return items[NextInt(0, items.Count)];
        }

        uint NextUInt()
        {
            var x = state;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            state = x;
            return x;
        }
    }
}
