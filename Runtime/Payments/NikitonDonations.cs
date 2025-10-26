using UnityEngine;
using UnityEngine.Purchasing;

namespace Nikiton.Cloud.Payments
{
    // Простейшая обёртка для вызова покупок. Сами продукты задаются в проекте игры.
    public static class NikitonDonations
    {
        static IStoreController ctrl;

        public static void Bind(IStoreController controller) => ctrl = controller;

        public static bool Purchase(string productId)
        {
            if (ctrl == null) { Debug.LogWarning("[IAP] Controller not ready"); return false; }
            ctrl.InitiatePurchase(productId);
            return true;
        }
    }
}
