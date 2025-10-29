using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TournamentUI : MonoBehaviour
{
    GameObject panel;
    RectTransform listContainer;
    Text header;
    Button close;

    void Start()
    {
        BuildUI();
    }

    void BuildUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        panel = new GameObject("TournamentPanel");
        var rt = panel.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform,false);
        rt.sizeDelta = new Vector2(720,520);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f,0.5f);
        rt.pivot = new Vector2(0.5f,0.5f);
        var bg = panel.AddComponent<Image>();
        bg.color = new Color(0,0,0,0.8f);

        header = new GameObject("Header").AddComponent<Text>();
        header.transform.SetParent(panel.transform,false);
        header.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        header.text = "Глобальные турниры";
        header.fontSize = 26;
        header.color = Color.white;
        header.rectTransform.anchoredPosition = new Vector2(0,220);

        var listGO = new GameObject("List");
        listContainer = listGO.AddComponent<RectTransform>();
        listContainer.SetParent(panel.transform,false);
        listContainer.sizeDelta = new Vector2(660,400);
        listContainer.anchoredPosition = new Vector2(0,-20);

        close = new GameObject("Закрыть").AddComponent<Button>();
        close.transform.SetParent(panel.transform,false);
        close.GetComponent<RectTransform>().sizeDelta = new Vector2(240,50);
        close.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-230);
        close.onClick.AddListener(()=> panel.SetActive(false));

        panel.SetActive(false);
    }

    public void Show(List<string> tournaments)
    {
        foreach (Transform t in listContainer) Destroy(t.gameObject);
        foreach (var name in tournaments)
        {
            var t = new GameObject("Item");
            t.transform.SetParent(listContainer,false);
            var txt = t.AddComponent<Text>();
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.text = name;
            txt.fontSize = 20;
            txt.color = Color.white;
        }
        panel.SetActive(true);
    }
}
