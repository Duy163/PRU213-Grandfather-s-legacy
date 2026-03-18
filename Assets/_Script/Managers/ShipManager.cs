using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] CurrencyManager currencyManager;
    [SerializeField] UpgradeView upgradeView;
    [SerializeField] ShipController shipController;
    public PlayerShipData playerShipData;

    private int maxSpeedMaxLv = 5;
    private float maxSpeedPerLv = 20f;
    private int maxSpeedPrice = 1000;

    private int accelerationMaxLv = 5;
    private float accelerationPerLv = 10f;
    private int accelerationPrice = 1000;

    private int turnStrengthMaxLv = 5;
    private float turnStrengthPerLv = 0.5f;
    private int turnStrengthPrice = 1000;

    private int lampPrice = 500;


    void Start()
    {
        playerShipData = DataManager.Instance.currentGameData.playerShipData;

        shipController.LoadShipData();

        upgradeView.SetMaxSpeedBar(playerShipData.maxSpeedLv / maxSpeedMaxLv);
        upgradeView.SetAccelerationBar((float)playerShipData.accelerationLv / (float)accelerationMaxLv);
        upgradeView.SetTurnStrengthBar((float)playerShipData.turnStrengthLv / (float)turnStrengthMaxLv);
        upgradeView.SetLampBar(playerShipData.hasLamp == true ? 1 : 0);

        upgradeView.SetMaxSpeedPrice(maxSpeedPrice);
        upgradeView.SetAccelerationPrice(accelerationPrice);
        upgradeView.SetTurnStrengthPrice(turnStrengthPrice);
        upgradeView.SetLampPrice(lampPrice);
    }

    public void OnOpenUpgrade()
    {
        InputManager.Instance.EnableEnding();
        upgradeView.Show();
    }

    public void OnCloseUpgrade()
    {
        InputManager.Instance.EnableShip();
        upgradeView.Hide();
    }

    public bool UpgradeMaxSpeed()
    {
        if (playerShipData.maxSpeedLv == maxSpeedMaxLv) return false;

        bool result = currencyManager.SpendMoney(maxSpeedPrice);
        if (!result) return false;

        playerShipData.maxSpeedLv++;
        playerShipData.maxSpeed += maxSpeedPerLv;

        upgradeView.SetMaxSpeedBar((float)playerShipData.maxSpeedLv / (float)maxSpeedMaxLv);
        ToSaveData();
        return true;
    }

    public bool UpgradeAcceleration()
    {
        if (playerShipData.accelerationLv == accelerationMaxLv) return false;
        bool result = currencyManager.SpendMoney(accelerationPrice);
        if (!result) return false;

        playerShipData.accelerationLv++;
        playerShipData.acceleration += accelerationPerLv;

        upgradeView.SetAccelerationBar((float)playerShipData.accelerationLv / (float)accelerationMaxLv);
        ToSaveData();
        return true;
    }

    public bool UpgradeTurnStrength()
    {
        if (playerShipData.turnStrengthLv == turnStrengthMaxLv) return false;

        bool result = currencyManager.SpendMoney(turnStrengthPrice);
        if (!result) return false;

        playerShipData.turnStrengthLv++;
        playerShipData.turnStrength += turnStrengthPerLv;

        upgradeView.SetTurnStrengthBar((float)playerShipData.turnStrengthLv / (float)turnStrengthMaxLv);
        ToSaveData();
        return true;
    }

    public bool UpgardeLamp()
    {
        if (playerShipData.hasLamp) return false;

        bool result = currencyManager.SpendMoney(lampPrice);
        if (!result) return false;

        playerShipData.hasLamp = true;

        upgradeView.SetLampBar(1);
        ToSaveData();
        return true;
    }

    void ToSaveData()
    {
        DataManager.Instance.currentGameData.playerShipData = playerShipData;
        DataManager.Instance.Save();
    }
}
