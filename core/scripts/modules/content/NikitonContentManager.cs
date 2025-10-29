using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System;
using System.Text;

public class NikitonContentManager : MonoBehaviour
{
    // URL каталога контента текущей игры (KazFishingMobile)
    private const string CatalogUrl =
        "https://raw.githubusercontent.com/nikitongames/nikiton-cloud/main/games/KazFishingMobile/content/catalog.json";

    // Локальная папка для установленного контента
    private static string LocalContentDir =>
        Path.Combine(Application.persistentDataPath, "NikitoN", "KazFishingMobile", "content");

    [Serializable]
    private class Catalog
    {
        public string version;
        public List<Item> items;
    }

    [Serializable]
    private class Item
    {
        public string id;         // например: "lake_charyn"
        public string type;       // "location" | "event" | "pack"
        public string title;      // "Озеро Чарын"
        public float  size_mb;    // 42.0
        public string hash;       // "sha256:...." (опционально можно не указывать, но лучше указать)
        public string url;        // полный URL на ZIP
    }

    public static IEnumerator CheckAndAskForContent()
    {
        // 1) Скачиваем каталог
        Catalog catalog = null;
        using (UnityWebRequest req = UnityWebRequest.Get(CatalogUrl))
        {
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"[Content] Не удалось получить каталог: {req.error}");
                yield break;
            }
            try
            {
                catalog = JsonUtility.FromJson<Catalog>(NormalizeJson(req.downloadHandler.text));
            }
            catch (Exception e)
            {
                Debug.LogWarning("[Content] Ошибка парсинга catalog.json: " + e.Message);
                yield break;
            }
        }
        if (catalog == null || catalog.items == null || catalog.items.Count == 0)
        {
            yield break;
        }

        // 2) Определяем, что из каталога отсутствует локально
        Directory.CreateDirectory(LocalContentDir);
        List<Item> toOffer = new List<Item>();
        foreach (var it in catalog.items)
        {
            string installMark = Path.Combine(LocalContentDir, it.id + ".installed");
            if (!File.Exists(installMark))
            {
                toOffer.Add(it);
            }
        }

        if (toOffer.Count == 0)
        {
            yield break; // всё уже установлено
        }

        // 3) Показываем диалог игроку: «Доступны новые материалы… Скачать?»
        yield return ShowOfferDialog(toOffer);
    }

    private static IEnumerator ShowOfferDialog(List<Item> toOffer)
    {
        // Готовим UI
        var canvasGO = new GameObject("NIK_ContentCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        var raycaster = canvasGO.AddComponent<GraphicRaycaster>();

        var panel = new GameObject("Panel").AddComponent<Image>();
        panel.color = new Color(0,0,0,0.75f);
        panel.rectTransform.SetParent(canvas.transform, false);
        panel.rectTransform.anchorMin = Vector2.zero;
        panel.rectTransform.anchorMax = Vector2.one;
        panel.rectTransform.offsetMin = Vector2.zero;
        panel.rectTransform.offsetMax = Vector2.zero;

        var box = new GameObject("Box").AddComponent<Image>();
        box.color = new Color(0.12f,0.12f,0.12f,1f);
        box.rectTransform.SetParent(panel.transform, false);
        box.rectTransform.sizeDelta = new Vector2(800, 480);

        var title = new GameObject("Title").AddComponent<Text>();
        title.rectTransform.SetParent(box.transform, false);
        title.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        title.text = "Доступен новый контент";
        title.alignment = TextAnchor.MiddleCenter;
        title.color = Color.white;
        title.fontSize = 32;
        title.rectTransform.anchoredPosition = new Vector2(0, 180);

        var listText = new GameObject("List").AddComponent<Text>();
        listText.rectTransform.SetParent(box.transform, false);
        listText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        listText.alignment = TextAnchor.UpperLeft;
        listText.color = Color.white;
        listText.fontSize = 22;
        listText.rectTransform.sizeDelta = new Vector2(720, 260);
        listText.rectTransform.anchoredPosition = new Vector2(0, 20);

        // Формируем список
        StringBuilder sb = new StringBuilder();
        foreach (var it in toOffer)
        {
            sb.AppendLine($"• {it.title}  ({Mathf.CeilToInt(it.size_mb)} МБ)");
        }
        listText.text = sb.ToString();

        // Кнопка «Скачать»
        var btnYesGO = new GameObject("BtnYes");
        var btnYesImg = btnYesGO.AddComponent<Image>();
        btnYesImg.color = new Color(0.2f,0.6f,0.2f,1f);
        var btnYes = btnYesGO.AddComponent<Button>();
        btnYesGO.transform.SetParent(box.transform, false);
        (btnYes.transform as RectTransform).sizeDelta = new Vector2(260, 60);
        (btnYes.transform as RectTransform).anchoredPosition = new Vector2(-150, -180);
        var yesTxt = new GameObject("YesTxt").AddComponent<Text>();
        yesTxt.rectTransform.SetParent(btnYes.transform, false);
        yesTxt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        yesTxt.alignment = TextAnchor.MiddleCenter;
        yesTxt.color = Color.white;
        yesTxt.fontSize = 24;
        yesTxt.text = "Скачать";

        // Кнопка «Позже»
        var btnNoGO = new GameObject("BtnNo");
        var btnNoImg = btnNoGO.AddComponent<Image>();
        btnNoImg.color = new Color(0.5f,0.5f,0.5f,1f);
        var btnNo = btnNoGO.AddComponent<Button>();
        btnNoGO.transform.SetParent(box.transform, false);
        (btnNo.transform as RectTransform).sizeDelta = new Vector2(260, 60);
        (btnNo.transform as RectTransform).anchoredPosition = new Vector2(150, -180);
        var noTxt = new GameObject("NoTxt").AddComponent<Text>();
        noTxt.rectTransform.SetParent(btnNo.transform, false);
        noTxt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        noTxt.alignment = TextAnchor.MiddleCenter;
        noTxt.color = Color.white;
        noTxt.fontSize = 24;
        noTxt.text = "Позже";

        bool? choice = null;
        btnYes.onClick.AddListener(() => choice = true);
        btnNo.onClick.AddListener(() => choice = false);

        // Ждём выбор
        while (choice == null) yield return null;

        if (choice == true)
        {
            // Скачиваем по очереди
            foreach (var it in toOffer)
            {
                yield return DownloadAndInstall(it);
            }
        }

        GameObject.Destroy(canvasGO);
    }

    private static IEnumerator DownloadAndInstall(Item item)
    {
        Directory.CreateDirectory(LocalContentDir);
        string tempZip = Path.Combine(LocalContentDir, item.id + ".zip");
        string installMark = Path.Combine(LocalContentDir, item.id + ".installed");

        Debug.Log($"[Content] Скачиваем {item.title} ({item.size_mb} МБ)...");
        using (UnityWebRequest req = UnityWebRequest.Get(item.url))
        {
            req.downloadHandler = new DownloadHandlerFile(tempZip);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[Content] Ошибка скачивания {item.id}: {req.error}");
                yield break;
            }
        }

        Debug.Log($"[Content] Распаковываем {item.id}...");
        try
        {
            // Распаковываем в папку контента
            ZipFile.ExtractToDirectory(tempZip, LocalContentDir, true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Content] Ошибка распаковки: {ex.Message}");
            yield break;
        }
        finally
        {
            if (File.Exists(tempZip)) File.Delete(tempZip);
        }

        // Ставим маркер установки
        File.WriteAllText(installMark, DateTime.UtcNow.ToString("O"));
        Debug.Log($"[Content] Установлено: {item.title}");
    }

    private static string NormalizeJson(string raw)
    {
        // На случай BOM/CRLF — приводим к норме
        return raw.Trim('\uFEFF', ' ', '\n', '\r', '\t');
    }
}
