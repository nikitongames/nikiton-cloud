void Start()
{
    // Проверяем устройство при каждом запуске
    gameObject.AddComponent<DeviceVerifier>();

    InitializeCore();
}
