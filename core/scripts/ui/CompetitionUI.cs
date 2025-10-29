using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CompetitionUI : MonoBehaviour
{
    public GameObject panel;
    public RectTransform listContainer;
    public GameObject listItemPrefab;
    public Button createButton;
    public Button refreshButton;
    public Text headerText;

    List<CompetitionData> competitions = new List<CompetitionData>();

    void Start()
    {
        BuildUI();
        RefreshList();
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

        panel = new GameObject("CompetitionPanel");
        var rt = panel.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(800, 600);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        var bg = panel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.8f);

        headerText = CreateText("Состязания", panel.transform, 32, new Vector2(0, 260));

        var scrollGO = new GameObject("ScrollView");
        var srt = scrollGO.AddComponent<RectTransform>();
        srt.SetParent(panel.transform, false);
        srt.sizeDelta = new Vector2(760, 400);
        srt.anchoredPosition = new Vector2(0, -20);
        var scroll = scrollGO.AddComponent<ScrollRect>();
        scroll.horizontal = false;
        var mask = scrollGO.AddComponent<Mask>();
        mask.showMaskGraphic = false;
        var img = scrollGO.AddComponent<Image>();
        img.color = new Color(1,1,1,0.1f);

        var contentGO = new GameObject("ListContainer");
        listContainer = contentGO.AddComponent<RectTransform>();
        listContainer.SetParent(scrollGO.transform, false);
        listContainer.sizeDelta = new Vector2(740, 10);
        scroll.content = listContainer;

        listItemPrefab = CreateListItemPrefab();

        createButton = CreateButton("Создать состязание", panel.transform, new Vector2(-160, -260));
        createButton.onClick.AddListener(()=> CreateCompetition());

        refreshButton = CreateButton("Обновить", panel.transform, new Vector2(+160, -260));
        refreshButton.onClick.AddListener(()=> RefreshList());
    }

    Text CreateText(string txt, Transform parent, int size, Vector2 pos)
    {
        var go = new GameObject("Text");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.sizeDelta = new Vector2(600, 40);
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
        rt.sizeDelta = new Vector2(280, 50);
        rt.anchoredPosition = pos;
        var img = go.AddComponent<Image>();
        img.color = new Color(0.2f,0.6f,0.9f,1f);
        var btn = go.AddComponent<Button>();

        var textGO = new GameObject("Label");
        var trt = textGO.AddComponent<RectTransform>();
        trt.SetParent(go.transform, false);
        trt.sizeDelta = new Vector2(260, 40);
        var t = textGO.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = label;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 20;
        t.color = Color.white;

        return btn;
    }

    GameObject CreateListItemPrefab()
    {
        var go = new GameObject("ListItemPrefab");
        var rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(720, 80);
        var img = go.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.1f);

        var nameT = new GameObject("Name");
        var rtName = nameT.AddComponent<RectTransform>();
        rtName.SetParent(go.transform, false);
        rtName.anchoredPosition = new Vector2(-220, 0);
        rtName.sizeDelta = new Vector2(200, 60);
        var tName = nameT.AddComponent<Text>();
        tName.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        tName.fontSize = 18;
        tName.color = Color.white;
        tName.text = "Название";

        var btnJoinGO = new GameObject("JoinButton");
        var rtBtn = btnJoinGO.AddComponent<RectTransform>();
        rtBtn.SetParent(go.transform, false);
        rtBtn.anchoredPosition = new Vector2(260, 0);
        rtBtn.sizeDelta = new Vector2(160, 50);
        var imgB = btnJoinGO.AddComponent<Image>();
        imgB.color = new Color(0.3f,0.7f,0.3f,1f);
        var btn = btnJoinGO.AddComponent<Button>();

        var txtB = new GameObject("Label");
        var trt = txtB.AddComponent<RectTransform>();
        trt.SetParent(btnJoinGO.transform, false);
        trt.sizeDelta = new Vector2(140, 40);
        var tB = txtB.AddComponent<Text>();
        tB.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        tB.text = "Присоединиться";
        tB.alignment = TextAnchor.MiddleCenter;
        tB.fontSize = 16;
        tB.color = Color.white;

        go.SetActive(false);
        return go;
    }

    void RefreshList()
    {
        // очистка списка
        foreach (Transform child in listContainer) Destroy(child.gameObject);

        competitions = CompetitionManager.GetActiveCompetitions(); // из модуля ядра
        foreach (var c in competitions)
        {
            var item = Instantiate(listItemPrefab, listContainer);
            item.SetActive(true);
            item.transform.Find("Name").GetComponent<Text>().text = $"{c.creator} — {c.mode.ToUpper()} ({c.participants.Count}/{c.max_players})";

            var btn = item.transform.Find("JoinButton").GetComponent<Button>();
            btn.onClick.AddListener(()=> JoinCompetition(c));
        }
    }

    void JoinCompetition(CompetitionData c)
    {
        CompetitionManager.JoinCompetition(c.id);
        RefreshList();
    }

    void CreateCompetition()
    {
        CompetitionManager.CreateCompetition("biggest_fish");
        RefreshList();
    }
}
