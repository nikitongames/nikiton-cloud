using UnityEngine;
using System.Collections;

public class NikitonCore : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        // 1) Заставка студии
        yield return NikitonSplashScreen.ShowLogo();

        // 2) Экран загрузки
        yield return NikitonLoadingScreen.ShowLoading();

        // 3) Обновление ядра (тихо/авто)
        yield return NikitonUpdater.CheckForUpdates();

        // 4) Контент по согласию игрока (локации/события/наборы)
        yield return NikitonContentManager.CheckAndAskForContent();
    }
}
