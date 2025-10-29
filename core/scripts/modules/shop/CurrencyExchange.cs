using UnityEngine;
using System.IO;

[System.Serializable] public class ExCfg{ public int ncoin_to_gold_rate; public int min_exchange_ncoin; public int max_exchange_ncoin; }

public static class CurrencyExchange
{
    static ExCfg cfg;
    static void Ensure(){ if(cfg==null) cfg = JsonUtility.FromJson<ExCfg>(File.ReadAllText("core/configs/shop/exchange.json")); }

    public static bool ExchangeNcoinToGold(ref int ncoin, ref int gold, int amountNcoin)
    {
        Ensure();
        if (amountNcoin < cfg.min_exchange_ncoin || amountNcoin > cfg.max_exchange_ncoin) return false;
        if (ncoin < amountNcoin) return false;
        ncoin -= amountNcoin;
        gold  += amountNcoin * cfg.ncoin_to_gold_rate;
        return true;
    }
}
