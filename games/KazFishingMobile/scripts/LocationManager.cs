using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

[Serializable] public class Loc { public string id; public string title; public string region; public string type; public int min_level; public int unlock_cost_gold; }
[Serializable] public class LocList { public string start_region; public List<Loc> locations; }

[Serializable] public class RegTour { public int gold_per_entry; public int duration_minutes; }
[Serializable] public class RegToursMap : Dictionary<string, RegTour> { }

public static class LocationManager
{
    static LocList locs;
    static Dictionary<string, RegTour> tours;

    public static void Init()
    {
        locs = JsonUtility.FromJson<LocList>(File.ReadAllText("games/KazFishingMobile/configs/locations.json"));
        tours = JsonUtility.FromJson<RegToursMap>(Normalize(File.ReadAllText("games/KazFishingMobile/configs/tours_regional.json")));
    }

    public static IEnumerable<Loc> All() => locs.locations;

    public static bool UnlockBase(string locId)
    {
        var l = locs.locations.FirstOrDefault(x=>x.id==locId);
        if (l==null || l.type!="base") return false;
        if (PlayerProfile.Level < l.min_level) return false;
        if (!PlayerProfile.SpendGold(l.unlock_cost_gold)) return false;
        PlayerProfile.Unlock(locId);
        return true;
    }

    public static bool EnterGoldTour(string locId)
    {
        var l = locs.locations.FirstOrDefault(x=>x.id==locId);
        if (l==null || l.type!="tour_gold") return false;

        if (!tours.TryGetValue(locId, out var cfg)) return false;
        if (!PlayerProfile.SpendGold(cfg.gold_per_entry)) return false;

        // активируем таймер тура на клиенте (упрощённо)
        PlayerPrefs.SetString("KF_TOUR_"+locId, DateTime.UtcNow.AddMinutes(cfg.duration_minutes).ToString("o"));
        PlayerPrefs.Save();
        return true;
    }

    public static bool IsTourActive(string locId)
    {
        string k = "KF_TOUR_"+locId;
        if (!PlayerPrefs.HasKey(k)) return false;
        var end = DateTime.Parse(PlayerPrefs.GetString(k)).ToUniversalTime();
        return DateTime.UtcNow < end;
    }

    static string Normalize(string raw)=>raw.Trim('\uFEFF',' ','\n','\r','\t');
}
