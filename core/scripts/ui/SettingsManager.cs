using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    Canvas canvas;
    GameObject panel;
    Slider soundSlider, musicSlider;
    Dropdown languageDropdown, graphicsDropdown;
    Toggle notifToggle;
    Button closeButton;
    bool visible = false;

    void Start()
    {
        BuildUI();
    }

    void BuildUI()
    {
        canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            var cgo = new GameObject("SettingsCanvas");
            canvas = cgo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cgo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cgo.AddComponent<GraphicRaycaster>();
        }

        panel = new GameObject("SettingsPanel");
        var rt = panel.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(640, 480);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        var bg = panel.AddComponent<Image>();
        bg.color = new Color(0,0,0,0.8f);

        CreateLabel("Настройки", panel.transform, new Vector2(0,180), 28);

        soundSlider = CreateSlider("Громкость звука", new Vector2(0, 100));
        musicSlider = CreateSlider("Громкость музыки", new Vector2(0, 40));

        languageDropdown = CreateDropdown("Язык", new Vector2(0, -40), new string[]{"Русский","English","Қазақша","Українська"});
        graphicsDropdown = CreateDropdown("Графика", new Vector2(0, -100), new string[]{"Низкая","Средняя","Высокая"});
        notifToggle = CreateToggle("Уведомления", new Vector2(0, -160));

        closeButton = CreateButton("Закрыть", new Vector2(0, -220));
        closeButton.onClick.AddListener(()=> panel.SetActive(false));
        panel.SetActive(false);
    }

    void CreateLabel(string text, Transform parent, Vector2 pos, int size)
    {
        var go = new GameObject(text);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent,false);
        rt.sizeDelta = new Vector2(500,40);
        rt.anchoredPosition = pos;
        var t = go.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = text;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = size;
        t.color = Color.white;
    }

    Slider CreateSlider(string label, Vector2 pos)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(panel.transform,false);
        rt.sizeDelta = new Vector2(500,40);
        rt.anchoredPosition = pos;
        var slider = go.AddComponent<Slider>();
        return slider;
    }

    Dropdown CreateDropdown(string label, Vector2 pos, string[] options)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(panel.transform,false);
        rt.sizeDelta = new Vector2(500,40);
        rt.anchoredPosition = pos;
        var dd = go.AddComponent<Dropdown>();
        dd.AddOptions(new System.Collections.Generic.List<string>(options));
        return dd;
    }

    Toggle CreateToggle(string label, Vector2 pos)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(panel.transform,false);
        rt.sizeDelta = new Vector2(500,40);
        rt.anchoredPosition = pos;
        var tgl = go.AddComponent<Toggle>();
        return tgl;
    }

    Button CreateButton(string label, Vector2 pos)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(panel.transform,false);
        rt.sizeDelta = new Vector2(240,50);
        rt.anchoredPosition = pos;
        var img = go.AddComponent<Image>();
        img.color = new Color(0.3f,0.6f,0.9f,1f);
        var btn = go.AddComponent<Button>();
        return btn;
    }

    public void TogglePanel()
    {
        visible = !visible;
        panel.SetActive(visible);
    }
}
