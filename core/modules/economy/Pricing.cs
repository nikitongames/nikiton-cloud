using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

public static class Pricing
{
    private static Dictionary<string, float> rates;
    private static string playerRegion = "KZ";

    public static void Init(string regionsJson, string forexJson, string rulesJson)
    {
        Debug.Log("[Economy] Pricing system initialized");
        rates = new Dictionary<string, float> {
            {"KZT", 1f}, {"RUB", 4.8f}, {"BYN", 14.7f}, {"UAH", 0.084f}, {"EUR", 490f}, {"USD", 460f}
        };
    }

    public static float ConvertFromKZT(float amount, string targetCurrency)
    {
        if (!rates.ContainsKey(targetCurrency)) return amount;
        return amount / rates[targetCurrency];
    }

    public static string Format(float amount, string currency)
    {
        return $"{amount:0.##} {currency}";
    }
}
