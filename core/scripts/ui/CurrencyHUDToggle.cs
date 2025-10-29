using UnityEngine;

public class CurrencyHUDToggle : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F10;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            var hud = FindObjectOfType<CurrencyHUD>();
            if (hud && hud.gameObject.activeSelf) hud.gameObject.SetActive(false);
            else if (hud) hud.gameObject.SetActive(true);
        }
    }
}
