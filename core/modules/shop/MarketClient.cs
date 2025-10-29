using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] public class MarketItem { public string id; public string seller; public string itemId; public int ncoin; public int qty; }

public static class MarketClient
{
    public static List<MarketItem> Listings = new List<MarketItem>();

    public static IEnumerator List(string itemId, int priceNcoin, int qty, string seller)
    {
        Listings.Add(new MarketItem{ id=System.Guid.NewGuid().ToString("N"), seller=seller, itemId=itemId, ncoin=priceNcoin, qty=qty });
        yield break;
    }

    public static bool Buy(string listingId, ref int buyerNCoin)
    {
        var it = Listings.Find(x=>x.id==listingId);
        if (it==null) return false;
        if (buyerNCoin < it.ncoin) return false;
        buyerNCoin -= it.ncoin;
        InventoryManager.AddByPack(it.itemId);
        Listings.Remove(it);
        return true;
    }
}
