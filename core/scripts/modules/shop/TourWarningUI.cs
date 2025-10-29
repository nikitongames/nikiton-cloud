using UnityEngine;
using UnityEngine.UI;

public class TourWarningUI : MonoBehaviour
{
    public static System.Collections.IEnumerator Show(string message, System.Action<bool> onChoice)
    {
        var canvasGO = new GameObject("TourWarnCanvas");
        var c = canvasGO.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        var bg = new GameObject("BG").AddComponent<Image>();
        bg.color = new Color(0,0,0,0.75f);
        bg.rectTransform.SetParent(c.transform,false);
        bg.rectTransform.anchorMin = Vector2.zero; bg.rectTransform.anchorMax = Vector2.one;

        var box = new GameObject("Box").AddComponent<Image>();
        box.color = new Color(0.15f,0.15f,0.15f,1);
        box.rectTransform.SetParent(c.transform,false);
        box.rectTransform.sizeDelta = new Vector2(820,260);

        var txt = new GameObject("Txt").AddComponent<Text>();
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.color = Color.white; txt.fontSize = 24; txt.alignment = TextAnchor.MiddleCenter;
        txt.rectTransform.SetParent(box.transform,false);
        txt.rectTransform.sizeDelta = new Vector2(780,140);
        txt.text = message;

        var y = MakeBtn(box.transform, "Да, купить", new Vector2(-160,-80));
        var n = MakeBtn(box.transform, "Отмена", new Vector2(160,-80));

        bool decided=false, result=false;
        y.onClick.AddListener(()=>{decided=true; result=true;});
        n.onClick.AddListener(()=>{decided=true; result=false;});

        while(!decided) yield return null;
        GameObject.Destroy(canvasGO);
        onChoice?.Invoke(result);
    }

    static Button MakeBtn(Transform parent, string label, Vector2 pos)
    {
        var bgo = new GameObject("Btn");
        var img = bgo.AddComponent<Image>(); img.color = new Color(0.25f,0.55f,0.25f,1);
        var b = bgo.AddComponent<Button>();
        bgo.transform.SetParent(parent,false);
        (b.transform as RectTransform).sizeDelta = new Vector2(280,56);
        (b.transform as RectTransform).anchoredPosition = pos;
        var t = new GameObject("T").AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.alignment = TextAnchor.MiddleCenter; t.color = Color.white; t.fontSize = 22;
        t.text = label; t.rectTransform.SetParent(b.transform,false);
        return b;
    }
}
