using System;
using TMPro;
using UnityEngine;

public class CurrencyView : MonoBehaviour
{
    public void SetMoney(float money)
    {
        transform.GetComponent<TextMeshProUGUI>().text = "$" + money.ToString();
    }
}
