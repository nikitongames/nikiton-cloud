using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[System.Serializable] public class RealPackList{ public List<RealPack> packs; }
[System.Serializable] public class RealPack{ public string id; public string title; public float base_kzt; }
[System.Serializable] public class NcoinList{ public List<NcoinPack> daily; public Market market; public class Market{ public bool enabled; } }
[System.Serializable] public class NcoinPack{ public string id; public string title; public int ncoin; }

public static class ShopSystem
{
    static RealPackList real;
    static NcoinList ncd;

    public static IEnumerator Init()
    {
        real = JsonUtility.FromJson<RealPackList>(File.ReadAllText("games/KazFishingMobile/shop/real_packs.json"));
        ncd  = JsonUtility.FromJson<NcoinList>(File.ReadAllText("games/KazFishingMobile/shop/ncoin_daily.json"));
        yield return null;
    }

    // Цена real-пакета в валюте игрока (с округлением)
    public static string GetDisplayPriceReal(string packId)
    {
        var p = real.packs.Find(x=>x.id==packId);
        if (p==null) return "-";
        (float price, string cur) = Pricing.ConvertRound(p.base_kzt);
        return $"{price.ToString(Pricing.PlayerLocale)} {cur}";
    }

    // Покупка real (пока заглушка выдачи, интеграция биллинга позже)
    public static IEnumerator PurchaseReal(string packId, System.Action<bool> cb)
    {
        // здесь будет вызов биллинга платформы; сейчас — мгновенная выдача
        GrantPack(packId);
        cb?.Invoke(true);
        yield return null;
    }

    // Покупка за N-Coin
    public static bool PurchaseNcoin(string packId, ref int ncoinBalance)
    {
        var p = ncd.daily.Find(x=>x.id==packId);
        if (p==null || ncoinBalance < p.ncoin) return false;
        ncoinBalance -= p.ncoin;
        GrantPack(packId);
        return true;
    }

    static void GrantPack(string packId)
    {
        // Заглушка: выдаём предметы в инвентарь по ID набора
        InventoryManager.AddByPack(packId);
        Debug.Log($"[Shop] Выдан набор {packId}");
    }
}
