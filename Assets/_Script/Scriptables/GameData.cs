

[System.Serializable]
public class GameData
{
    public PlayerShipData playerShipData;
    public InventoryData inventoryData;
    public ProgressionData progressionData;
    public WorldData worldData;
    public EconomyData economyData;


    public GameData()
    {
        playerShipData = new PlayerShipData();
        inventoryData = new InventoryData();
        progressionData = new ProgressionData();
        worldData = new WorldData();
        economyData = new EconomyData();
    }
}
