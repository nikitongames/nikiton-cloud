using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

[Serializable] public class RegionEntry { public string name; public string currency; public string symbol; public string lang; public List<string> fallbacks; }
[Serializable] public class RegionsCfg { public string @default; public Dictionary<string, RegionEntry> regions; }

public static class LanguageManager
{
    private const string KEY = "KF_LANG";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AutoSetLanguage()
    {
        try
        {
            // читаем регионы
            var json = File.ReadAllText("core/configs/system/regions.json");
            var cfg  = JsonUtility.FromJson<RegionsCfg>(json);

            // берём страну из Pricing (если уже инициализировано) или default
            string country = GetCountryFallback(cfg);
            string lang = cfg.regions.ContainsKey(country) ? cfg.regions[country].lang : "en";

            // если уже зафиксирован язык — не трогаем
            if (!PlayerPrefs.HasKey(KEY))
            {
                PlayerPrefs.SetString(KEY, lang);
                PlayerPrefs.Save();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("[Language] AutoSetLanguage fail: " + e.Message);
            if (!PlayerPrefs.HasKey(KEY)) { PlayerPrefs.SetString(KEY, "en"); PlayerPrefs.Save(); }
        }
    }

    static string GetCountryFallback(RegionsCfg cfg)
    {
        // до интеграции с реальным API региона — используем default
        return string.IsNullOrEmpty(cfg.@default) ? "KZ" : cfg.@default;
    }

    public static string GetLang() => PlayerPrefs.GetString(KEY, "en");
    public static void SetLang(string lang) { PlayerPrefs.SetString(KEY, lang); PlayerPrefs.Save(); }
}
