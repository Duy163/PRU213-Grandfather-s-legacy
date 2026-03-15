

[System.Serializable]
public class GameData
{
    public int loginCount = 0;
    public PlayerShipData playerShipData = new PlayerShipData();
    public InventoryData inventoryData = new InventoryData();
    public EconomyData economyData = new EconomyData();
    public WorldData worldData = new WorldData();
    public ProgressionData progressionData = new ProgressionData();
    public WorldStateSaveData worldStateSaveData = new WorldStateSaveData();


    public GameData()
    {
        loginCount = 0;
        playerShipData = new PlayerShipData();
        inventoryData = new InventoryData();
        progressionData = new ProgressionData();
        worldData = new WorldData();
        economyData = new EconomyData();
        worldStateSaveData = new WorldStateSaveData();
    }
}
