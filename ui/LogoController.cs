using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace NikitonCore.UI
{
    /// <summary>
    /// Управляет показом логотипа Nikiton Games при запуске.
    /// </summary>
    public class LogoController : MonoBehaviour
    {
        public GameObject logoImage;
        public float displayTime = 2.5f;
        public string nextScene = "PolicyScene";

        private IEnumerator Start()
        {
            if (logoImage != null)
                logoImage.SetActive(true);

            yield return new WaitForSeconds(displayTime);
            SceneManager.LoadScene(nextScene);
        }
    }
}
