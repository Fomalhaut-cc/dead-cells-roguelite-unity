using System.Collections.Generic;
using Project.Build;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// 三选一的数据面。不暂停游戏，调用方自己用 GameClock.Pause。
    /// 选项不是 3 个时仍打开，并打一条警告。
    /// </summary>
    public sealed class DraftPresenter : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public IReadOnlyList<ItemDefinition> Choices { get; private set; }

        public void Show(IReadOnlyList<ItemDefinition> choices)
        {
            if (choices == null || choices.Count != 3)
                Debug.LogWarning("三选一期望正好 3 个选项。", this);

            Choices = choices;
            IsOpen = choices != null && choices.Count > 0;
        }

        public ItemDefinition Choose(int index)
        {
            if (!IsOpen || Choices == null || index < 0 || index >= Choices.Count)
                return null;

            var picked = Choices[index];
            IsOpen = false;
            return picked;
        }

        public void Close()
        {
            IsOpen = false;
        }
    }
}
