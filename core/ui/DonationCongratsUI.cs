using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DonationCongratsUI : MonoBehaviour
{
    // Статический запуск из любого места: DonationCongratsUI.Show(level, title, body, gold, ncoin);
    public static void Show(int gratitudeLevel, string title, string body, int gold, int ncoin)
    {
        // Если в сцене уже есть — не дублируем
        if (FindObjectOfType<DonationCongratsUI>() != null) return;

        var host = new GameObject("DonationCongratsUI").AddComponent<DonationCongratsUI>();
        host.StartCoroutine(host.BuildAndPlay(gratitudeLevel, title, body, gold, ncoin));
    }

    private Canvas _canvas;
    private Image _backdrop;
    private RectTransform _panel;
    private Button _closeBtn;

    private IEnumerator BuildAndPlay(int level, string title, string body, int gold, int ncoin)
    {
        // Canvas
        _canvas = gameObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 5000;
        gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        gameObject.AddComponent<GraphicRaycaster>();

        // Фон-затемнение
        var backdropGO = new GameObject("Backdrop");
        backdropGO.transform.SetParent(transform, false);
        _backdrop = backdropGO.AddComponent<Image>();
        _backdrop.color = new Color(0, 0, 0, 0); // анимируем в 0.6
        var bdRt = backdropGO.GetComponent<RectTransform>();
        bdRt.anchorMin = Vector2.zero; bdRt.anchorMax = Vector2.one; bdRt.offsetMin = Vector2.zero; bdRt.offsetMax = Vector2.zero;

        // Панель
        var panelGO = new GameObject("Panel");
        panelGO.transform.SetParent(transform, false);
        _panel = panelGO.AddComponent<RectTransform>();
        _panel.sizeDelta = new Vector2(720, 460);
        _panel.localScale = Vector3.one * 0.8f;
        var panelImg = panelGO.AddComponent<Image>();
        panelImg.color = new Color(0.11f, 0.13f, 0.16f, 0.98f); // #1C2228
        panelImg.raycastTarget = true;
        var panelShadow = panelGO.AddComponent<Shadow>(); panelShadow.effectDistance = new Vector2(0, -6);

        // Центрирование
        _panel.anchorMin = new Vector2(0.5f, 0.5f);
        _panel.anchorMax = new Vector2(0.5f, 0.5f);
        _panel.anchoredPosition = Vector2.zero;

        // Логотип (опционально из Resources/nikiton_logo)
        Image logo = null;
        var logoGO = new GameObject("Logo");
        logoGO.transform.SetParent(_panel, false);
        logo = logoGO.AddComponent<Image>();
        var logoRt = logoGO.GetComponent<RectTransform>();
        logoRt.anchorMin = new Vector2(0.5f, 1f);
        logoRt.anchorMax = new Vector2(0.5f, 1f);
        logoRt.pivot = new Vector2(0.5f, 1f);
        logoRt.anchoredPosition = new Vector2(0, -24);
        logoRt.sizeDelta = new Vector2(160, 64);

        // Попытка загрузить спрайт из Resources/nikiton_logo (PNG без расширения)
        var logoTex = Resources.Load<Texture2D>("nikiton_logo");
        if (logoTex != null)
        {
            var rect = new Rect(0, 0, logoTex.width, logoTex.height);
            var sp = Sprite.Create(logoTex, rect, new Vector2(0.5f, 0.5f), 100f);
            logo.sprite = sp;
            logo.preserveAspect = true;
        }
        else
        {
            // Если лого нет — делаем декоративную полоску
            logo.color = new Color(1f, 1f, 1f, 0.06f);
        }

        // Заголовок
        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(_panel, false);
        var titleTxt = titleGO.AddComponent<TextMeshProUGUI>();
        titleTxt.text = string.IsNullOrEmpty(title) ? $"Поздравляем! Уровень благодарности {level} 🎉" : title;
        titleTxt.fontSize = 30;
        titleTxt.alignment = TextAlignmentOptions.Center;
        titleTxt.color = new Color(1f, 0.84f, 0.35f); // золотистый
        var titleRt = titleGO.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.5f, 1f);
        titleRt.anchorMax = new Vector2(0.5f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.anchoredPosition = new Vector2(0, -110);
        titleRt.sizeDelta = new Vector2(660, 40);

        // Подзаголовок / описание
        var bodyGO = new GameObject("Body");
        bodyGO.transform.SetParent(_panel, false);
        var bodyTxt = bodyGO.AddComponent<TextMeshProUGUI>();
        bodyTxt.text = string.IsNullOrEmpty(body)
            ? "Спасибо за поддержку Nikiton Games! Ваши бонусы зачислены на аккаунт."
            : body;
        bodyTxt.fontSize = 20;
        bodyTxt.alignment = TextAlignmentOptions.Center;
        bodyTxt.color = new Color(0.9f, 0.92f, 0.96f);
        var bodyRt = bodyGO.GetComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0.5f, 1f);
        bodyRt.anchorMax = new Vector2(0.5f, 1f);
        bodyRt.pivot = new Vector2(0.5f, 1f);
        bodyRt.anchoredPosition = new Vector2(0, -150);
        bodyRt.sizeDelta = new Vector2(660, 60);

        // Ревард-блок
        var rewardsGO = new GameObject("Rewards");
        rewardsGO.transform.SetParent(_panel, false);
        var rewardsTxt = rewardsGO.AddComponent<TextMeshProUGUI>();
        rewardsTxt.text = $"Награды: <color=#FFD700>+{gold:N0} Gold</color>  •  <color=#FFD700>+{ncoin} Ncoin</color>";
        rewardsTxt.fontSize = 22;
        rewardsTxt.alignment = TextAlignmentOptions.Center;
        rewardsTxt.color = new Color(0.95f, 0.96f, 0.98f);
        var rewardsRt = rewardsGO.GetComponent<RectTransform>();
        rewardsRt.anchorMin = new Vector2(0.5f, 1f);
        rewardsRt.anchorMax = new Vector2(0.5f, 1f);
        rewardsRt.pivot = new Vector2(0.5f, 1f);
        rewardsRt.anchoredPosition = new Vector2(0, -205);
        rewardsRt.sizeDelta = new Vector2(660, 40);

        // Кнопка закрытия
        var closeGO = new GameObject("CloseButton");
        closeGO.transform.SetParent(_panel, false);
        _closeBtn = closeGO.AddComponent<Button>();
        var closeImg = closeGO.AddComponent<Image>();
        closeImg.color = new Color(0.18f, 0.22f, 0.28f); // тёмная кнопка
        var closeRt = closeGO.GetComponent<RectTransform>();
        closeRt.anchorMin = new Vector2(0.5f, 0f);
        closeRt.anchorMax = new Vector2(0.5f, 0f);
        closeRt.pivot = new Vector2(0.5f, 0f);
        closeRt.anchoredPosition = new Vector2(0, 28);
        closeRt.sizeDelta = new Vector2(220, 48);

        var closeLabelGO = new GameObject("Label");
        closeLabelGO.transform.SetParent(closeGO.transform, false);
        var closeLabel = closeLabelGO.AddComponent<TextMeshProUGUI>();
        closeLabel.text = "Продолжить";
        closeLabel.fontSize = 22;
        closeLabel.alignment = TextAlignmentOptions.Center;
        closeLabel.color = new Color(0.95f, 0.96f, 0.98f);
        var clRt = closeLabelGO.GetComponent<RectTransform>();
        clRt.anchorMin = Vector2.zero; clRt.anchorMax = Vector2.one; clRt.offsetMin = Vector2.zero; clRt.offsetMax = Vector2.zero;

        _closeBtn.onClick.AddListener(() => StartCoroutine(FadeOutAndDestroy()));

        // Анимация появления
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        float dur = 0.35f;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / dur);
            _backdrop.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.6f, k));
            _panel.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, k);
            yield return null;
        }
        _backdrop.color = new Color(0, 0, 0, 0.6f);
        _panel.localScale = Vector3.one;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float t = 0f;
        float dur = 0.25f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / dur);
            _backdrop.color = new Color(0, 0, 0, Mathf.Lerp(0.6f, 0f, k));
            _panel.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.9f, k);
            yield return null;
        }
        Destroy(gameObject);
    }
}
