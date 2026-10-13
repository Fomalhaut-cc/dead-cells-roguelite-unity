# 层和 2D 碰撞

层名定义在 `ProjectSettings/TagManager.asset`，代码常量在 `GameLayers`。改名要两处一起改，否则 `LayerMask.NameToLayer` 会得到 -1，组件就不再自动设层。

索引：

| 索引 | 层 | 用途 |
| --- | --- | --- |
| 0 | Default | 场景杂物 |
| 5 | UI | 不参与 2D 物理 |
| 6 | Player | 角色身体，撞地面，不撞敌人 |
| 7 | Enemy | 敌人身体，撞地面，不撞玩家 |
| 8 | PlayerHurtbox | 玩家受击 |
| 9 | EnemyHurtbox | 敌人受击 |
| 10 | PlayerHitbox | 玩家攻击，默认关闭，挥砍时打开 |
| 11 | EnemyHitbox | 敌人攻击 |
| 12 | PlayerProjectile | 玩家子弹 |
| 13 | EnemyProjectile | 敌人子弹 |
| 14 | Ground | 地面 |
| 15 | OneWayPlatform | 单向平台。穿过逻辑还没写，矩阵先按地面处理 |
| 16 | Pickup | 只碰玩家身体 |
| 17 | RoomTrigger | 进出房间，只碰玩家身体 |

18 到 31 层是空的，矩阵里默认不和任何层碰撞。新加层之后要到 **Project Settings → Physics 2D → Layer Collision Matrix** 里勾上，否则触发器不会响。

允许碰撞的组合（其余都关掉）：

- Default、TransparentFX、Ignore Raycast、Water、Ground、OneWayPlatform 彼此碰撞
- 上述世界层与 Player、Enemy 碰撞
- Enemy 与 Enemy 碰撞
- PlayerHurtbox 只碰 EnemyHitbox、EnemyProjectile
- EnemyHurtbox 只碰 PlayerHitbox、PlayerProjectile
- 两种子弹只额外碰 Ground、OneWayPlatform、Default
- Pickup、RoomTrigger 只碰 Player

玩家子弹不碰玩家，敌人子弹不碰敌人。`Hurtbox` 还会再看一次 `CombatTeam`，防止矩阵被改乱以后友军互伤。

攻击体和子弹的 Collider2D 要勾 **Is Trigger**。`Hitbox` 在 `Awake` 里会把自己关掉，挥砍代码负责 `SetEnabledHit(true)`。

重力是 `(0, -30)`，在 `Physics2DSettings.asset`。这是侧视起步值，不是手感结论。`PlayerMotor2D.gravityScale` 保持 1，不要两处一起乘。

排序层：Default、Background、Midground、Actor、Vfx。角色用 Actor，房间背景用 Background。
