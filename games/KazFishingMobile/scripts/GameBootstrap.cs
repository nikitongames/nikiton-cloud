using UnityEngine;
using System.IO;

public class GameBootstrap : MonoBehaviour
{
    void Start()
    {
        var cfg = File.ReadAllText("games/KazFishingMobile/configs/game_config.json");
        var startGold = JsonUtility.FromJson<Root>(cfg).economy.starting_gold;
        var startN = JsonUtility.FromJson<Root>(cfg).economy.starting_ncoin;

        PlayerProfile.Init(startGold, startN);
        GameInventory.EquippedRod = GameInventory.EquippedRod; // прогрев
        FishCatchLogic.Init();
        LocationManager.Init();

        Debug.Log("[KF] Game initialized.");
    }

    [System.Serializable] class Root { public Economy economy; [System.Serializable] public class Economy{ public int starting_gold; public int starting_ncoin; } }
}
