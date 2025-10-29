using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[Serializable] public class FishItem { public string id; public string title; public float weight_min; public float weight_max; public List<string> spots; }
[Serializable] public class FishDB { public List<FishItem> fish; }

public static class FishCatchLogic
{
    static FishDB db;

    public static void Init()
    {
        string json = File.ReadAllText("games/KazFishingMobile/configs/fish_db.json");
        db = JsonUtility.FromJson<FishDB>(json);
    }

    public static void TryCatch(string locationId)
    {
        var candidates = db.fish.Where(f=>f.spots.Contains(locationId)).ToList();
        if (candidates.Count==0){ Debug.Log("Здесь рыба не клюёт."); return; }

        var f = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        float kg = UnityEngine.Random.Range(f.weight_min, f.weight_max);

        // сила комплекта — минимум из трёх элементов
        float rod = GetMaxKg(GameInventory.EquippedRod);
        float reel = GetMaxKg(GameInventory.EquippedReel);
        float line = GetMaxKg(GameInventory.EquippedLine);
        float gearMax = Mathf.Min(rod, Mathf.Min(reel, line));

        // износ
        DurabilitySystem.ApplyFightDamage(kg, gearMax);

        // шанс обрыва если перегруз
        bool broke = (kg > gearMax) && UnityEngine.Random.value < Mathf.Clamp01((kg-gearMax)/kg);
        if (broke || DurabilitySystem.IsBroken(GameInventory.EquippedLine))
        {
            Debug.Log($"Обрыв! Рыба {f.title} ~{kg:0.0} кг. Леска не выдержала.");
            return;
        }

        int goldReward = Mathf.CeilToInt(kg * 50); // пример
        PlayerProfile.AddGold(goldReward);
        Debug.Log($"Поймано: {f.title} ~{kg:0.0} кг. Награда: {goldReward} золота.");
    }

    static float GetMaxKg(string itemId)
    {
        // простая карта — синхронизируй с gear.json
        if (itemId=="rod_basic"||itemId=="reel_basic") return 5f;
        if (itemId=="rod_pro"  ||itemId=="reel_pro")   return 12f;
        if (itemId=="line_basic")  return 4f;
        if (itemId=="line_strong") return 10f;
        return 3f;
    }
}
