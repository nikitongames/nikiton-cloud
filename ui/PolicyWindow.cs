using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace NikitonCore.UI
{
    /// <summary>
    /// Окно подтверждения политики (3 чекбокса + кнопка "Продолжить").
    /// </summary>
    public class PolicyWindow : MonoBehaviour
    {
        public Toggle privacyToggle;
        public Toggle termsToggle;
        public Toggle legalToggle;
        public Button continueButton;
        public string nextScene = "Main";

        void Start()
        {
            continueButton.interactable = false;

            privacyToggle.onValueChanged.AddListener(UpdateState);
            termsToggle.onValueChanged.AddListener(UpdateState);
            legalToggle.onValueChanged.AddListener(UpdateState);
        }

        void UpdateState(bool _)
        {
            continueButton.interactable = privacyToggle.isOn && termsToggle.isOn && legalToggle.isOn;
        }

        public void OnContinue()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
