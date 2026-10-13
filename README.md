# Roguevania 工程骨架

侧视 2D 动作平台，加上单局死亡清空、局外解锁保留的 Roguelite 结构。这是给小团队开工用的 Unity 工程骨架，不是可玩版本：没有手感调参、没有敌人 AI、没有真正的房间拼装求解器，也没有美术和音频。

视角是横版，不是俯视。关卡路线采用「固定岛屿图 + 每层概念图 + 手制房间块」，不要做成纯随机挖洞。

## 用哪个 Unity 打开

用 **Unity 6000.x**（Unity 6）打开。`ProjectSettings/ProjectVersion.txt` 写的是 `6000.0.58f2`，Hub 里任意 6000.0 / 6000.2 及以上都可以，打开后让编辑器改写版本号。

脚本只使用 2022 LTS 和 Unity 6 都有的 API。`Rigidbody2D` 的速度在 Unity 6 叫 `linearVelocity`，工程里用编译分支兼容 2022。若必须用 **2022.3 LTS**，Package Manager 会拒绝当前这组包版本，需要把 `Packages/manifest.json` 改成那条线能解析的版本：URP 14.x、uGUI 1.0.0、Cinemachine 2.10.x、Input System 1.7 以上。不要在 2022 上硬装 URP 17。

## 打开步骤

1. 安装 Unity 6000.x，Hub 里 Add project，选本目录（含 `Assets` 和 `Packages` 的这一层）。
2. 等 Package Manager 拉完包。若编辑器询问输入后端，选 **Both**（工程里 `activeInputHandler` 已是 Both）。
3. 第一次打开若画面全粉，或 Console 提示没有渲染管线资产：菜单 **Create → Rendering → URP Asset (with 2D Renderer)**，把生成的 URP Asset 拖到 **Project Settings → Graphics → Scriptable Render Pipeline Settings**，并指定到 Quality 里正在用的档。不指定时，Sprite 仍可能用内置管线显示，但 2D 光不可用。Cinemachine 已装上，工程里还没有虚拟相机。
4. 打开 `Assets/_Project/Scenes/Bootstrap.unity`。Build Settings 里它是第 0 个场景。按 Play 只会留下服务对象，不会自动进入 Hub。
5. 手感周不要从 Bootstrap 测。直接打开 `Assets/_Project/Scenes/Run.unity` 做角色。

元进度文件在运行后出现，路径是 `Application.persistentDataPath/meta-save.json`（编辑器里一般在本机 `~/.config/unity3d/Studio/Roguevania/`）。

## 目录

```
Assets/
  Plugins/                         空，预编译插件以后放这里
  _Project/
    Scenes/                        Bootstrap, Boot, Hub, Run
    Prefabs/                       Player, Enemies, Rooms, UI, Vfx（空）
    Art/Placeholders/              空。先灰盒，不要贴外部作品的图
    Audio/                         空
    Settings/                      输入动作表、层说明
    ScriptableObjects/
      Items/  Enemies/  Rooms/  Flows/  Mutations/
      Routes/                      固定岛屿图（相对概念图多出来的一层）
      Meta/                        解锁价目
    Scripts/
      Core/  Run/  Util/           程序集 Project.Core
      Combat/                      Project.Combat
      Generation/                  Project.Generation
      Build/                       Project.Build
      Meta/                        Project.Meta
      UI/                          Project.UI
```

场景职责：

| 场景 | 作用 |
| --- | --- |
| Bootstrap | 时钟、随机数、读取元进度。切场景不销毁 |
| Boot | 进 Hub 的空场景，不会自己跳转 |
| Hub | 局外。出发进 Run 的按钮以后挂在这里 |
| Run | 侧视单局。手感、房间、战斗都在这里测 |

## 程序集

`Project.Core` 不引用其他工程程序集，也不引用 UI。依赖方向：

```
Core
├── Build
├── Combat → Build、Input System
├── Generation
├── Meta
└── UI → Core、Combat、Build、Generation、Meta
```

细分、死亡时谁清空，见 [ARCHITECTURE.md](ARCHITECTURE.md)。

## 示例数据

资产是文本 YAML，已经放在 `ScriptableObjects` 里，可以用 Create 菜单再做新的。菜单名：

- Project / Build / Item Definition
- Project / Build / Mutation Definition
- Project / Combat / Enemy Definition
- Project / Generation / Room Template
- Project / Generation / Flow Graph
- Project / Generation / Island Route
- Project / Meta / Unlock Catalog

说明见 `Assets/_Project/ScriptableObjects/README.md`。

`Run` 场景上的 `RunFloorDriver` 已接好示例概念图。运行中或在组件菜单里执行 **Generate Stub Floor**，Console 会打印连通性结果。那一步只是把节点排成一行，不是房间求解器。

## Week 1：只做手感

目标是一个人在 `Run` 场景里跑、跳、翻滚，并打出一次近战或一发子弹。不要做地图生成，不要做解锁界面。

按这个顺序改：

1. `PlayerMotor2D`、`RollAbility`。在 `Run` 里放一个 **Ground** 层的碰撞体，把角色根节点放在脚底附近（地面检测从 Transform 往下打）。重力在 **Project Settings → Physics 2D**，当前是 `(0, -30)`，和脚本里的 `jumpSpeed` 一起调。
2. 新建 `Assets/_Project/Prefabs/Player/Player.prefab`，挂上 `Rigidbody2D`、`Collider2D`、`PlayerMotor2D`、`RollAbility`、`Health`、`Hurtbox`、`WeaponController`、`PlayerInputReader`。输入资产用 `Settings/Input/Player.inputactions`。
3. `WeaponController.TryPrimaryAttack`：近战就短时间打开子物体上的 `Hitbox`（默认是关的），远程就 `Instantiate` 一个带 `Projectile` 的物体并 `Launch`。
4. 确认层和矩阵没被改乱，见 `Assets/_Project/Settings/Layers.md`。玩家身体不和敌人身体相撞，伤害只走 Hurtbox / Hitbox。

手感过不了关，就不要加第二把武器。

## Week 2：一条能走完的层

1. 按 `RoomTemplate` 的门锚点做 8 到 12 个房间 Prefab，补上 Start / Shop / Elite / Boss 模板。现在只有一间战斗房，别的节点会标成缺模板。
2. 替换 `FloorGenerator` 里的 `LinearStub`：按门方向对接，对不上就换一块再试。连通性检查留着，它只保证图上走得到 Boss。
3. 做三选一 UI，读 `DraftPresenter`。打开时调用 `GameClock.Pause()`。
4. Hub 里用 `UnlockService.TryPurchase`，成功后 `MetaSaveService.Save`。安全点调用 `RunController.NotifySafePoint()`，死亡调用 `NotifyDeath()`。不要在死亡回调里入账。

## 故意空着的部分

- 敌人 AI、Boss 阶段、受击硬直、击退的实际位移
- 攻击取消翻滚、下平台、土狼时间、跳跃缓冲
- 条件暴击、卷轴乘区、锻造权重、难度标记的规则（字段在，逻辑不在）
- 门对门的楼层求解、瓦片地图刷写、小地图
- 角色和房间 Prefab、动画、特效、音效
- HUD / 三选一 / 结算的控件（只有字符串和选项数据）
- Cinemachine 跟随相机
- 联机、账号、加密存档

存档没有做崩溃原子写。写到一半杀进程可能得到半截 JSON，下次会当损坏文件丢掉并新建。

## 本地脚本怎么编

本仓库不带 Unity 编辑器。C# 要等 Hub 打开工程后由编辑器编译。`Project.Combat` 依赖 Input System 包，包没拉下来之前 Combat 会报找不到命名空间，这是预期现象。
