using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.RemoteConfig;

namespace Nikiton.Cloud.Communication
{
    public static class ChatService
    {
        public static async Task<bool> Send(string user, string text)
        {
            bool enabled = RemoteConfigService.Instance.appConfig.GetBool("features.chat_enabled", false);
            string url   = RemoteConfigService.Instance.appConfig.GetString("chat.webhook_url", "");
            if (!enabled || string.IsNullOrEmpty(url)) return false;

            var payload = JsonUtility.ToJson(new Msg { user = user, text = text, ts = Now() });
            using var req = new UnityWebRequest(url, "POST");
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            return req.result == UnityWebRequest.Result.Success;
        }

        static long Now() => System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        [System.Serializable] struct Msg { public string user; public string text; public long ts; }
    }
}
