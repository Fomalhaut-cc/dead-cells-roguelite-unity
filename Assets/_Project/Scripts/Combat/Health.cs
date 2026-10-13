using System;
using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 当前生命。受伤直接扣数字，死亡发 <see cref="Died"/>。
    /// </summary>
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] int max = 100;

        public int Max { get { return max; } }
        public int Current { get; private set; }
        public bool IsDead { get { return Current <= 0; } }

        public event Action<DamageInfo> Damaged;
        public event Action Died;

        void Awake()
        {
            Current = max;
        }

        public void Apply(DamageInfo info)
        {
            if (IsDead || info.Amount <= 0)
                return;

            Current = Mathf.Max(0, Current - info.Amount);
            var damaged = Damaged;
            if (damaged != null)
                damaged(info);

            if (Current == 0)
            {
                var died = Died;
                if (died != null)
                    died();
            }
        }
    }
}
