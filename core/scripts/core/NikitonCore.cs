using UnityEngine;
using System.Collections;
using System.IO;

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

        // 3) Инициализация валют/регионов/цен
        Pricing.Init(
            File.ReadAllText("core/configs/system/regions.json"),
            File.ReadAllText("updates/forex/exchange_rates.json"),
            File.ReadAllText("core/configs/shop/pricing_rules.json")
        );

        // 4) Проверка обновлений ядра
        yield return NikitonUpdater.CheckForUpdates();

        // 5) Проверка контента (по согласию игрока)
        yield return NikitonContentManager.CheckAndAskForContent();

        // 6) Инициализация магазинов и туров
        yield return ShopSystem.Init();
        yield return TourManager.Init();
    }
}
