using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System;

public class NikitonUpdater : MonoBehaviour
{
    private static readonly string updateUrl = "https://cloud.nikiton.games/updates/";
    private static readonly string localVersionFile = "core/configs/system/core_config.json";
    private static readonly string localPatchDir = "core/patches/";

    public static IEnumerator CheckForUpdates()
    {
        Debug.Log("Проверяем обновления NikitoN Cloud...");

        if (!Directory.Exists(localPatchDir))
            Directory.CreateDirectory(localPatchDir);

        string versionData = File.ReadAllText(localVersionFile);
        CoreVersion cfg = JsonUtility.FromJson<CoreVersion>(versionData);
        string currentVersion = cfg.core_version;

        string remoteVersion = GetRemoteVersion();

        if (remoteVersion != currentVersion && !string.IsNullOrEmpty(remoteVersion))
        {
            Debug.Log($"Обнаружено обновление: {currentVersion} → {remoteVersion}");
            yield return DownloadPatch(remoteVersion);
            Debug.Log("✅ Обновление успешно установлено!");
        }
        else
        {
            Debug.Log("Актуальная версия ядра установлена.");
        }

        yield return null;
    }

    private static string GetRemoteVersion()
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                string data = client.DownloadString(updateUrl + "version.txt");
                return data.Trim();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Ошибка проверки обновлений: " + ex.Message);
            return "";
        }
    }

    private static IEnumerator DownloadPatch(string version)
    {
        string patchFile = $"{updateUrl}{version}/update.zip";
        string localFile = $"{localPatchDir}update_{version}.zip";

        using (WebClient client = new WebClient())
        {
            client.DownloadFile(patchFile, localFile);
        }

        // Разархивировать обновление
        System.IO.Compression.ZipFile.ExtractToDirectory(localFile, Application.dataPath, true);
        yield return null;
    }

    [System.Serializable]
    public class CoreVersion
    {
        public string core_version;
    }
}
