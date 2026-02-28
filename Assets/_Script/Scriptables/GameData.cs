

[System.Serializable]
public class GameData
{
    public PlayerShipData playerShipData = new PlayerShipData();
    public InventoryData inventoryData = new InventoryData();
    public EconomyData economyData = new EconomyData();
    public WorldData worldData = new WorldData();
    public ProgressionData progressionData = new ProgressionData();
    public WorldStateSaveData worldStateSaveData = new WorldStateSaveData();


    public GameData()
    {
        playerShipData = new PlayerShipData();
        inventoryData = new InventoryData();
        progressionData = new ProgressionData();
        worldData = new WorldData();
        economyData = new EconomyData();
        worldStateSaveData = new WorldStateSaveData();
    }
}
