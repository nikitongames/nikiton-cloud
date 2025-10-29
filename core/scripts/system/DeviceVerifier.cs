using UnityEngine;
using System;

public class DeviceVerifier : MonoBehaviour
{
    const string DeviceKey = "device_id";
    const string AgreementKey = "user_has_agreed";

    void Awake()
    {
        VerifyDevice();
    }

    void VerifyDevice()
    {
        // Уникальный ID устройства (Unity автоматически создаёт GUID)
        string currentDeviceId = SystemInfo.deviceUniqueIdentifier;

        // Если нет сохранённого ID — создаём и сохраняем
        if (!PlayerPrefs.HasKey(DeviceKey))
        {
            PlayerPrefs.SetString(DeviceKey, currentDeviceId);
            PlayerPrefs.Save();
            Debug.Log("[DeviceVerifier] Первый запуск, сохраняем ID устройства: " + currentDeviceId);
            return;
        }

        // Получаем старый ID
        string savedDeviceId = PlayerPrefs.GetString(DeviceKey);

        // Если устройство новое — сбрасываем согласие
        if (savedDeviceId != currentDeviceId)
        {
            Debug.Log("[DeviceVerifier] Обнаружено новое устройство! Сбрасываем соглашение.");
            PlayerPrefs.SetInt(AgreementKey, 0);
            PlayerPrefs.SetString(DeviceKey, currentDeviceId);
            PlayerPrefs.Save();
        }
    }
}
