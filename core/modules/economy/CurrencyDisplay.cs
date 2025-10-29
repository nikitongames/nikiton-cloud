using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

[System.Serializable] class CurrencyVisualEntry { public string icon; public string symbol; }
[System.Serializable] class CurrencyVisualMap : Dictionary<string, CurrencyVisualEntry> {}

public static class CurrencyDisplay
{
    static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    static CurrencyVisualMap visual;
    static string basePath = "core/assets/icons/currency/";
    static bool loaded = false;

    static void EnsureLoaded()
    {
        if (loaded) return;
        try
        {
            string cfg = File.ReadAllText("core/configs/shop/currency_visual.json");
            visual = JsonUtility.FromJson<CurrencyVisualMap>(NormalizeToJson(cfg));
        }
        catch { visual = new CurrencyVisualMap(); }
        loaded = true;
    }

    public static Sprite GetIcon(string currencyId)
    {
        EnsureLoaded();
        currencyId = currencyId.ToLowerInvariant();
        if (spriteCache.TryGetValue(currencyId, out var sp)) return sp;

        string fileName = (visual != null && visual.ContainsKey(currencyId)) ? visual[currencyId].icon : (currencyId + ".png");
        string path = Path.Combine(basePath, fileName);

        if (File.Exists(path))
        {
            byte[] img = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(img);
            sp = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f,0.5f));
            spriteCache[currencyId] = sp;
            return sp;
        }
        return null;
    }

    public static string GetSymbol(string currencyId)
    {
        EnsureLoaded();
        currencyId = currencyId.ToLowerInvariant();
        if (visual != null && visual.ContainsKey(currencyId) && !string.IsNullOrEmpty(visual[currencyId].symbol))
            return visual[currencyId].symbol;

        // запасной вариант
        switch(currencyId)
        {
            case "gold":  return "🪙";
            case "ncoin": return "💠";
            default: return "?";
        }
    }

    public static string Format(string currencyId, int amount)
    {
        var ci = (Pricing.PlayerLocale != null ? Pricing.PlayerLocale : CultureInfo.InvariantCulture);
        return amount.ToString("N0", ci) + " " + GetSymbol(currencyId);
    }

    static string NormalizeToJson(string raw) => raw.Trim('\uFEFF',' ','\n','\r','\t');
}
