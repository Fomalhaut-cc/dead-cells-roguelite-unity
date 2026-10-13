namespace Project.Core
{
    /// <summary>
    /// 构筑槽位数量。Core 与 Build 共用，避免两个程序集互相引用。
    /// </summary>
    public static class LoadoutLimits
    {
        public const int Weapons = 2;
        public const int Skills = 2;
    }
}
