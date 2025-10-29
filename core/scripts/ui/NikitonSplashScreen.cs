using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NikitonSplashScreen : MonoBehaviour
{
    public static IEnumerator ShowLogo()
    {
        GameObject logo = new GameObject("NikitoNLogo");
        Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Image img = logo.AddComponent<Image>();
        img.sprite = Resources.Load<Sprite>("core/assets/ui/logo/nikiton_logo");

        logo.transform.SetParent(canvas.transform, false);
        logo.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 400);
        logo.GetComponent<Image>().color = new Color(1,1,1,0);

        for(float t=0; t<1.5f; t+=Time.deltaTime)
        {
            img.color = new Color(1,1,1,t/1.5f);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        for(float t=0; t<1f; t+=Time.deltaTime)
        {
            img.color = new Color(1,1,1,1-t);
            yield return null;
        }

        Destroy(canvas.gameObject);
    }
}
