using Project.Core;
using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 受击体。同阵营忽略。翻滚无敌帧在这里被丢掉，不改动画。
    /// </summary>
    public sealed class Hurtbox : MonoBehaviour
    {
        [SerializeField] CombatTeam team = CombatTeam.Player;
        [SerializeField] Health health;

        public CombatTeam Team { get { return team; } }

        void Reset()
        {
            ApplyLayer();
        }

        void Awake()
        {
            ApplyLayer();
            if (health == null)
                health = GetComponentInParent<Health>();
        }

        public void Receive(DamageInfo info)
        {
            if (info.Team == team)
                return;

            var roll = GetComponentInParent<RollAbility>();
            if (roll != null && roll.IsInvulnerable)
                return;

            if (health == null)
                health = GetComponentInParent<Health>();
            if (health == null)
                return;

            health.Apply(info);
        }

        void ApplyLayer()
        {
            var layerName = team == CombatTeam.Player ? GameLayers.PlayerHurtbox : GameLayers.EnemyHurtbox;
            var index = LayerMask.NameToLayer(layerName);
            if (index >= 0)
                gameObject.layer = index;
        }
    }
}
