using UnityEngine;

public class CurrencyHUDBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Spawn()
    {
        // Создаём объект с HUD, если ещё нет
        if (Object.FindObjectOfType<CurrencyHUD>() == null)
        {
            var go = new GameObject("CurrencyHUD");
            go.AddComponent<CurrencyHUD>();
            Object.DontDestroyOnLoad(go);
        }
    }
}
