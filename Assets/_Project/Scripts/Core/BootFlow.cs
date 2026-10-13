using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core
{
    /// <summary>
    /// 场景名和手动切场景。不会在 Start 里自动跳转，方便单独打开某个场景调手感。
    /// </summary>
    public static class SceneNames
    {
        public const string Bootstrap = "Bootstrap";
        public const string Boot = "Boot";
        public const string Hub = "Hub";
        public const string Run = "Run";
    }

    public sealed class BootFlow : MonoBehaviour
    {
        [SerializeField] string hubScene = SceneNames.Hub;
        [SerializeField] string runScene = SceneNames.Run;

        public void ContinueToHub()
        {
            SceneManager.LoadScene(hubScene);
        }

        public void StartRun()
        {
            SceneManager.LoadScene(runScene);
        }
    }
}
