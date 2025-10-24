using System;
using UnityEngine;

namespace NikitonCore
{
    /// <summary>
    /// Загрузка и хранение глобальных настроек ядра из Settings.json.
    /// </summary>
    public static class Settings
    {
        [Serializable]
        public class Data
        {
            public string CoreVersion = "1.0.0";
            public CloudBlock Cloud = new CloudBlock();
            public LegalBlock Legal = new LegalBlock();

            [Serializable] public class CloudBlock
            {
                public string BaseURL = "https://github.com/nikitongames/nikiton-cloud/";
                public string CorePath = "core/";
                public string LegalPath = "legal/";
            }

            [Serializable] public class LegalBlock
            {
                public bool RequireConsent = true;
            }
        }

        private static Data _cached;
        public static Data Current
        {
            get
            {
                if (_cached == null) Load();
                return _cached;
            }
        }

        public static void Load()
        {
            Logger.Log("Settings loaded from cloud (stub).");
            _cached = new Data();
        }
    }
}
