using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 翻滚占位。无敌时间和位移都是临时数字，不是手感目标。
    /// 无敌期间 <see cref="PlayerMotor2D"/> 保持 <see cref="LockedHorizontalSpeed"/>，避免下一帧把速度盖掉。
    /// 没有穿怪、压低碰撞箱，也没有攻击后摇取消。
    /// </summary>
    public sealed class RollAbility : MonoBehaviour
    {
        [SerializeField] float duration = 0.25f;
        [SerializeField] float cooldown = 0.45f;
        [SerializeField] float rollSpeed = 12f;

        float invulnerableUntil;
        float readyAt;

        public bool IsInvulnerable { get; private set; }
        public bool IsCoolingDown { get { return Time.time < readyAt && !IsInvulnerable; } }
        public float LockedHorizontalSpeed { get; private set; }

        void Update()
        {
            if (IsInvulnerable && Time.time >= invulnerableUntil)
                IsInvulnerable = false;
        }

        public bool TryRoll(Vector2 direction)
        {
            if (IsInvulnerable || Time.time < readyAt)
                return false;

            if (direction.sqrMagnitude < 0.0001f)
                direction = Vector2.right;
            direction.Normalize();

            IsInvulnerable = true;
            invulnerableUntil = Time.time + duration;
            readyAt = Time.time + duration + cooldown;
            LockedHorizontalSpeed = direction.x * rollSpeed;

            var body = GetComponent<Rigidbody2D>();
            if (body != null)
            {
                var velocity = Rigidbody2DCompat.GetVelocity(body);
                velocity.x = direction.x * rollSpeed;
                Rigidbody2DCompat.SetVelocity(body, velocity);
            }

            return true;
        }
    }
}
