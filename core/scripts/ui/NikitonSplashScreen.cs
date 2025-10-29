using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NikitonSplashScreen : MonoBehaviour
{
    public static IEnumerator ShowLogo()
    {
        GameObject canvasObj = new GameObject("NikitoNCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        Image logo = new GameObject("Logo").AddComponent<Image>();
        logo.transform.SetParent(canvas.transform, false);
        logo.sprite = Resources.Load<Sprite>("core/assets/ui/logo/nikiton_logo");
        logo.color = new Color(1,1,1,0);
        logo.rectTransform.sizeDelta = new Vector2(800, 400);
        logo.rectTransform.anchoredPosition = Vector2.zero;

        for (float t = 0; t < 1.5f; t += Time.deltaTime)
        {
            logo.color = new Color(1,1,1,t/1.5f);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            logo.color = new Color(1,1,1,1 - t);
            yield return null;
        }

        Object.Destroy(canvasObj);
    }
}
