using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LegalManager : MonoBehaviour
{
    GameObject panel;
    Text bodyText;
    Dropdown selectDropdown;
    Button closeButton;

    void Start()
    {
        BuildUI();
    }

    void BuildUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        var go = new GameObject("LegalPanel");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(720, 540);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        var img = go.AddComponent<Image>();
        img.color = new Color(0,0,0,0.85f);

        selectDropdown = new GameObject("Selector").AddComponent<Dropdown>();
        selectDropdown.transform.SetParent(go.transform,false);
        selectDropdown.AddOptions(new System.Collections.Generic.List<string>(){
            "Политика конфиденциальности","Условия использования","EULA","Политика доната","Отказ от ответственности"
        });
        selectDropdown.onValueChanged.AddListener(idx=> LoadText(idx));

        bodyText = new GameObject("Body").AddComponent<Text>();
        bodyText.transform.SetParent(go.transform,false);
        bodyText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        bodyText.color = Color.white;
        bodyText.fontSize = 18;
        bodyText.alignment = TextAnchor.UpperLeft;
        bodyText.horizontalOverflow = HorizontalWrapMode.Wrap;
        bodyText.verticalOverflow = VerticalWrapMode.Truncate;
        bodyText.rectTransform.sizeDelta = new Vector2(680, 400);
        bodyText.rectTransform.anchoredPosition = new Vector2(0,-40);

        closeButton = new GameObject("Закрыть").AddComponent<Button>();
        closeButton.transform.SetParent(go.transform,false);
        closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(220,50);
        closeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-230);
        closeButton.onClick.AddListener(()=> go.SetActive(false));

        go.SetActive(false);
        panel = go;
    }

    void LoadText(int idx)
    {
        string[] files = {
            "privacy_policy.json","terms_of_use.json","eula.json","donation_policy.json","disclaimer.json"
        };
        string path = Path.Combine("core/legal", files[idx]);
        if (File.Exists(path))
            bodyText.text = File.ReadAllText(path);
        else
            bodyText.text = "Файл не найден.";
    }

    public void Show() => panel.SetActive(true);
}
