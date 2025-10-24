namespace NikitonCore.Systems
{
    /// <summary>
    /// Единица доната (подписка, покупка, премиум и т.д.)
    /// </summary>
    [System.Serializable]
    public class DonationItem
    {
        public string Id;
        public string Name;
        public float Price;
    }
}
