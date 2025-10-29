using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DonationSupportUI : MonoBehaviour
{
    [Header("Основные элементы UI")]
    public TMP_Text titleText;
    public TMP_Text gratitudeLevelText;
    public TMP_Text totalSupportText;
    public TMP_Text ncoinPreviewText;
    public TMP_InputField customAmountInput;
    public Button calculateButton;
    public Button confirmDonationButton;
    public Transform packageContainer;
    public GameObject packageButtonPrefab;

    [Header("Интерфейс прогресса")]
    public Slider gratitudeProgressSlider;
    public TMP_Text nextLevelText;
    public TMP_Text nextRewardText;

    [Header("История донатов")]
    public Transform historyContainer;
    public GameObject historyEntryPrefab;

    [Header("Данные")]
    public float conversionRateKZTPerNcoin = 40f;
    public float totalSpentKZT = 0f;
    public int gratitudeLevel = 0;

    private List<DonationPackage> donationPackages = new List<DonationPackage>();

    private void Start()
    {
        titleText.text = "💎 Покупки и благодарности";
        LoadPackages();
        RenderPackages();
        UpdateGratitudeUI();

        calculateButton.onClick.AddListener(CalculateCustomDonation);
        confirmDonationButton.onClick.AddListener(ConfirmDonation);
    }

    private void LoadPackages()
    {
        donationPackages = new List<DonationPackage>()
        {
            new DonationPackage("Мини-набор", 1000, 25),
            new DonationPackage("Малый набор", 5000, 150),
            new DonationPackage("Средний набор", 15000, 400),
            new DonationPackage("Большой набор", 45000, 1200),
            new DonationPackage("Премиум-набор", 100000, 3000)
        };
    }

    private void RenderPackages()
    {
        foreach (Transform child in packageContainer)
            Destroy(child.gameObject);

        foreach (DonationPackage pack in donationPackages)
        {
            GameObject button = Instantiate(packageButtonPrefab, packageContainer);
            button.GetComponentInChildren<TMP_Text>().text = $"{pack.name} — {pack.priceKZT:N0} ₸";
            button.GetComponent<Button>().onClick.AddListener(() => OnPackageSelected(pack));
        }
    }

    private void OnPackageSelected(DonationPackage package)
    {
        int ncoinAmount = package.ncoin;
        ncoinPreviewText.text = $"Вы получите: <color=#FFD700>{ncoinAmount} Ncoin</color>";
        totalSpentKZT += package.priceKZT;
        gratitudeProgressSlider.value = totalSpentKZT;
        UpdateGratitudeUI();
    }

    private void CalculateCustomDonation()
    {
        if (float.TryParse(customAmountInput.text, out float amount))
        {
            int ncoin = Mathf.FloorToInt(amount / conversionRateKZTPerNcoin);
            ncoinPreviewText.text = $"Вы получите: <color=#FFD700>{ncoin} Ncoin</color>";
        }
        else
        {
            ncoinPreviewText.text = "Введите корректную сумму";
        }
    }

    private void ConfirmDonation()
    {
        if (float.TryParse(customAmountInput.text, out float amount) && amount >= 500)
        {
            totalSpentKZT += amount;
            int ncoin = Mathf.FloorToInt(amount / conversionRateKZTPerNcoin);
            gratitudeProgressSlider.value = totalSpentKZT;
            gratitudeLevel = CalculateGratitudeLevel(totalSpentKZT);

            ncoinPreviewText.text = $"✅ Донат подтверждён! +{ncoin} Ncoin";
            UpdateGratitudeUI();
            LogDonation(amount, ncoin);
        }
        else
        {
            ncoinPreviewText.text = "Минимальная сумма — 500 ₸";
        }
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

    private void UpdateGratitudeUI()
    {
        gratitudeLevelText.text = $"Уровень благодарности: {gratitudeLevel}";
        totalSupportText.text = $"Всего поддержано: {totalSpentKZT:N0} ₸";

        switch (gratitudeLevel)
        {
            case 1: nextLevelText.text = "Следующий уровень — 5 000 ₸"; break;
            case 2: nextLevelText.text = "Следующий уровень — 15 000 ₸"; break;
            case 3: nextLevelText.text = "Следующий уровень — 45 000 ₸"; break;
            case 4: nextLevelText.text = "Следующий уровень — 100 000 ₸"; break;
            default: nextLevelText.text = "Вы достигли максимального уровня!"; break;
        }
    }

    private void LogDonation(float amount, int ncoin)
    {
        GameObject entry = Instantiate(historyEntryPrefab, historyContainer);
        entry.GetComponentInChildren<TMP_Text>().text =
            $"Получено: {ncoin} Ncoin — сумма: {amount:N0} ₸ — {System.DateTime.Now:dd.MM.yyyy HH:mm}";
    }

    [System.Serializable]
    public class DonationPackage
    {
        public string name;
        public int priceKZT;
        public int ncoin;

        public DonationPackage(string name, int priceKZT, int ncoin)
        {
            this.name = name;
            this.priceKZT = priceKZT;
            this.ncoin = ncoin;
        }
    }
}
