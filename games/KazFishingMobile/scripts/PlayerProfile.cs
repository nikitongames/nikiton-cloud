using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class ProfileData {
    public int level = 1;
    public int xp = 0;
    public int gold = 0;
    public int ncoin = 0;
    public List<string> unlocked = new List<string>();
}

public static class PlayerProfile
{
    static ProfileData data;
    const string KEY = "KF_PROFILE_JSON";

    public static void Init(int startGold, int startNcoin)
    {
        if (PlayerPrefs.HasKey(KEY))
            data = JsonUtility.FromJson<ProfileData>(PlayerPrefs.GetString(KEY));
        else {
            data = new ProfileData { gold = startGold, ncoin = startNcoin };
            Save();
        }
    }

    public static void Save() => PlayerPrefs.SetString(KEY, JsonUtility.ToJson(data));
    public static int Gold => data.gold;
    public static int NCoin => data.ncoin;
    public static int Level => data.level;
    public static bool IsUnlocked(string loc) => data.unlocked.Contains(loc);

    public static bool SpendGold(int amount){ if (data.gold<amount) return false; data.gold-=amount; Save(); return true; }
    public static bool SpendNCoin(int amount){ if (data.ncoin<amount) return false; data.ncoin-=amount; Save(); return true; }
    public static void AddGold(int amount){ data.gold+=amount; Save(); }
    public static void AddNCoin(int amount){ data.ncoin+=amount; Save(); }
    public static void Unlock(string loc){ if(!data.unlocked.Contains(loc)) { data.unlocked.Add(loc); Save(); } }
}
