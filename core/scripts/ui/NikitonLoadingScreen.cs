using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class NikitonLoadingScreen : MonoBehaviour
{
    public static IEnumerator ShowLoading()
    {
        string configPath = "core/configs/system/core_config.json";
        string json = File.ReadAllText(configPath);
        var config = JsonUtility.FromJson<CoreConfig>(json);
        string gameName = config.current_game;

        GameObject canvasObj = new GameObject("LoadingCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        Image bg = new GameObject("Background").AddComponent<Image>();
        bg.transform.SetParent(canvas.transform, false);
        bg.sprite = Resources.Load<Sprite>($"games/{gameName}/assets/ui/loading/background");
        bg.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        bg.color = Color.white;

        Text txt = new GameObject("LoadingText").AddComponent<Text>();
        txt.text = "Загрузка данных с NikitoN Cloud...";
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.color = Color.white;
        txt.fontSize = 26;
        txt.alignment = TextAnchor.LowerCenter;
        txt.transform.SetParent(canvas.transform, false);
        txt.rectTransform.anchoredPosition = new Vector2(0, -200);

        yield return new WaitForSeconds(2.5f);
    }

    [System.Serializable]
    public class CoreConfig
    {
        public string current_game;
    }
}
