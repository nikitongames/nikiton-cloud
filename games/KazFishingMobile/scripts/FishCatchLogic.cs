using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[Serializable] public class FishItem { public string id; public string title; public float weight_min; public float weight_max; public List<string> spots; }
[Serializable] public class FishDB { public List<FishItem> fish; }

[Serializable] public class GearRod { public string id; public string title; public float max_strength_kg; public int durability_max; }
[Serializable] public class GearReel { public string id; public string title; public float max_strength_kg; public int durability_max; }
[Serializable] public class GearLine { public string id; public string title; public float max_strength_kg; public int durability_max; }
[Serializable] public class GearBait { public string id; public string title; }
[Serializable] public class GearRoot {
    public Starter starter_set; public List<GearRod> rods; public List<GearReel> reels; public List<GearLine> lines; public List<GearBait> baits;
    [Serializable] public class Starter { public string rod; public string reel; public string line; public string bait; }
}

public static class FishCatchLogic
{
    static FishDB db;
    static GearRoot gear;

    public static void Init()
    {
        db   = JsonUtility.FromJson<FishDB>(File.ReadAllText("games/KazFishingMobile/configs/fish_db.json"));
        gear = JsonUtility.FromJson<GearRoot>(File.ReadAllText("games/KazFishingMobile/configs/gear.json"));
    }

    public static void TryCatch(string locationId)
    {
        var candidates = db.fish.Where(f=>f.spots.Contains(locationId)).ToList();
        if (candidates.Count==0){ Debug.Log("Здесь рыба не клюёт."); return; }

        var f = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        float kg = UnityEngine.Random.Range(f.weight_min, f.weight_max);

        // фактическая прочность комплекта
        float rod  = GetMaxKgRod(GameInventory.EquippedRod);
        float reel = GetMaxKgReel(GameInventory.EquippedReel);
        float line = GetMaxKgLine(GameInventory.EquippedLine);
        float gearMax = Mathf.Min(rod, Mathf.Min(reel, line));

        // износ
        DurabilitySystem.ApplyFightDamage(kg, gearMax);

        // шанс обрыва если перегруз
        bool broke = (kg > gearMax) && UnityEngine.Random.value < Mathf.Clamp01((kg-gearMax)/Mathf.Max(kg, 0.01f));
        if (broke || DurabilitySystem.IsBroken(GameInventory.EquippedLine))
        {
            Debug.Log($"Обрыв! {f.title} ~{kg:0.0} кг. Комплект не выдержал.");
            return;
        }

        int goldReward = Mathf.CeilToInt(kg * 50); // базовая экономика
        PlayerProfile.AddGold(goldReward);
        TournamentSystem.RegisterCatch(kg); // учёт для турниров, если активны
        Debug.Log($"Поймано: {f.title} ~{kg:0.0} кг. Награда: {goldReward} золота.");
    }

    static float GetMaxKgRod(string id)  { var x = gear.rods.FirstOrDefault(r=>r.id==id);  return x!=null? x.max_strength_kg : 3f; }
    static float GetMaxKgReel(string id) { var x = gear.reels.FirstOrDefault(r=>r.id==id); return x!=null? x.max_strength_kg : 3f; }
    static float GetMaxKgLine(string id) { var x = gear.lines.FirstOrDefault(r=>r.id==id); return x!=null? x.max_strength_kg : 3f; }
}
