using UnityEngine;
using System.Collections;
using System.IO;

public class GameOnlineInit : MonoBehaviour
{
    IEnumerator Start()
    {
        // Профиль/инвентарь/локации
        var cfg = File.ReadAllText("games/KazFishingMobile/configs/game_config.json");
        var root = JsonUtility.FromJson<Root>(cfg);
        PlayerProfile.Init(root.economy.starting_gold, root.economy.starting_ncoin);
        FishCatchLogic.Init();
        LocationManager.Init();

        // Чат
        StartCoroutine(ChatClient.Init());

        // Турниры
        yield return TournamentSystem.Init();

        Debug.Log("[KF] Online systems ready.");
    }

    [System.Serializable] class Root { public Economy economy; [System.Serializable] public class Economy{ public int starting_gold; public int starting_ncoin; } }
}
