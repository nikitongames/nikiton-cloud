using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;

[Serializable] public class RegionInfoMap { public string default_country; public Dictionary<string, Cfg> countries; [Serializable] public class Cfg{ public string currency; public string locale; } }
[Serializable] public class ForexRates { public string @base; public string updated; public Dictionary<string, float> rates; }
[Serializable] public class PricingRules { public Dictionary<string, List<float>> rounding; }

public static class Pricing
{
    public static string PlayerCurrency = "KZT";
    public static CultureInfo PlayerLocale = new CultureInfo("ru-KZ");
    public static Dictionary<string,float> Rates = new Dictionary<string,float>();
    public static Dictionary<string,List<float>> Rule = new Dictionary<string,List<float>>();

    public static void Init(string regionsJson, string forexJson, string rulesJson)
    {
        var reg = JsonUtility.FromJson<RegionInfoMap>(regionsJson);
        string country = reg.default_country;
        PlayerCurrency = reg.countries[country].currency;
        PlayerLocale = new CultureInfo(reg.countries[country].locale);

        var fx = JsonUtility.FromJson<ForexRates>(forexJson);
        Rates = fx.rates;

        var pr = JsonUtility.FromJson<PricingRules>(rulesJson);
        Rule = pr.rounding;
    }

    // baseCurrency=KZT; amountBase=в KZT → конвертация в валюту игрока с округлением
    public static (float, string) ConvertRound(float amountBaseKZT)
    {
        float rate = Rates.ContainsKey(PlayerCurrency) ? Rates[PlayerCurrency] : 1f;
        float raw = amountBaseKZT * rate;
        float rounded = RoundPsy(raw, PlayerCurrency);
        return (rounded, PlayerCurrency);
    }

    static float RoundPsy(float value, string cur)
    {
        if (!Rule.ContainsKey(cur)) return (float)Math.Round(value, 0);
        float best = Rule[cur][0];
        float diffBest = Mathf.Abs(best - value);
        foreach (var r in Rule[cur]) {
            float d = Mathf.Abs(r - value);
            if (d < diffBest) { diffBest = d; best = r; }
        }
        return best;
    }
}
