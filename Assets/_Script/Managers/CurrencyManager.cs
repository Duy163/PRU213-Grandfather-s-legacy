using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private CurrencyView currencyView;
    private EconomyData economyData;

    void Start()
    {
        economyData = DataManager.Instance.currentGameData.economyData;
        currencyView.SetMoney(economyData.currentMoney);
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("[CurrencyManager] Cannot add negative money.");
            return;
        }

        economyData.currentMoney += amount;
        currencyView.SetMoney(economyData.currentMoney);
        DataManager.Instance.Save();
    }

    public bool SpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("[CurrencyManager] Cannot spend negative money.");
            return false;
        }

        if (economyData.currentMoney < amount)
        {
            Debug.Log("[CurrencyManager] Không đủ tiền.");
            return false;
        }

        economyData.currentMoney -= amount;
        currencyView.SetMoney(economyData.currentMoney);
        DataManager.Instance.Save();
        return true;
    }

    public float GetCurrentMoney()
    {
        return economyData.currentMoney;
    }
}
