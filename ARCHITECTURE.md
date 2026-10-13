# 架构

侧视动作平台 + Roguelite。单局数据在 `RunState`，跨局数据在 `MetaState`。关卡是手制房间按概念图拼起来，不是运行时挖几何。

程序集用 Unity Assembly Definition，边界按目录生效：`Scripts/Project.Core.asmdef` 盖住 `Core`、`Run`、`Util`；其余每个目录自己的 asmdef 从 Core 里划出去。

## 模块边界

| 程序集 | 可以引用 | 放什么 | 不放什么 |
| --- | --- | --- | --- |
| Project.Core | 仅引擎 | 服务表、种子随机、时钟、对象池、场景名、`RunState`、`RunController` | UI、装备资产、房间资产、存档实现 |
| Project.Build | Core | `ItemDefinition`、标签、槽位、触发器数据、突变定义 | 扣血、生成子弹 |
| Project.Combat | Core、Build、Input System | 生命、攻击体、受击体、移动、翻滚、子弹、武器入口、敌人定义 | 存档、UI 控件、房间拼接 |
| Project.Generation | Core | 岛屿图、层概念图、房间模板、楼层草稿、连通性 | 读 Meta、实例化之外的战斗规则 |
| Project.Meta | Core | 元进度、JSON 存档、货币、解锁价目、安全点入账 | 装备资产类型、房间类型 |
| Project.UI | 上面全部 | HUD、三选一、结算的数据面 | 规则本身。UI 不改货币、不生成地图 |

禁止：

- Core 引用 UI、Combat、Build、Generation、Meta
- Build 引用 Combat（避免装备和打击互相引用）
- Generation 引用 Meta。符文是否拥有，用调用方传入的 `Func<string, bool>`
- Meta 引用 Build。解锁条目是字符串 Id，和 `ItemDefinition.Id` 对上即可
- 在 Tilemap、房间 Prefab 的杂项脚本里写道具规则

`ServiceLocator` 是一张主线程字典，不是 DI 框架。只登记时钟、随机数、`RunState`、`RunController`、`MetaState`。域重载时会清空。

标签 `ItemTag` 放在 Build。`DamageInfo` 会带上它，所以 Combat 引用 Build。以后若要在命中时回血，在 Core 加一个小接口，让 Build 的触发器调用接口，不要让 Build 去引用 `Health`。

## Run 和 Meta

| 数据 | 放哪 | 死亡 | 安全点 | 通关 |
| --- | --- | --- | --- | --- |
| 种子、层数、房间 Id | RunState | 种子保留，其余清空 | 保留 | 清空 |
| 金币、三色卷轴、身上的装备 Id | RunState | 清空 | 保留 | 清空 |
| 携带货币、携带蓝图 | RunState | 丢掉，不入账 | 货币入账，蓝图改记到「已发现」 | 同安全点，然后清空整局 |
| 已入账货币、解锁、符文、已发现蓝图、永久升级、难度标记、锻造等级 | MetaState | 不动 | 写入 JSON | 写入 JSON |

携带货币字段是 `RunState.CarriedCurrency`。已入账字段是 `MetaState.BankedCurrency`。设计上对应「带到安全点才算数」的那种元货币，代码和资产 Id 不用具体作品的名字。

难度标记 `DifficultyMarksOwned` 和 `ForgeQualityLevel` 只是存档字段，没有任何规则在读它们。

### 时序

`RunController` 先发 `RunEvents.Ended`，整轮回调结束之后才按原因 `Clear`。死亡和放弃没有人改 `RunState`，所以每个回调都能读到清空前的携带货币。安全点和通关由 `MetaBootstrap` 在回调里入账：它把携带额加进 `BankedCurrency` 然后把携带额归零。排在它后面的回调看到的是入账之后的数。

```
NotifyDeath / NotifyAbandon
  → 事件（Meta 什么都不做，不写盘）
  → RunState.Clear()

NotifySafePoint
  → 事件（MetaBootstrap 入账并 Save）
  → 本局继续，装备和卷轴还在

NotifyVictory
  → 事件（同样入账并 Save）
  → RunState.Clear()
```

结算 UI 如果要显示「这次丢掉了多少携带货币」，必须在事件回调里读，不能等 `NotifyDeath()` 返回。不要在死亡回调里再调 `RunSettlement.ApplyDeath`，控制器自己会清。

直接打开 `Run` 场景时没有 `MetaBootstrap`，事件没有监听者，安全点不会入账。这是手感周的预期。

`Clear()` 不清 `Seed`，方便死后对局。`BeginRun` 会先 Clear 再写入新种子，并 `Reseed` 随机服务。

## 生成管线

两张图，不要混：

1. **岛屿图** `IslandRouteGraph`：生物群系之间的固定有向边，可以要求符文。整局冒险选路用它，不随种子变化。
2. **层概念图** `FlowGraph`：进了某个生物群系之后，这一层有哪些 Start / Combat / Elite / Shop / Boss / Passage。`BiomeNode.FloorFlowId` 指向它。

```
种子 + FlowGraph + RoomTemplate 目录
        │
        ▼
FloorGenerator.Generate
  按节点用途和 BiomeId 过滤模板
  用 SeededRandom 挑一块（目录顺序算进种子，不要事先打乱）
  对不上就记 TemplateMissing，不重试
  按节点下标把房间排到 (i * 24, 0)
  边原样抄成 RoomLink，门 Id 固定写 right → left
        │
        ▼
FloorPlan（纯数据，还没有场景物体）
        │
        ▼
ConnectivityCheck
  从全部 Start 沿有向边 BFS
  必须有 Start、有 Boss，且每个节点都到达
  不看门的坐标
        │
        ▼
（还没写）按 FloorPlan 实例化 RoomPrefab，刷怪，卸掉上一层
```

`PlacementMode` 现在恒为 `LinearStub`。Week 2 换成门方向匹配时，失败应换模板重试，而不是改连通性检查的定义。检查回答的是「图通不通」，求解器回答的是「门对不对得上」。

房间 Prefab 工作流：

1. 在 `Prefabs/Rooms` 里摆碰撞、地面（Ground 层）、出怪点空物体。
2. 原点放在房间左下或几何中心，全项目统一，并写在模板注释里。当前示例把原点当房间中心，左右门在 `x = ±8`。
3. 建 `RoomTemplate` 资产：用途、生物群系、`SizeInTiles`、`DoorAnchor`（本地坐标 + 朝向）、出怪点、玩家出生点，`RoomPrefab` 指向上一步。
4. 生成器在求解器写好之前不会读这些坐标。锚点是给下一步用的合同，不是装饰字段。

出怪点只是数据。敌人定义在 `EnemyDefinition`，没有行为。

## 构筑

槽位数量在 `LoadoutLimits`：2 武器 + 2 技能。双手武器占满两个武器槽，实现在 `InventorySlots`。

`RunState` 只存字符串 Id。`InventorySlots` 持有 `ItemDefinition` 引用，用 `CopyIdsTo` 写回局内状态。Meta 的解锁表也只用字符串，避免 Meta 依赖 Build。

协同以后只认 `ItemTag`（Poison、OnHit、ProjectileCount、Melee、Ranged、Burn、Freeze、Area）。`EffectDispatcher.CountMatches` 现在只计数。条件暴击、弹数增加都不要写成「这件武器特判那件武器」。

三选一由 `DraftPresenter` 持有选项。它不暂停时间。打开面板的人调用 `GameClock.Pause()`，关掉再 `Resume()`。`timeScale` 会停物理；翻滚计时用 `Time.deltaTime`，暂停时无敌也不会偷偷走完。

## 随机和存档

`SeededRandom` 是 xorshift32，不读 `UnityEngine.Random`。同一种子、同一调用顺序，结果不跟 .NET 版本走。它不是网络协议，也没做加密。

`MetaSaveService` 用 `JsonUtility`。能存的只有公共字段、`List<string>` 和标了 `[Serializable]` 的类。不要往 `MetaState` 里加字典或属性，否则写进去也读不回来。版本号是 `MetaState.Version`，当前为 1。比工程新的文件会留下并打警告，不会覆盖。

## 战斗占位

伤害路径：`Hitbox` 或 `Projectile` → `Hurtbox.Receive` → 同阵营丢弃 → 父节点上的 `RollAbility` 若在无敌帧则丢弃 → `Health.Apply`。

层矩阵再挡一层友军碰撞，见 `Settings/Layers.md`。两处都要在，单靠一边会在有人改层时误伤。

`PlayerMotor2D` 只做水平速度和落地跳。`RollAbility` 给一段无敌，并沿朝向写一次横向速度。数字是占位，不是目标手感。

## 新功能往哪放

| 要做的事 | 放哪 |
| --- | --- |
| 土狼时间、跳跃缓冲、下平台 | `PlayerMotor2D` |
| 攻击取消进翻滚 | `WeaponController` 与 `RollAbility` 之间，仍在 Combat |
| 近战挥砍、远程发射 | `WeaponController` |
| 子弹回收 | Combat 里用 Core 的 `ObjectPool<T>` 包一层组件池 |
| 门匹配求解 | 替换 `FloorGenerator` 的摆放，保留 `FloorPlan` |
| 刷房间、卸层 | Generation 里新的场景装载器，不要写进 `RunController` |
| 安全点按钮 | Hub 或 Run 里的物体调用 `RunController.NotifySafePoint` |
| 花钱解锁 | `UnlockService.TryPurchase` 之后 `MetaSaveService.Save` |
| 血条、三选一面板 | UI 程序集，读 Presenter，不复制规则 |
| 跟随相机 | Cinemachine，挂在 Run 场景，不要写进 Motor |
