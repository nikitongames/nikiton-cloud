using UnityEngine;

namespace Nikiton.Core.System
{
    public class SystemInitializer : MonoBehaviour
    {
        public GameSettings settings;

        void Start()
        {
            if (settings.enableCloudSync)
                Debug.Log("Cloud sync enabled: " + settings.cloudSyncURL);

            if (settings.enableDonations)
                Debug.Log("Donations system active");

            if (settings.enableEvents)
                Debug.Log("Event system active");
        }
    }
}
