using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace NikitonCore.UI
{
    /// <summary>
    /// Универсальный экран загрузки.
    /// </summary>
    public class UILoader : MonoBehaviour
    {
        public GameObject loadingPanel;
        public float minDisplayTime = 1.5f;

        private IEnumerator Start()
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(true);

            yield return new WaitForSeconds(minDisplayTime);

            Logger.Log("Loading complete (stub).");
            loadingPanel.SetActive(false);
        }

        public static void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
