using UnityEngine;

namespace Project.Combat
{
    /// <summary>
    /// Unity 6 把速度改名为 linearVelocity，2022 LTS 仍叫 velocity。
    /// </summary>
    public static class Rigidbody2DCompat
    {
        public static Vector2 GetVelocity(Rigidbody2D body)
        {
#if UNITY_6000_0_OR_NEWER
            return body.linearVelocity;
#else
            return body.velocity;
#endif
        }

        public static void SetVelocity(Rigidbody2D body, Vector2 velocity)
        {
#if UNITY_6000_0_OR_NEWER
            body.linearVelocity = velocity;
#else
            body.velocity = velocity;
#endif
        }
    }
}
