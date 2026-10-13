using Project.Core;
using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// 侧视移动占位：水平速度、落地后跳。
    /// 没有土狼时间、跳跃缓冲、可变跳高、下平台或攻击取消。
    /// 重力在 Project Settings → Physics 2D，不要在这里再乘一套。
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMotor2D : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 8f;
        [SerializeField] float jumpSpeed = 16f;
        [SerializeField] float groundCheckDistance = 0.15f;
        [SerializeField] LayerMask groundMask;

        Rigidbody2D body;
        RollAbility roll;
        float moveAxis;
        bool jumpRequested;

        public bool IsGrounded { get; private set; }

        void Reset()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.freezeRotation = true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
            rigidbody.gravityScale = 1f;
        }

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            roll = GetComponent<RollAbility>();
            body.freezeRotation = true;
            if (groundMask.value == 0)
                groundMask = LayerMask.GetMask(GameLayers.Ground, GameLayers.OneWayPlatform);
        }

        public void SetMoveAxis(float axis)
        {
            moveAxis = Mathf.Clamp(axis, -1f, 1f);
        }

        public void RequestJump()
        {
            jumpRequested = true;
        }

        /// <summary>单向平台穿过。还没做。</summary>
        public void RequestDropThrough()
        {
        }

        void FixedUpdate()
        {
            IsGrounded = CheckGrounded();
            var velocity = Rigidbody2DCompat.GetVelocity(body);
            if (roll != null && roll.IsInvulnerable)
                velocity.x = roll.LockedHorizontalSpeed;
            else
                velocity.x = moveAxis * moveSpeed;
            if (jumpRequested && IsGrounded)
                velocity.y = jumpSpeed;
            jumpRequested = false;
            Rigidbody2DCompat.SetVelocity(body, velocity);
        }

        bool CheckGrounded()
        {
            var origin = (Vector2)transform.position;
            var hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundMask);
            return hit.collider != null;
        }
    }
}
