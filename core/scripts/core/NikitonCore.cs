using UnityEngine;
using System.Collections;

public class NikitonCore : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return NikitonSplashScreen.ShowLogo();
        yield return NikitonLoadingScreen.ShowLoading();
        yield return NikitonUpdater.CheckForUpdates();
    }
}
