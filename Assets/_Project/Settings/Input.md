# 输入

动作表：`Player.inputactions`。`PlayerInputReader` 只读 **Gameplay** 这张图。资产没挂到组件上时，读入器直接返回。

| 动作 | 键盘鼠标 | 手柄 |
| --- | --- | --- |
| Move | WASD，方向键 | 左摇杆 |
| Jump | Space | 南键 |
| Roll | Left Shift | 东键 |
| Attack | 鼠标左键，J | 西键 |
| SkillA | Q | 右肩键 |
| SkillB | E | 左肩键 |
| Pause | Esc | Start |

Pause 和 **UI** 图（Navigate / Submit / Cancel）已经写在资产里，没有代码去读。暂停以后接到 `GameClock`。

工程的 Player Settings 使用 Both，旧输入和新输入都能用。新脚本不要再写 `UnityEngine.Input`。

控制方案有 KeyboardMouse 和 Gamepad。左摇杆绑定带了 `stickDeadzone`。
