using System;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    [SerializeField] private CurrencyView currencyView;
    private EconomyData economyData;

    void Start()
    {
        economyData = DataManager.Instance.currentGameData.economyData;
        currencyView.SetMoney(economyData.currentMoney);
    }
}
