using UnityEngine;

namespace NikitonCore
{
    public static class Logger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(string message)
        {
            Debug.Log($"[Nikiton Core] {message}");
        }

        public static void Warn(string message)
        {
            Debug.LogWarning($"[Nikiton Core] {message}");
        }

        public static void Error(string message)
        {
            Debug.LogError($"[Nikiton Core ERROR] {message}");
        }
    }
}
