using System.Collections.Generic;
using UnityEngine;

namespace Nikiton.Cloud.Localization
{
    public static class Localization
    {
        static Dictionary<string, string> table = new();

        public static void Apply(string lang)
        {
            var txt = Resources.Load<TextAsset>($"Localization/{lang}");
            table.Clear();
            if (txt)
            {
                foreach (var line in txt.text.Split('\n'))
                {
                    var p = line.Split('=');
                    if (p.Length == 2) table[p[0].Trim()] = p[1].Trim();
                }
            }
            Debug.Log($"[Localization] Loaded {lang}, keys: {table.Count}");
        }

        public static string T(string key)
            => table.TryGetValue(key, out var v) ? v : key;
    }
}
