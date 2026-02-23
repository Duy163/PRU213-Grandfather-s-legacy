using System.Collections.Generic;

[System.Serializable]
public class EconomyData
{
    public float currentMoney;
    public Dictionary<string, float> shopPrices; // ItemID -> Price
    public List<TransactionHistory> transactions;
    public Dictionary<string, List<string>> shopInventories; // ShopID -> List of ItemIDs

    public EconomyData()
    {
        currentMoney = 100f;
        shopPrices = new Dictionary<string, float>();
        transactions = new List<TransactionHistory>();
        shopInventories = new Dictionary<string, List<string>>();
    }
}

[System.Serializable]
public class TransactionHistory
{
    public string itemID;
    public float price;
    public bool isSell; // true = bán, false = mua
    public string shopID;
    public int day;

    public TransactionHistory(string item, float p, bool sell, string shop, int gameDay)
    {
        itemID = item;
        price = p;
        isSell = sell;
        shopID = shop;
        day = gameDay;
    }
}
