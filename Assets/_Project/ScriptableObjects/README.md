# 示例资产

这些 `.asset` 是文本 YAML，脚本 GUID 和 `Scripts` 下的 `.meta` 对齐。用 Unity 打开后应能直接在 Inspector 里看到字段。若脚本显示 Missing，不要另存一份盖掉 GUID，先确认包已经导入、Console 里程序集没有编译错误。

| 资产 | 类型 | 说明 |
| --- | --- | --- |
| Items/AshBlade | ItemDefinition | 近战，Melee + OnHit |
| Items/SplinterBow | ItemDefinition | 远程，Ranged + ProjectileCount |
| Items/CinderFlask | ItemDefinition | 技能，Poison + Area |
| Enemies/CorridorWalker | EnemyDefinition | 一个走路占位，没有 AI |
| Rooms/NarrowCombat | RoomTemplate | 左右门锚点。Prefab 仍是空引用 |
| Flows/NarrowGuttersFlow | FlowGraph | 起点、两间战斗、商店、精英、Boss |
| Mutations/OpenWound | MutationDefinition | 被动占位，没有效果 |
| Routes/StarterFork | IslandRouteGraph | 三岔里有一条边要求 `rune.vault` |
| Meta/StarterUnlocks | UnlockCatalog | 五条解锁，用字符串 Id |

新建资产用 Project 窗口的 Create 菜单，见仓库根目录 README。不要手改 YAML，除非只是改 Id 或数字；Unity 重新序列化之后字段顺序会变，这是正常的。

`NarrowCombat` 的 `BiomeId` 是 `narrow_gutters`，用途是 Combat。概念图里的商店、精英、Boss、起点对不上这块模板，生成结果里会标 `TemplateMissing`。先复制这块资产再改用途，不要把一间房的用途改成「什么都能当」。

岛屿图和层概念图不是同一张图。选路看 `Routes/StarterFork`，进了窄渠之后的房间序列看 `Flows/NarrowGuttersFlow`。
