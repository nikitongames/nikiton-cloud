using UnityEngine;
using System.Collections;

public class NikitonCore : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InitializeCore());
    }

    IEnumerator InitializeCore()
    {
        yield return NikitonSplashScreen.ShowLogo();
        yield return NikitonLoadingScreen.ShowLoading();
        yield return NikitonUpdater.CheckForUpdates();
    }
}
