using UnityEngine;

namespace NikitonCore.UI
{
    /// <summary>
    /// Управляет базовыми UI элементами (кнопки, звуки, экран загрузки).
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayClickSound()
        {
            Logger.Log("UI Click sound (stub).");
        }

        public void ShowMessage(string msg)
        {
            Logger.Log($"UI Message: {msg}");
        }
    }
}
