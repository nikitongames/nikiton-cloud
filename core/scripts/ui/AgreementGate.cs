using UnityEngine;
using UnityEngine.UI;

public class AgreementGate : MonoBehaviour
{
    GameObject panel;
    Toggle tPrivacy, tTerms, tEula;
    Button continueBtn;
    Text warning;

    void Start()
    {
        // Проверяем, было ли ранее принято соглашение
        if (PlayerPrefs.GetInt("user_has_agreed", 0) == 1)
        {
            Destroy(this.gameObject);
            return;
        }

        BuildUI();
    }

    void BuildUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            var cgo = new GameObject("AgreementCanvas");
            canvas = cgo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cgo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cgo.AddComponent<GraphicRaycaster>();
        }

        panel = new GameObject("AgreementPanel");
        var rt = panel.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(720, 520);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        var bg = panel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.85f);

        CreateText("Перед началом игры подтвердите согласие:", panel.transform, new Vector2(0, 200), 26);

        tPrivacy = CreateToggle("Политика конфиденциальности", new Vector2(0, 100));
        tTerms   = CreateToggle("Условия использования", new Vector2(0, 40));
        tEula    = CreateToggle("Лицензионное соглашение (EULA)", new Vector2(0, -20));

        warning = CreateText("Для продолжения отметьте все пункты", panel.transform, new Vector2(0, -120), 18);
        warning.color = Color.yellow;

        continueBtn = CreateButton("Продолжить", new Vector2(0, -200));
        continueBtn.interactable = false;
        continueBtn.onClick.AddListener(()=> AcceptAll());

        tPrivacy.onValueChanged.AddListener(delegate { CheckToggles(); });
        tTerms.onValueChanged.AddListener(delegate { CheckToggles(); });
        tEula.onValueChanged.AddListener(delegate { CheckToggles(); });
    }

    Toggle CreateToggle(string label, Vector2 pos)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(panel.transform, false);
        rt.sizeDelta = new Vector2(600, 40);
        rt.anchoredPosition = pos;

        var tgl = go.AddComponent<Toggle>();
        var bg = new GameObject("BG").AddComponent<Image>();
        bg.transform.SetParent(go.transform,false);
        bg.color = new Color(0.25f,0.25f,0.25f,1f);

        var check = new GameObject("Checkmark").AddComponent<Image>();
        check.transform.SetParent(go.transform,false);
        check.color = Color.green;
        tgl.graphic = check;

        var lbl = new GameObject("Label").AddComponent<Text>();
        lbl.transform.SetParent(go.transform,false);
        lbl.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        lbl.text = label;
        lbl.alignment = TextAnchor.MiddleLeft;
        lbl.fontSize = 18;
        lbl.color = Color.white;
        lbl.rectTransform.anchoredPosition = new Vector2(30, 0);

        return tgl;
    }

    Text CreateText(string text, Transform parent, Vector2 pos, int size)
    {
        var go = new GameObject("Text");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.sizeDelta = new Vector2(640, 60);
        rt.anchoredPosition = pos;
        var t = go.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = text;
        t.fontSize = size;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        return t;
    }

    Button CreateButton(string label, Vector2 pos)
    {
        var go = new GameObject(label);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(panel.transform, false);
        rt.sizeDelta = new Vector2(240, 60);
        rt.anchoredPosition = pos;
        var img = go.AddComponent<Image>();
        img.color = new Color(0.3f, 0.6f, 0.9f);
        var btn = go.AddComponent<Button>();

        var lbl = new GameObject("Label").AddComponent<Text>();
        lbl.transform.SetParent(go.transform,false);
        lbl.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        lbl.text = label;
        lbl.alignment = TextAnchor.MiddleCenter;
        lbl.fontSize = 20;
        lbl.color = Color.white;
        lbl.rectTransform.sizeDelta = new Vector2(240,60);
        return btn;
    }

    void CheckToggles()
    {
        bool ready = tPrivacy.isOn && tTerms.isOn && tEula.isOn;
        continueBtn.interactable = ready;
        warning.text = ready ? "" : "Для продолжения отметьте все пункты";
    }

    void AcceptAll()
    {
        PlayerPrefs.SetInt("user_has_agreed", 1);
        PlayerPrefs.Save();
        panel.SetActive(false);
    }
}
