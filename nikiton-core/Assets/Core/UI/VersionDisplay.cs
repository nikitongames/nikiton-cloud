using UnityEngine;
using UnityEngine.UI;
using Nikiton.Core.System;

namespace Nikiton.Core.UI
{
    public class VersionDisplay : MonoBehaviour
    {
        public Text versionText;
        public GameSettings settings;

        void Start()
        {
            versionText.text = "Version: " + settings.version;
        }
    }
}
