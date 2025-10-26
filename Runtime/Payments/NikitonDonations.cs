using UnityEngine;
using UnityEngine.Purchasing;

namespace Nikiton.Cloud.Payments
{
    public static class NikitonDonations
    {
        static IStoreController controller;

        public static void Bind(IStoreController ctrl) => controller = ctrl;

        public static bool Purchase(string productId)
        {
            if (controller == null)
            {
                Debug.LogWarning("[IAP] Controller not ready");
                return false;
            }
            controller.InitiatePurchase(productId);
            return true;
        }
    }
}
