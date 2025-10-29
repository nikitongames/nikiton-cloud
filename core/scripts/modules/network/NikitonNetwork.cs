using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class NikitonNetwork
{
    public static IEnumerator GetText(string url, System.Action<string> onDone)
    {
        using (var req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success) onDone(req.downloadHandler.text);
            else onDone(null);
        }
    }
}
