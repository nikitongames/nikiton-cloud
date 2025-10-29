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
        string regions = File.ReadAllText("core/configs/system/regions.json");
        string forex   = File.ReadAllText("updates/forex/exchange_rates.json");
        string rules   = File.ReadAllText("core/configs/shop/pricing_rules.json");
        Pricing.Init(regions, forex, rules);

        // 4) Обновление ядра
        yield return NikitonUpdater.CheckForUpdates();

        // 5) Контент по согласию игрока (локации/события/наборы)
        yield return NikitonContentManager.CheckAndAskForContent();

        // 6) Модули магазина и туров
        yield return ShopSystem.Init();
        yield return TourManager.Init();
    }
}
