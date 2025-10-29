using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CompetitionNotifications : MonoBehaviour
{
    public static CompetitionNotifications Instance;
    Queue<NotifyMessage> queue = new Queue<NotifyMessage>();
    GameObject panel;
    Text titleText, bodyText;
    Button actionButton, closeButton;
    bool showing = false;

    void Awake()
    {
        if (Instance) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildUI();
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

        panel = new GameObject("CompetitionNotification");
        var rt = panel.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0, -80);
        rt.sizeDelta = new Vector2(620, 140);
        var img = panel.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.75f);

        titleText = CreateText("Состязание", panel.transform, 24, new Vector2(0, -30));
        bodyText  = CreateText("Описание события", panel.transform, 18, new Vector2(0, -70));

        actionButton = CreateButton("Открыть", panel.transform, new Vector2(-100, -110));
        closeButton  = CreateButton("×", panel.transform, new Vector2(+240, -40));
        closeButton.GetComponentInChildren<Text>().fontSize = 24;
        closeButton.onClick.AddListener(()=> Hide());

        panel.SetActive(false);
    }

    Text CreateText(string txt, Transform parent, int size, Vector2 pos)
    {
        var go = new GameObject("Text");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.sizeDelta = new Vector2(580, 40);
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
        rt.sizeDelta = new Vector2(200, 44);
        rt.anchoredPosition = pos;
        var img = go.AddComponent<Image>();
        img.color = new Color(0.25f, 0.6f, 0.9f, 1f);
        var btn = go.AddComponent<Button>();

        var textGO = new GameObject("Label");
        var trt = textGO.AddComponent<RectTransform>();
        trt.SetParent(go.transform, false);
        trt.sizeDelta = new Vector2(200, 40);
        var t = textGO.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = label;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 18;
        t.color = Color.white;

        return btn;
    }

    public void Notify(string title, string message, System.Action onClick)
    {
        queue.Enqueue(new NotifyMessage{title=title, body=message, onClick=onClick});
        if (!showing) StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        showing = true;
        while (queue.Count > 0)
        {
            var msg = queue.Dequeue();
            Show(msg);
            yield return new WaitUntil(()=> !panel.activeSelf);
        }
        showing = false;
    }

    void Show(NotifyMessage msg)
    {
        titleText.text = msg.title;
        bodyText.text  = msg.body;
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(()=> { msg.onClick?.Invoke(); Hide(); });
        panel.SetActive(true);
    }

    void Hide()
    {
        panel.SetActive(false);
    }

    class NotifyMessage
    {
        public string title;
        public string body;
        public System.Action onClick;
    }
}
