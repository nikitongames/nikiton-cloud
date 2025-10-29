using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NikitonLoadingScreen : MonoBehaviour
{
    Canvas canvas;
    GameObject logoObj, loadingTextObj;
    Image logoImage;
    Text loadingText;

    void Start()
    {
        BuildUI();
        StartCoroutine(ShowSequence());
    }

    void BuildUI()
    {
        canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            var cgo = new GameObject("NikitonCanvas");
            canvas = cgo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cgo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cgo.AddComponent<GraphicRaycaster>();
        }

        // Задний фон (индивидуальный для каждой игры)
        var bg = new GameObject("Background");
        var bgImg = bg.AddComponent<Image>();
        bgImg.color = Color.black;
        bg.transform.SetParent(canvas.transform, false);
        var rt = bg.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        // Логотип NikitoN Games
        logoObj = new GameObject("NikitonLogo");
        logoObj.transform.SetParent(canvas.transform, false);
        logoImage = logoObj.AddComponent<Image>();
        logoImage.color = new Color(1, 1, 1, 0);
        var logoRT = logoObj.GetComponent<RectTransform>();
        logoRT.sizeDelta = new Vector2(400, 160);
        logoRT.anchorMin = new Vector2(0.5f, 0.5f);
        logoRT.anchorMax = new Vector2(0.5f, 0.5f);
        logoRT.pivot = new Vector2(0.5f, 0.5f);
        logoRT.anchoredPosition = Vector2.zero;
        logoImage.sprite = Resources.Load<Sprite>("assets/splash/logo");

        // Текст "Загрузка данных..."
        loadingTextObj = new GameObject("LoadingText");
        loadingTextObj.transform.SetParent(canvas.transform, false);
        loadingText = loadingTextObj.AddComponent<Text>();
        loadingText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        loadingText.text = "Загрузка данных с Nikiton Cloud…";
        loadingText.alignment = TextAnchor.MiddleCenter;
        loadingText.fontSize = 22;
        loadingText.color = new Color(1, 1, 1, 0);
        var txtRT = loadingTextObj.GetComponent<RectTransform>();
        txtRT.anchorMin = new Vector2(0.5f, 0.15f);
        txtRT.anchorMax = new Vector2(0.5f, 0.15f);
        txtRT.pivot = new Vector2(0.5f, 0.5f);
        txtRT.anchoredPosition = Vector2.zero;
        txtRT.sizeDelta = new Vector2(800, 80);
    }

    IEnumerator ShowSequence()
    {
        // Плавное появление логотипа
        for (float t = 0; t < 1f; t += Time.deltaTime / 1.5f)
        {
            logoImage.color = new Color(1, 1, 1, t);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        // Плавное исчезновение логотипа
        for (float t = 1f; t > 0f; t -= Time.deltaTime / 1.5f)
        {
            logoImage.color = new Color(1, 1, 1, t);
            yield return null;
        }

        // Отображаем предупреждение
        ShowDisclaimer("Все персонажи и события вымышлены. Любые совпадения случайны.");

        yield return new WaitForSeconds(1.2f);

        // Появление текста "Загрузка данных..."
        for (float t = 0; t < 1f; t += Time.deltaTime / 1f)
        {
            loadingText.color = new Color(1, 1, 1, t);
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    void ShowDisclaimer(string message)
    {
        var go = new GameObject("DisclaimerText");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(800, 60);
        rt.anchorMin = new Vector2(0.5f, 0);
        rt.anchorMax = new Vector2(0.5f, 0);
        rt.pivot = new Vector2(0.5f, 0);
        rt.anchoredPosition = new Vector2(0, 40);

        var txt = go.AddComponent<Text>();
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.text = message;
        txt.fontSize = 16;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = new Color(1, 1, 1, 0);

        // Плавное появление текста
        StartCoroutine(FadeDisclaimer(txt));
    }

    IEnumerator FadeDisclaimer(Text txt)
    {
        for (float t = 0; t < 1f; t += Time.deltaTime / 1.2f)
        {
            txt.color = new Color(1, 1, 1, t);
            yield return null;
        }

        yield return new WaitForSeconds(3.5f);

        for (float t = 1f; t > 0f; t -= Time.deltaTime / 1.2f)
        {
            txt.color = new Color(1, 1, 1, t);
            yield return null;
        }

        Destroy(txt.gameObject);
    }
}
