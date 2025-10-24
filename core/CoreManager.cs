using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikitonCore
{
    /// <summary>
    /// Точка входа ядра. Живёт между сценами.
    /// </summary>
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance { get; private set; }

        [Header("Boot")]
        [Tooltip("Имя стартовой сцены после экрана политики/логотипа.")]
        public string gameplaySceneName = "Main";

        [Tooltip("Имя сцены прелоадера/логотипа/политик.")]
        public string bootSceneName = "InitScene";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Logger.Log("Nikiton Core initialized.");
            Settings.Load();
            LicenseChecker.Verify();

            // Подготовка онлайн-сервисов (позже подключим реальные)
            CloudRuntime.Init();
        }

        /// <summary>
        /// Вызывается после подтверждения политик/логотипа.
        /// </summary>
        public void ContinueToGameplay()
        {
            if (!string.IsNullOrEmpty(gameplaySceneName))
            {
                Logger.Log($"Loading gameplay scene: {gameplaySceneName}");
                SceneManager.LoadScene(gameplaySceneName);
            }
            else
            {
                Logger.Error("Gameplay scene name is empty.");
            }
        }
    }
}
