using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : BasePanel
{
    [SerializeField] ShipManager shipManager;

    [Header("Level")]
    [SerializeField] Image MaxSpeedLevel;
    [SerializeField] Image AccelerationLevel;
    [SerializeField] Image TurnStrengthLevel;
    [SerializeField] Image LampLevel;

    [Header("Button")]
    [SerializeField] GameObject MaxSpeedUPButton;
    [SerializeField] GameObject AccelerationUPButton;
    [SerializeField] GameObject TurnStrengthUPButton;
    [SerializeField] GameObject LampUPButton;

    [SerializeField] TextMeshProUGUI MaxSpeedButtonPrice;
    [SerializeField] TextMeshProUGUI AccelerationButtonPrice;
    [SerializeField] TextMeshProUGUI TurnStrengthButtonPrice;
    [SerializeField] TextMeshProUGUI LampButtonPrice;

    public void UpgradePressed(string name)
    {
        switch (name)
        {
            case "maxSpeed":
                shipManager.UpgradeMaxSpeed();

                break;
            case "acceleration":
                shipManager.UpgradeAcceleration();

                break;
            case "turnStrength":
                shipManager.UpgradeTurnStrength();

                break;
            case "lamp":
                shipManager.UpgardeLamp();

                break;
        }
    }

    void HideButton(string name)
    {
        switch (name)
        {
            case "maxSpeed":
                break;
            case "acceleration":
                break;
            case "turnStrength":
                break;
            case "lamp":
                break;
        }
    }

    public void SetMaxSpeedBar(float value)
    {
        MaxSpeedLevel.fillAmount = value;
        if (MaxSpeedLevel.fillAmount == 1) MaxSpeedUPButton.SetActive(false);
    }

    public void SetAccelerationBar(float value)
    {
        AccelerationLevel.fillAmount = value;
        if (AccelerationLevel.fillAmount == 1) AccelerationUPButton.SetActive(false);
    }

    public void SetTurnStrengthBar(float value)
    {
        TurnStrengthLevel.fillAmount = value;
        if (TurnStrengthLevel.fillAmount == 1) TurnStrengthUPButton.SetActive(false);
    }

    public void SetLampBar(float value)
    {
        LampLevel.fillAmount = value;
        if (LampLevel.fillAmount == 1) LampUPButton.SetActive(false);
    }


    public void SetMaxSpeedPrice(int value)
    {
        MaxSpeedButtonPrice.text = "$" + value;
    }

    public void SetAccelerationPrice(int value)
    {
        AccelerationButtonPrice.text = "$" + value;
    }

    public void SetTurnStrengthPrice(int value)
    {
        TurnStrengthButtonPrice.text = "$" + value;
    }

    public void SetLampPrice(int value)
    {
        LampButtonPrice.text = "$" + value;
    }
}
