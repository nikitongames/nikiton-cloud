using UnityEngine;

public static class GameInventory
{
    public static string EquippedRod   { get=>PlayerPrefs.GetString("KF_EQ_ROD","rod_basic");   set{PlayerPrefs.SetString("KF_EQ_ROD",value);} }
    public static string EquippedReel  { get=>PlayerPrefs.GetString("KF_EQ_REEL","reel_basic"); set{PlayerPrefs.SetString("KF_EQ_REEL",value);} }
    public static string EquippedLine  { get=>PlayerPrefs.GetString("KF_EQ_LINE","line_basic"); set{PlayerPrefs.SetString("KF_EQ_LINE",value);} }
    public static string EquippedBait  { get=>PlayerPrefs.GetString("KF_EQ_BAIT","bait_universal"); set{PlayerPrefs.SetString("KF_EQ_BAIT",value);} }

    public static int Durability(string itemId) => PlayerPrefs.GetInt("KF_DUR_"+itemId, 100);
    public static void SetDurability(string itemId, int value){ PlayerPrefs.SetInt("KF_DUR_"+itemId, Mathf.Max(0,value)); PlayerPrefs.Save(); }
}
