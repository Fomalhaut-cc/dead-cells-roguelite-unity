using System.IO;
using UnityEngine;

namespace Project.Meta
{
    /// <summary>
    /// 把 <see cref="MetaState"/> 写成 persistentDataPath 下的 JSON。
    /// Unity 的 JsonUtility 只认字段，不认属性和字典。
    /// </summary>
    public static class MetaSaveService
    {
        public const string FileName = "meta-save.json";

        public static string SavePath
        {
            get { return Path.Combine(Application.persistentDataPath, FileName); }
        }

        public static bool Save(MetaState state)
        {
            if (state == null)
                return false;

            Normalize(state);
            if (state.Version < MetaState.CurrentVersion)
                state.Version = MetaState.CurrentVersion;

            try
            {
                var json = JsonUtility.ToJson(state, true);
                File.WriteAllText(SavePath, json);
                return true;
            }
            catch (IOException exception)
            {
                Debug.LogError("写入元进度失败：" + exception.Message);
                return false;
            }
        }

        public static MetaState LoadOrCreate()
        {
            string error;
            MetaState state;
            if (!TryLoad(out state, out error))
            {
                if (!string.IsNullOrEmpty(error))
                    Debug.LogWarning(error);
                state = new MetaState();
            }

            Normalize(state);
            return state;
        }

        public static bool TryLoad(out MetaState state, out string error)
        {
            state = null;
            error = null;
            if (!File.Exists(SavePath))
            {
                state = new MetaState();
                return true;
            }

            try
            {
                var json = File.ReadAllText(SavePath);
                state = JsonUtility.FromJson<MetaState>(json);
            }
            catch (IOException exception)
            {
                error = "读取元进度失败：" + exception.Message;
                return false;
            }

            if (state == null)
            {
                error = "元进度文件无法解析，已忽略。";
                return false;
            }

            if (state.Version > MetaState.CurrentVersion)
            {
                error = "存档版本 " + state.Version + " 比工程新，未覆盖文件。";
                Normalize(state);
                return true;
            }

            Migrate(state);
            Normalize(state);
            return true;
        }

        static void Migrate(MetaState state)
        {
            // Version 1 没有更早的格式。以后在这里按旧版本补字段。
            if (state.Version < MetaState.CurrentVersion)
                state.Version = MetaState.CurrentVersion;
        }

        public static void Normalize(MetaState state)
        {
            if (state.UnlockedItemIds == null)
                state.UnlockedItemIds = new System.Collections.Generic.List<string>();
            if (state.UnlockedMutationIds == null)
                state.UnlockedMutationIds = new System.Collections.Generic.List<string>();
            if (state.OwnedRuneIds == null)
                state.OwnedRuneIds = new System.Collections.Generic.List<string>();
            if (state.DiscoveredBlueprintIds == null)
                state.DiscoveredBlueprintIds = new System.Collections.Generic.List<string>();
            if (state.PurchasedUpgradeIds == null)
                state.PurchasedUpgradeIds = new System.Collections.Generic.List<string>();
            if (state.Version <= 0)
                state.Version = MetaState.CurrentVersion;
        }
    }
}
