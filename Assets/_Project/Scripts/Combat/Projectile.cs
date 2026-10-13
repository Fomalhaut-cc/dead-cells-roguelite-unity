using Project.Build;
using Project.Core;
using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 直线运动的触发器子弹。碰到受击体或地面后关闭。
    /// 对象池在 <see cref="ObjectPool{T}"/>，这里先 SetActive(false)。
    /// </summary>
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 14f;
        [SerializeField] int damage = 6;
        [SerializeField] CombatTeam team = CombatTeam.Player;
        [SerializeField] ItemTag tags = ItemTag.Ranged;
        [SerializeField] string sourceId = "projectile";

        Vector2 direction = Vector2.right;
        bool alive;

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
        }

        public void Launch(Vector2 newDirection, int damageAmount, string newSourceId)
        {
            direction = newDirection.sqrMagnitude < 0.0001f ? Vector2.right : newDirection.normalized;
            if (damageAmount > 0)
                damage = damageAmount;
            if (!string.IsNullOrEmpty(newSourceId))
                sourceId = newSourceId;
            alive = true;
            gameObject.SetActive(true);
        }

        void Update()
        {
            if (!alive)
                return;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!alive || other == null)
                return;

            var hurt = other.GetComponent<Hurtbox>();
            if (hurt != null)
            {
                if (hurt.Team == team)
                    return;

                hurt.Receive(new DamageInfo
                {
                    Amount = damage,
                    SourceId = sourceId,
                    Tags = tags,
                    Knockback = direction * 1.5f,
                    IsCritical = false,
                    Team = team
                });
                Despawn();
                return;
            }

            Despawn();
        }

        public void Despawn()
        {
            alive = false;
            gameObject.SetActive(false);
        }

        void ApplyLayer()
        {
            var layerName = team == CombatTeam.Player ? GameLayers.PlayerProjectile : GameLayers.EnemyProjectile;
            var index = LayerMask.NameToLayer(layerName);
            if (index >= 0)
                gameObject.layer = index;
        }
    }
}
