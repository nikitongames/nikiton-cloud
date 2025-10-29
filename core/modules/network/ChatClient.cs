using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable] public class ChatMsg { public string id; public string user; public string txt; public string lang; public long ts; }
[Serializable] public class ChatPacket { public List<ChatMsg> messages; }

public static class ChatClient
{
    static string pollUrl = "https://raw.githubusercontent.com/nikitongames/nikiton-cloud/main/updates/chat/global.json";
    static float pollInterval = 5f;
    static List<ChatMsg> buffer = new List<ChatMsg>();
    public static System.Action<ChatMsg> OnMessage;

    public static IEnumerator Init()
    {
        // Fallback-поллинг: читаем общий канал
        while (true)
        {
            using (var req = UnityWebRequest.Get(pollUrl))
            {
                yield return req.SendWebRequest();
                if (req.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        var pkt = JsonUtility.FromJson<ChatPacket>(Normalize(req.downloadHandler.text));
                        if (pkt?.messages != null)
                        {
                            foreach (var m in pkt.messages)
                            {
                                if (!buffer.Exists(x=>x.id==m.id))
                                {
                                    buffer.Add(m);
                                    // авто-перевод
                                    var t = Translator.Auto(m.txt, m.lang);
                                    m.txt = t.text; m.lang = t.lang;
                                    OnMessage?.Invoke(m);
                                }
                            }
                        }
                    }
                    catch {}
                }
            }
            yield return new WaitForSeconds(pollInterval);
        }
    }

    public static void SendLocal(string user, string text, string lang="auto")
    {
        var m = new ChatMsg { id=Guid.NewGuid().ToString("N"), user=user, txt=text, lang=lang, ts=DateTimeOffset.UtcNow.ToUnixTimeSeconds() };
        var t = Translator.Auto(m.txt, lang);
        m.txt = t.text; m.lang = t.lang;
        buffer.Add(m);
        OnMessage?.Invoke(m);
    }

    static string Normalize(string raw)=>raw.Trim('\uFEFF',' ','\n','\r','\t');
}

public static class Translator
{
    static Dictionary<string, Dictionary<string,string>> dict = new Dictionary<string, Dictionary<string,string>>();
    static string defaultLang = "ru";

    public struct TOut { public string text; public string lang; }

    public static TOut Auto(string src, string srcLang="auto")
    {
        // Простая стратегия: если есть словарь — подставим, иначе вернём как есть
        string playerLang = PlayerPrefs.GetString("KF_LANG", defaultLang);
        if (playerLang==defaultLang) return new TOut{ text=src, lang=playerLang };

        var t = Lookup(playerLang, src.ToLowerInvariant());
        return new TOut{ text=string.IsNullOrEmpty(t)?src:t, lang=playerLang };
    }

    static string Lookup(string lang, string key)
    {
        if (!dict.ContainsKey(lang))
        {
            var url = $"https://raw.githubusercontent.com/nikitongames/nikiton-cloud/main/updates/i18n/{lang}.json";
            try {
                using (var w = new System.Net.WebClient())
                {
                    var json = w.DownloadString(url);
                    var map = JsonUtility.FromJson<JsonMap>(json);
                    dict[lang] = map.ToDict();
                }
            } catch { dict[lang] = new Dictionary<string,string>(); }
        }
        if (dict[lang].TryGetValue(key, out var t)) return t;
        return null;
    }

    [Serializable] class JsonMap { public List<KV> _ = new List<KV>(); }
    [Serializable] class KV { public string k; public string v; }
}

