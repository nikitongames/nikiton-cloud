using UnityEngine;

public static class DurabilitySystem
{
    // Урон за "перегруз" (рыба тяжелее прочности комплекта)
    public static void ApplyFightDamage(float fishKg, float gearMaxKg)
    {
        float overload = Mathf.Max(0f, fishKg - gearMaxKg); // кг сверх прочности
        int dmg = Mathf.CeilToInt(overload * 10f);          // 1 кг перегруза ~ 10 ед. износа
        if (dmg <= 0) dmg = 1;

        DamageItem(GameInventory.EquippedRod, dmg);
        DamageItem(GameInventory.EquippedReel, dmg);
        DamageItem(GameInventory.EquippedLine, dmg * 2); // леска слабее — быстрее изнашивается
    }

    static void DamageItem(string itemId, int dmg)
    {
        int cur = GameInventory.Durability(itemId);
        GameInventory.SetDurability(itemId, cur - dmg);
    }

    public static bool IsBroken(string itemId) => GameInventory.Durability(itemId) <= 0;
}
