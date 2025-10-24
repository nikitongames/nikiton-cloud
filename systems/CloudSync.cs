using UnityEngine;
using System.Collections;

namespace NikitonCore.Systems
{
    /// <summary>
    /// Проверка обновлений и синхронизация с Nikiton Cloud.
    /// </summary>
    public class CloudSync : MonoBehaviour
    {
        public static CloudSync Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartCoroutine(CheckForUpdatesRoutine());
        }

        private IEnumerator CheckForUpdatesRoutine()
        {
            Logger.Log("Checking Nikiton Cloud for updates...");
            yield return new WaitForSeconds(2f);
            Logger.Log("No updates found. Cloud sync up-to-date (stub).");
        }
    }
}
