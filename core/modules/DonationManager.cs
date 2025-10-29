using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DonationManager : MonoBehaviour
{
    public static DonationManager Instance;
    private const string CloudPath = "core/cloud/profile/player_donations.json";
    private const string ProfilePath = "core/cloud/profile/player_profile.json";

    [System.Serializable]
    public class DonationTransaction
    {
        public string id;
        public float amountKZT;
        public int ncoinReceived;
        public System.DateTime date;
    }

    public List<DonationTransaction> transactions = new List<DonationTransaction>();
    public float totalSpentKZT = 0;
    public int gratitudeLevel = 0;
    public int totalNcoin = 0;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            await LoadData();
        }
        else Destroy(gameObject);
    }

    private async Task LoadData()
    {
        var donationsData = await NikitonCloudManager.Instance.LoadAsync(CloudPath);
        if (donationsData != null)
        {
            totalSpentKZT = donationsData["player_donations"]["total_spent_kzt"].AsFloat;
            gratitudeLevel = donationsData["player_donations"]["gratitude_level"].AsInt;
        }
    }

    public async void RegisterDonation(float amountKZT, int ncoinReceived)
    {
        DonationTransaction newTx = new DonationTransaction
        {
            id = System.Guid.NewGuid().ToString(),
            amountKZT = amountKZT,
            ncoinReceived = ncoinReceived,
            date = System.DateTime.Now
        };

        transactions.Add(newTx);
        totalSpentKZT += amountKZT;
        totalNcoin += ncoinReceived;
        gratitudeLevel = CalculateGratitudeLevel(totalSpentKZT);

        PlayerProfile.Instance.AddNcoin(ncoinReceived);
        await UpdateCloudData();
        await GrantGratitudeRewards();
    }

    private async Task UpdateCloudData()
    {
        var donationsJson = new Dictionary<string, object>
        {
            { "total_spent_kzt", totalSpentKZT },
            { "gratitude_level", gratitudeLevel },
            { "last_donation_date", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") }
        };

        await NikitonCloudManager.Instance.SaveAsync(CloudPath, donationsJson);
        await NikitonCloudManager.Instance.SyncNow();
    }

    private int CalculateGratitudeLevel(float spent)
    {
        if (spent >= 100000) return 5;
        if (spent >= 45000) return 4;
        if (spent >= 15000) return 3;
        if (spent >= 5000) return 2;
        if (spent >= 1000) return 1;
        return 0;
    }

    private async Task GrantGratitudeRewards()
    {
        string mailTitle = $"🎁 Уровень благодарности {gratitudeLevel} достигнут!";
        string mailBody = "Поздравляем! Вы получили бонусы за поддержку проекта.";

        switch (gratitudeLevel)
        {
            case 1:
                await PlayerMailbox.Instance.SendSystemMail(mailTitle, mailBody, gold: 250, ncoin: 25);
                DonationCongratsUI.Show(1, "Поздравляем! Уровень благодарности 1 🎉", "Спасибо за поддержку Nikiton Games!", 250, 25);
                break;

            case 2:
                await PlayerMailbox.Instance.SendSystemMail(mailTitle, mailBody, gold: 1500, ncoin: 150, item: "vip_badge_bronze");
                PlayerProfile.Instance.UnlockVIP(1);
                DonationCongratsUI.Show(2, "VIP Bronze активирован 🌟", "Открыт бронзовый VIP и начислены награды.", 1500, 150);
                break;

            case 3:
                await PlayerMailbox.Instance.SendSystemMail(mailTitle, mailBody, gold: 5000, ncoin: 400, item: "vip_badge_silver");
                PlayerProfile.Instance.UnlockVIP(2);
                DonationCongratsUI.Show(3, "Новый уровень благодарности 💎", "Доступ к эксклюзивным событиям усилен.", 5000, 400);
                break;

            case 4:
                await PlayerMailbox.Instance.SendSystemMail(mailTitle, mailBody, gold: 15000, ncoin: 1200, item: "vip_badge_gold");
                PlayerProfile.Instance.UnlockVIP(3);
                DonationCongratsUI.Show(4, "VIP Gold 👑", "Золотой статус и уникальные привилегии!", 15000, 1200);
                break;

            case 5:
                await PlayerMailbox.Instance.SendSystemMail(mailTitle, mailBody, gold: 30000, ncoin: 3000, item: "founder_badge");
                PlayerProfile.Instance.UnlockVIP(4);
                DonationCongratsUI.Show(5, "Партнёр Nikiton Games 🏅", "Наивысший уровень благодарности. Спасибо!", 30000, 3000);
                break;
        }

        await NikitonCloudManager.Instance.SyncNow();
    }
}
