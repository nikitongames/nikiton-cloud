using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NikitonLoadingScreen : MonoBehaviour
{
    public static IEnumerator ShowLoading()
    {
        GameObject loadingScreen = new GameObject("LoadingScreen");
        Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Image background = loadingScreen.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>("core/assets/ui/loading/fisherman_background");
        background.color = new Color(1,1,1,1);
        loadingScreen.transform.SetParent(canvas.transform, false);

        Text loadingText = new GameObject("LoadingText").AddComponent<Text>();
        loadingText.text = "Загрузка данных с NikitoN Cloud...";
        loadingText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        loadingText.alignment = TextAnchor.LowerCenter;
        loadingText.color = Color.white;
        loadingText.fontSize = 28;
        loadingText.transform.SetParent(canvas.transform, false);
        loadingText.rectTransform.anchoredPosition = new Vector2(0, -200);

        yield return new WaitForSeconds(2.0f);
    }
}
