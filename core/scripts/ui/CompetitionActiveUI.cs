using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CompetitionActiveUI : MonoBehaviour
{
    public static CompetitionActiveUI Instance;

    GameObject panel;
    RectTransform listContainer;
    Text headerText, timerText;
    Button toggleButton, closeButton;

    bool isVisible = true;
    CompetitionData current;

    void Awake()
    {
        if (Instance) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BuildUI();
        Hide();
    }

    void BuildUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            var cgo = new GameObject("CompetitionCanvas");
            canvas = cgo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cgo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cgo.AddComponent<GraphicRaycaster>();
        }

        panel = new GameObject("CompetitionActivePanel");
        var rt = panel.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(500, 400);
        rt.anchorMin = new Vector2(1, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(-20, -100);
        var bg = panel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.75f);

        headerText = CreateText("Активное состязание", panel.transform, 22, new Vector2(0, -30));
        timerText  = CreateText("00:00:00", panel.transform, 20, new Vector2(0, -60));

        // Кнопка сворачивания
        toggleButton = CreateButton("⤢", panel.transform, new Vector2(-220, -30));
        toggleButton.onClick.AddListener(()=> TogglePanel());

        // Кнопка закрытия
        closeButton = CreateButton("×", panel.transform, new Vector2(220, -30));
        closeButton.onClick.AddListener(()=> Hide());

        // Таблица лидеров
        var scrollGO = new GameObject("ScrollView");
        var srt = scrollGO.AddComponent<RectTransform>();
        srt.SetParent(panel.transform, false);
        srt.sizeDelta = new Vector2(460, 260);
        srt.anchoredPosition = new Vector2(0, -180);
        var scroll = scrollGO.AddComponent<ScrollRect>();
        scroll.horizontal = false;
        var mask = scrollGO.AddComponent<Mask>();
        mask.showMaskGraphic = false;
        var img = scrollGO.AddComponent<Image>();
        img.color = new Color(1,1,1,0.1f);

        var contentGO = new GameObject("ListContainer");
        listContainer = contentGO.AddComponent<RectTransform>();
        listContainer.SetParent(scrollGO.transform, false);
        listContainer.sizeDelta = new Vector2(440, 10);
        scroll.content = listContainer;
    }

    Text CreateText(string txt, Transform parent, int size, Vector2 pos)
    {
        var go = new GameObject("Text");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.sizeDelta = new Vector2(400, 40);
        rt.anchoredPosition = pos;
        var t = go.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = txt;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = size;
        t.color = Color.white;
        return t;
    }

    Button CreateButton(string label, Transform parent, Vector2 pos)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.sizeDelta = new Vector2(36, 36);
        rt.anchoredPosition = pos;
        var img = go.AddComponent<Image>();
        img.color = new Color(0.3f, 0.6f, 0.9f, 1f);
        var btn = go.AddComponent<Button>();
        var textGO = new GameObject("Label");
        var trt = textGO.AddComponent<RectTransform>();
        trt.SetParent(go.transform, false);
        trt.sizeDelta = new Vector2(36, 36);
        var t = textGO.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = label;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 20;
        t.color = Color.white;
        return btn;
    }

    public void Show(CompetitionData comp)
    {
        current = comp;
        isVisible = true;
        panel.SetActive(true);
        RefreshList();
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    public void Hide()
    {
        panel.SetActive(false);
        StopAllCoroutines();
    }

    public void TogglePanel()
    {
        isVisible = !isVisible;
        foreach (Transform child in panel.transform)
            if (child.name != "⤢" && child.name != "×")
                child.gameObject.SetActive(isVisible);
    }

    void RefreshList()
    {
        foreach (Transform c in listContainer) Destroy(c.gameObject);
        if (current == null || current.participants == null) return;

        var sorted = current.participants.OrderByDescending(p=>p.score).ToList();
        float offsetY = -20f;
        for (int i=0;i<sorted.Count;i++)
        {
            var go = new GameObject("Row");
            var rt = go.AddComponent<RectTransform>();
            rt.SetParent(listContainer, false);
            rt.sizeDelta = new Vector2(400, 30);
            rt.anchoredPosition = new Vector2(0, offsetY);
            offsetY -= 32f;

            var t = go.AddComponent<Text>();
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.text = $"{i+1}. {sorted[i].name} — {sorted[i].score:0.0}";
            t.fontSize = 18;
            t.color = Color.white;
            t.alignment = TextAnchor.MiddleLeft;
        }
    }

    System.Collections.IEnumerator UpdateTimer()
    {
        while (current != null && current.endTime > System.DateTime.UtcNow)
        {
            var remain = current.endTime - System.DateTime.UtcNow;
            timerText.text = remain.ToString(@"hh\:mm\:ss");
            yield return new WaitForSeconds(1);
        }
        timerText.text = "Завершено";
        RefreshList();
    }
}
