using UnityEngine;

namespace NikitonCore.Systems
{
    /// <summary>
    /// Система благодарностей — считает общую сумму пожертвований игрока.
    /// </summary>
    public static class GratitudeSystem
    {
        private static float totalDonations = 0f;

        public static void AddDonation(float amount)
        {
            totalDonations += amount;
            Logger.Log($"[Gratitude] Total donations: {totalDonations}$");
        }

        public static float GetTotal()
        {
            return totalDonations;
        }

        public static void Reset()
        {
            totalDonations = 0;
        }
    }
}
