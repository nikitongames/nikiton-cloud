using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Nikiton.Core.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        public Image logo;
        public float displayTime = 3f;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Invoke(nameof(LoadNextScene), displayTime);
        }

        void LoadNextScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
