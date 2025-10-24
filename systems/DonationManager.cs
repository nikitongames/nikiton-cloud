using UnityEngine;
using System.Collections.Generic;

namespace NikitonCore.Systems
{
    /// <summary>
    /// Управление донатами. Связан с GratitudeSystem.
    /// </summary>
    public class DonationManager : MonoBehaviour
    {
        public static DonationManager Instance { get; private set; }
        public List<DonationItem> donationItems = new List<DonationItem>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Donate(string itemId)
        {
            var item = donationItems.Find(i => i.Id == itemId);
            if (item != null)
            {
                Logger.Log($"Donation processed: {item.Name} ({item.Price}$)");
                GratitudeSystem.AddDonation(item.Price);
            }
            else
            {
                Logger.Error($"Donation item not found: {itemId}");
            }
        }

        public void RegisterItem(string id, string name, float price)
        {
            donationItems.Add(new DonationItem { Id = id, Name = name, Price = price });
        }
    }
}
