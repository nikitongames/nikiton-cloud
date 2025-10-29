using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class CurrencyHUD : MonoBehaviour
{
    public Vector2 panelSize = new Vector2(520, 64);
    public Vector2 padding = new Vector2(16, 12); // отступ от краёв (справа-сверху)
    public float refreshInterval = 0.3f; // сек

    RectTransform panel;
    Image goldIconImg, ncoinIconImg;
    Text goldText, ncoinText;
    Button goldPlusBtn, ncoinPlusBtn;

    int lastGold = -1, lastNcoin = -1;

    IEnumerator Start()
    {
        BuildUI();
        yield return null;

        // периодическое обновление значений
        while (true)
        {
            UpdateValues();
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    void BuildUI()
    {
        // Canvas — ищем существующий или создаём
        Canvas canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            var cgo = new GameObject("CurrencyCanvas");
            canvas = cgo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cgo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cgo.AddComponent<GraphicRaycaster>();
        }

        // Панель
        var pgo = new GameObject("CurrencyPanel");
        panel = pgo.AddComponent<RectTransform>();
        var img = pgo.AddComponent<Image>();
        img.color = new Color(0f,0f,0f,0.45f);
        panel.SetParent(canvas.transform, false);
        panel.sizeDelta = panelSize;

        // позиция — правый верх с отступом
        panel.anchorMin = new Vector2(1,1);
        panel.anchorMax = new Vector2(1,1);
        panel.pivot = new Vector2(1,1);
        panel.anchoredPosition = new Vector2(-padding.x, -padding.y);

        // Блок GOLD
        var goldBlock = CreateCurrencyBlock(panel, "gold", out goldIconImg, out goldText, out goldPlusBtn);
        (goldBlock as RectTransform).anchoredPosition = new Vector2(-panelSize.x/2 + 120, 0);

        // Блок NCOIN
        var ncoinBlock = CreateCurrencyBlock(panel, "ncoin", out ncoinIconImg, out ncoinText, out ncoinPlusBtn);
        (ncoinBlock as RectTransform).anchoredPosition = new Vector2(+panelSize.x/2 - 120, 0);

        // Иконки
        var gi = CurrencyDisplay.GetIcon("gold");
        if (gi) goldIconImg.sprite = gi;
        var ni = CurrencyDisplay.GetIcon("ncoin");
        if (ni) ncoinIconImg.sprite = ni;

        // Плюс-кнопки (пока открывают условный магазин)
        goldPlusBtn.onClick.AddListener(()=> { Debug.Log("[HUD] GOLD + clicked → open shop (gold tab)"); });
        ncoinPlusBtn.onClick.AddListener(()=> { Debug.Log("[HUD] NCOIN + clicked → open shop (ncoin tab)"); });
    }

    GameObject CreateCurrencyBlock(Transform parent, string id, out Image icon, out Text text, out Button plus)
    {
        var block = new GameObject(id.ToUpper()+"_Block");
        var rt = block.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.sizeDelta = new Vector2(220, 48);

        // Иконка
        var igo = new GameObject("Icon");
        var irt = igo.AddComponent<RectTransform>();
        irt.SetParent(block.transform, false);
        irt.sizeDelta = new Vector2(36, 36);
        irt.anchoredPosition = new Vector2(-80, 0);
        icon = igo.AddComponent<Image>();

        // Текст
        var tgo = new GameObject("Text");
        var trt = tgo.AddComponent<RectTransform>();
        trt.SetParent(block.transform, false);
        trt.sizeDelta = new Vector2(120, 40);
        trt.anchoredPosition = new Vector2(-5, 0);
        text = tgo.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleLeft;
        text.color = Color.white;
        text.text = "—";

        // Кнопка +
        var pgo = new GameObject("Plus");
        var prt = pgo.AddComponent<RectTransform>();
        prt.SetParent(block.transform, false);
        prt.sizeDelta = new Vector2(36, 36);
        prt.anchoredPosition = new Vector2(+80, 0);
        var pimg = pgo.AddComponent<Image>();
        pimg.color = new Color(0.3f,0.6f,0.3f,1f);
        plus = pgo.AddComponent<Button>();

        // знак +
        var lgo = new GameObject("Label");
        var lrt = lgo.AddComponent<RectTransform>();
        lrt.SetParent(pgo.transform, false);
        lrt.sizeDelta = new Vector2(36, 36);
        var lt = lgo.AddComponent<Text>();
        lt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        lt.text = "+";
        lt.fontSize = 24;
        lt.alignment = TextAnchor.MiddleCenter;
        lt.color = Color.white;

        return block;
    }

    void UpdateValues()
    {
        if (PlayerProfile.Gold != lastGold)
        {
            lastGold = PlayerProfile.Gold;
            goldText.text = CurrencyDisplay.Format("gold", lastGold);
        }
        if (PlayerProfile.NCoin != lastNcoin)
        {
            lastNcoin = PlayerProfile.NCoin;
            ncoinText.text = CurrencyDisplay.Format("ncoin", lastNcoin);
        }
    }
}
