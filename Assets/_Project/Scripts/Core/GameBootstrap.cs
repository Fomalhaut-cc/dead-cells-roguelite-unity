using UnityEngine;

namespace Project.Core
{
    /// <summary>
    /// Bootstrap 场景的入口。注册时钟和随机数，并在切场景时保留自己。
    /// 不读取存档，存档在 <c>Project.Meta.MetaBootstrap</c>。
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public sealed class GameBootstrap : MonoBehaviour
    {
        static GameBootstrap instance;

        [SerializeField] int fallbackSeed = 1;

        public GameClock Clock { get; private set; }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            Clock = new GameClock();
            ServiceLocator.Register(Clock);
            ServiceLocator.Register<IRandomService>(new SeededRandom(fallbackSeed));
        }

        void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
