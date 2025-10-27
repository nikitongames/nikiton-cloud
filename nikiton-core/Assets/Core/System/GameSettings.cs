using UnityEngine;

namespace Nikiton.Core.System
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "NikitoN/Core/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public string version = "1.0.0";
        public string cloudSyncURL = "https://nikiton-cloud/api/";
        public bool enableDonations = true;
        public bool enableCloudSync = true;
        public bool enableEvents = true;
    }
}
