using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[Serializable] public class TMode { public string id; public string title; public int duration_min; public string scoring; }
[Serializable] public class TRewards { public int gold; public int ncoin; }
[Serializable] public class TCfg { public bool enabled; public List<TMode> modes; public DictR rewards; [Serializable] public class DictR : Dictionary<string, TRewards>{} }

public static class TournamentSystem
{
    static TCfg cfg;
    static bool active=false;
    static string modeId;
    static DateTime endUtc;
    static float bestSingle=0, totalWeight=0;

    public static IEnumerator Init()
    {
        cfg = JsonUtility.FromJson<TCfg>(File.ReadAllText("games/KazFishingMobile/configs/tournaments.json"));
        yield break;
    }

    public static void StartTournament(string id)
    {
        var m = cfg.modes.Find(x=>x.id==id);
        if (m==null) return;
        modeId=id; active=true;
        endUtc=DateTime.UtcNow.AddMinutes(m.duration_min);
        bestSingle=0; totalWeight=0;
        Debug.Log("[Tournament] Started: "+m.title);
    }

    public static void RegisterCatch(float kg)
    {
        if (!active) return;
        var m = cfg.modes.Find(x=>x.id==modeId);
        if (m==null) return;
        if (m.scoring=="max_single") bestSingle = Mathf.Max(bestSingle, kg);
        else if (m.scoring=="total_weight") totalWeight += kg;
    }

    public static void Tick()
    {
        if (!active) return;
        if (DateTime.UtcNow>=endUtc) { Finish(); }
    }

    static void Finish()
    {
        var m = cfg.modes.Find(x=>x.id==modeId);
        var rw = cfg.rewards[modeId];
        if (m.scoring=="max_single" && bestSingle>0) { PlayerProfile.AddGold(rw.gold); PlayerProfile.AddNCoin(rw.ncoin); }
        if (m.scoring=="total_weight" && totalWeight>0) { PlayerProfile.AddGold(rw.gold); PlayerProfile.AddNCoin(rw.ncoin); }
        active=false;
        Debug.Log("[Tournament] Finished.");
    }
}
