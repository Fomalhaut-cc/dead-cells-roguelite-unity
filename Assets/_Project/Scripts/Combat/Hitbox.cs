using Project.Build;
using Project.Core;
using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 攻击体。Collider2D 需要勾 Is Trigger。只在进入时结算一次。
    /// </summary>
    public sealed class Hitbox : MonoBehaviour
    {
        [SerializeField] CombatTeam team = CombatTeam.Player;
        [SerializeField] int damage = 10;
        [SerializeField] string sourceId = "hitbox";
        [SerializeField] ItemTag tags = ItemTag.Melee;
        [SerializeField] float knockback = 2f;

        public bool EnabledHit { get; private set; }

        void Reset()
        {
            var collider2d = GetComponent<Collider2D>();
            if (collider2d != null)
                collider2d.isTrigger = true;
            ApplyLayer();
        }

        void Awake()
        {
            ApplyLayer();
            SetEnabledHit(false);
        }

        public void SetEnabledHit(bool enabledHit)
        {
            EnabledHit = enabledHit;
            var collider2d = GetComponent<Collider2D>();
            if (collider2d != null)
                collider2d.enabled = enabledHit;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!EnabledHit || other == null)
                return;

            var hurt = other.GetComponent<Hurtbox>();
            if (hurt == null || hurt.Team == team)
                return;

            var direction = (Vector2)(other.transform.position - transform.position);
            if (direction.sqrMagnitude < 0.0001f)
                direction = Vector2.right;

            hurt.Receive(new DamageInfo
            {
                Amount = damage,
                SourceId = sourceId,
                Tags = tags,
                Knockback = direction.normalized * knockback,
                IsCritical = false,
                Team = team
            });
        }

        void ApplyLayer()
        {
            var layerName = team == CombatTeam.Player ? GameLayers.PlayerHitbox : GameLayers.EnemyHitbox;
            var index = LayerMask.NameToLayer(layerName);
            if (index >= 0)
                gameObject.layer = index;
        }
    }
}
