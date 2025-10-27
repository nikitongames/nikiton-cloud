using UnityEngine;
using System.IO;

namespace Nikiton.Core.Data
{
    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath + "/save.json";

        public static void Save(string json)
        {
            File.WriteAllText(path, json);
            Debug.Log("Game saved to: " + path);
        }

        public static string Load()
        {
            return File.Exists(path) ? File.ReadAllText(path) : "{}";
        }
    }
}
