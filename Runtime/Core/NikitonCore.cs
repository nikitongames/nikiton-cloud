using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using Unity.Services.CloudSave;

namespace Nikiton.Cloud
{
    public static class NikitonCore
    {
        public static string GameId { get; private set; } = "";
        public static bool IsReady { get; private set; }

        public static async Task InitializeAsync(string gameId)
        {
            if (IsReady) return;
            GameId = gameId;

            // 1) Unity Gaming Services
            await UnityServices.InitializeAsync();

            // 2) Анонимная аутентификация
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            // 3) Remote Config — дождаться загрузки
            var tcs = new TaskCompletionSource<bool>();
            RemoteConfigService.Instance.LoadCompleted += _ => tcs.TrySetResult(true);
            RemoteConfigService.Instance.FetchConfigs(new user(), new app());
            await tcs.Task;

            // 4) Cloud Save — проверка доступности
            try { await CloudSaveService.Instance.Data.ForceSaveAsync(); } catch { /* ok */ }

            IsReady = true;
            Debug.Log($"[NikitonCore] Ready for {GameId}");
        }

        struct user {}
        struct app {}
    }
}
