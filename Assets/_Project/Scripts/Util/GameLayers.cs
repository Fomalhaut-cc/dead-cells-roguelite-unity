namespace Project.Core
{
    /// <summary>
    /// 与 ProjectSettings/TagManager 里的层名一致。改名时两处一起改。
    /// 碰撞矩阵见 Settings/Layers.md。
    /// </summary>
    public static class GameLayers
    {
        public const string Player = "Player";
        public const string Enemy = "Enemy";
        public const string PlayerHurtbox = "PlayerHurtbox";
        public const string EnemyHurtbox = "EnemyHurtbox";
        public const string PlayerHitbox = "PlayerHitbox";
        public const string EnemyHitbox = "EnemyHitbox";
        public const string PlayerProjectile = "PlayerProjectile";
        public const string EnemyProjectile = "EnemyProjectile";
        public const string Ground = "Ground";
        public const string OneWayPlatform = "OneWayPlatform";
        public const string Pickup = "Pickup";
        public const string RoomTrigger = "RoomTrigger";
    }
}
