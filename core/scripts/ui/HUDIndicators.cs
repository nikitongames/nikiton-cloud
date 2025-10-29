using UnityEngine;
using UnityEngine.UI;

public class HUDIndicators : MonoBehaviour
{
    Image rod, reel, line, energy;
    Slider rodSlider, reelSlider, lineSlider, energySlider;

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        var p = new GameObject("IndicatorsPanel");
        var rt = p.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform,false);
        rt.sizeDelta = new Vector2(300,160);
        rt.anchorMin = new Vector2(0,1);
        rt.anchorMax = new Vector2(0,1);
        rt.pivot = new Vector2(0,1);
        rt.anchoredPosition = new Vector2(20,-20);

        rodSlider = CreateBar("Удочка", p.transform, new Vector2(0, -20));
        reelSlider = CreateBar("Катушка", p.transform, new Vector2(0, -60));
        lineSlider = CreateBar("Леска", p.transform, new Vector2(0, -100));
        energySlider = CreateBar("Энергия", p.transform, new Vector2(0, -140));
    }

    Slider CreateBar(string name, Transform parent, Vector2 pos)
    {
        var go = new GameObject(name);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent,false);
        rt.sizeDelta = new Vector2(260,20);
        rt.anchoredPosition = pos;
        var slider = go.AddComponent<Slider>();
        slider.value = 1f;
        return slider;
    }

    public void UpdateIndicators(float rodV, float reelV, float lineV, float energyV)
    {
        rodSlider.value = rodV;
        reelSlider.value = reelV;
        lineSlider.value = lineV;
        energySlider.value = energyV;
    }
}
