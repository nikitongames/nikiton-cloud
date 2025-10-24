using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace NikitonCore.Localization
{
    /// <summary>
    /// Менеджер локализации Nikiton Cloud.
    /// Загружает JSON-файлы с переводами и выдаёт текст по ключам.
    /// </summary>
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        private Dictionary<string, string> localizedTexts = new Dictionary<string, string>();
        private string currentLanguage = "en";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadLanguage(currentLanguage);
        }

        public void LoadLanguage(string lang)
        {
            currentLanguage = lang;
            localizedTexts.Clear();

            TextAsset jsonFile = Resources.Load<TextAsset>($"Localization/lang_{lang}");
            if (jsonFile == null)
            {
                Logger.Warn($"Localization file not found for language: {lang}");
                return;
            }

            var dict = JsonUtility.FromJson<SerializableDictionary>(jsonFile.text);
            if (dict != null)
            {
                foreach (var pair in dict.items)
                {
                    localizedTexts[pair.key] = pair.value;
                }
            }

            Logger.Log($"Loaded language: {lang}");
        }

        public string Get(string key)
        {
            if (localizedTexts.TryGetValue(key, out string value))
                return value;
            return $"#{key}";
        }

        [System.Serializable]
        private class SerializableDictionary
        {
            public List<Item> items;
            [System.Serializable]
            public class Item
            {
                public string key;
                public string value;
            }
        }
    }
}
