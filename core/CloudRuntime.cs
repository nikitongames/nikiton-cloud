namespace NikitonCore
{
    /// <summary>
    /// Управление взаимодействием с Nikiton Cloud.
    /// </summary>
    public static class CloudRuntime
    {
        public static bool IsReady { get; private set; }

        public static void Init()
        {
            IsReady = true;
            Logger.Log("Cloud runtime initialized (stub).");
        }
    }
}
