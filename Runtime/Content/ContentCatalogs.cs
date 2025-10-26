using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.RemoteConfig;

namespace Nikiton.Cloud.Content
{
    public static class ContentCatalogs
    {
        public static Dictionary<string, object> Loaded = new();

        public static async Task LoadAllAsync()
        {
            string baseUrl = RemoteConfigService.Instance.appConfig.GetString(
                "content.catalog_base_url",
                "https://raw.githubusercontent.com/nikitongames/nikiton-cloud/main/games/kazfishing_mobile/catalogs/"
            );
            string[] files = { "locations.json", "fish.json", "gear.json", "shops.json" };
            foreach (var f in files)
            {
                string url = baseUrl + f;
                using var req = UnityWebRequest.Get(url);
                await req.SendWebRequest();
                if (req.result == UnityWebRequest.Result.Success)
                {
                    Loaded[f] = req.downloadHandler.text;
                    Debug.Log($"[Catalog] Loaded {f}");
                }
                else Debug.LogWarning($"[Catalog] Failed {f}: {req.error}");
            }
        }
    }
}
