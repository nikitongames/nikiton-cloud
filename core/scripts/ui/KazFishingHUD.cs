using UnityEngine;
using System.Collections;

public class KazFishingHUD : MonoBehaviour
{
    int ncoin=1500, gold=0;

    public void BuyEuropeTour1h()
    {
        string warn;
        if (TourManager.HasActiveTour())
        {
            StartCoroutine(TourWarningUI.Show("У вас активен тур. Покупка нового приведёт к потере оставшегося времени. Продолжить?", ok=>{
                if (ok) StartCoroutine(BuyDonationTourInternal(1));
            }));
        }
        else
        {
            StartCoroutine(BuyDonationTourInternal(1));
        }
    }

    IEnumerator BuyDonationTourInternal(int hours)
    {
        string warn;
        if (!TourManager.StartDonationTour(hours, ref ncoin, out warn))
        {
            if (!string.IsNullOrEmpty(warn)) Debug.LogWarning(warn);
            yield break;
        }
        yield return null;
    }

    public void ExchangeNcoinToGold(int amount)
    {
        if (CurrencyExchange.ExchangeNcoinToGold(ref ncoin, ref gold, amount))
            Debug.Log($"Обмен выполнен: {amount} N→ {amount*100} G");
        else
            Debug.LogWarning("Обмен отклонён.");
    }

    void Update(){ TourManager.Tick(); }
    void OnApplicationPause(bool p){ TourManager.OnAppPause(p); }
}
