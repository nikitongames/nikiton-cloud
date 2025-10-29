using UnityEngine;

public static class InventoryManager
{
    public static void AddByPack(string packId)
    {
        PlayerPrefs.SetInt("inv_"+packId, PlayerPrefs.GetInt("inv_"+packId,0)+1);
        PlayerPrefs.Save();
    }
}
