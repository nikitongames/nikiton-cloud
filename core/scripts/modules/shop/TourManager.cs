using UnityEngine;
using System;
using System.IO;

[Serializable] public class ToursCfg{ public bool single_active; public DonationTours donation_tours_europe; public RealLoc real_money_location; 
  [Serializable] public class DonationTours{ public int _1h; public int _3h; public int _5h; }
  [Serializable] public class RealLoc{ public float _1h_kzt; public float _3h_kzt; public float _5h_kzt; } }

public static class TourManager
{
    static ToursCfg cfg;
    static string keyActive = "tour_active";
    static string keyEndUtc = "tour_end_utc";
    static string keyPaused = "tour_paused"; // остаток секунд при выходе

    public static IEnumerator Init(){ cfg = JsonUtility.FromJson<ToursCfg>(File.ReadAllText("games/KazFishingMobile/configs/tours.json")); yield return null; }

    public static bool HasActiveTour() => PlayerPrefs.GetInt(keyActive,0)==1;

    public static double SecondsLeft()
    {
        if (!HasActiveTour()) return 0;
        var end = DateTime.Parse(PlayerPrefs.GetString(keyEndUtc,"1970-01-01T00:00:00Z")).ToUniversalTime();
        var now = DateTime.UtcNow;
        return Math.Max(0,(end-now).TotalSeconds);
    }

    public static void OnAppPause(bool paused)
    {
        if (!HasActiveTour()) return;
        if (paused)
        {
            // фиксируем остаток на момент выхода
            PlayerPrefs.SetInt(keyPaused, (int)SecondsLeft());
        }
        else
        {
            // при входе пересчитаем новый end от остатка
            int left = PlayerPrefs.GetInt(keyPaused,0);
            if (left>0)
            {
                var newEnd = DateTime.UtcNow.AddSeconds(left);
                PlayerPrefs.SetString(keyEndUtc, newEnd.ToString("o"));
                PlayerPrefs.SetInt(keyPaused,0);
                PlayerPrefs.Save();
            }
        }
    }

    public static bool StartDonationTour(int hours, ref int ncoinBalance, out string warn)
    {
        warn = null;
        if (cfg.single_active && HasActiveTour())
        {
            warn = "У вас уже активен тур. Покупка нового приведёт к потере оставшегося времени.";
            return false;
        }
        int cost = hours==1? cfg.donation_tours_europe._1h : hours==3? cfg.donation_tours_europe._3h : cfg.donation_tours_europe._5h;
        if (ncoinBalance < cost) { warn = "Недостаточно N-Coin."; return false; }
        ncoinBalance -= cost;
        ActivateHours(hours);
        return true;
    }

    public static bool StartRealLocationTour(int hours, out string priceDisplay)
    {
        float baseKzt = hours==1? cfg.real_money_location._1h_kzt : hours==3? cfg.real_money_location._3h_kzt : cfg.real_money_location._5h_kzt;
        var pr = Pricing.ConvertRound(baseKzt);
        priceDisplay = $"{pr.Item1.ToString(Pricing.PlayerLocale)} {pr.Item2}";
        // здесь должна быть покупка real → при успехе:
        ActivateHours(hours);
        return true;
    }

    public static void ExtendTour(int hours)
    {
        if (!HasActiveTour()) { ActivateHours(hours); return; }
        var end = DateTime.Parse(PlayerPrefs.GetString(keyEndUtc)).ToUniversalTime();
        PlayerPrefs.SetString(keyEndUtc, end.AddHours(hours).ToString("o"));
        PlayerPrefs.Save();
    }

    static void ActivateHours(int hours)
    {
        PlayerPrefs.SetInt(keyActive,1);
        PlayerPrefs.SetString(keyEndUtc, DateTime.UtcNow.AddHours(hours).ToString("o"));
        PlayerPrefs.Save();
    }

    public static void Tick()
    {
        if (HasActiveTour() && SecondsLeft()<=0)
        {
            PlayerPrefs.SetInt(keyActive,0);
            PlayerPrefs.DeleteKey(keyEndUtc);
            PlayerPrefs.Save();
        }
    }
}
