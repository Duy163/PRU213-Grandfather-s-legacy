using System;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public GameData currentGameData { get; private set; }
    public WorldStateManager WorldState { get; private set; }

    private string savePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    // Shortcut cho các manager khác dùng
    public ProgressionData Progression => currentGameData.progressionData;

    protected override void Awake()
    {
        base.Awake();
        Load();
    }

    // ══════════════════════════════════════════════════
    //  SAVE / LOAD / DELETE
    // ══════════════════════════════════════════════════

    public void Save()
    {
        try
        {
            // Ghi WorldState vào GameData trước khi serialize
            currentGameData.worldStateSaveData = WorldState.ToSaveData();

            string json = JsonUtility.ToJson(currentGameData, prettyPrint: true);
            File.WriteAllText(savePath, json);
            Debug.Log("[DataManager] Saved. \n" + savePath);
        }
        catch (Exception e)
        {
            Debug.LogError("[DataManager] Save failed: " + e.Message);
        }
    }

    public void Load()
    {
        WorldState = new WorldStateManager();

        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                currentGameData = JsonUtility.FromJson<GameData>(json);
                WorldState.FromSaveData(currentGameData.worldStateSaveData);
                Debug.Log("[DataManager] Loaded existing save.");
            }
            else
            {
                CreateNewGame();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("[DataManager] Load failed: " + e.Message);
            CreateNewGame();
        }
    }

    public void CreateNewGame()
    {
        currentGameData = new GameData();
        WorldState = new WorldStateManager();
        Debug.Log("[DataManager] New game created.");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);

        CreateNewGame();
        Debug.Log("[DataManager] Save deleted.");
    }

    // ══════════════════════════════════════════════════
    //  SHIP
    // ══════════════════════════════════════════════════

    public void UpdateShipPosition(Vector3 position, Quaternion rotation)
    {
        currentGameData.playerShipData.position = position;
        currentGameData.playerShipData.rotation = rotation;
    }

    public void UpdatePanicLevel(float level)
    {
        currentGameData.playerShipData.panicLevel = Mathf.Clamp(level, 0f, 100f);
    }

    // ══════════════════════════════════════════════════
    //  INVENTORY
    // ══════════════════════════════════════════════════

    public void AddItemToInventory(InventoryItemData item)
    {
        currentGameData.inventoryData.items.Add(item);
    }

    // public void RemoveItemFromInventory(string itemID)
    // {
    //     currentGameData.inventoryData.items.RemoveAll(x => x.itemID == itemID);
    // }

    // ══════════════════════════════════════════════════
    //  ECONOMY
    // ══════════════════════════════════════════════════

    public bool SpendMoney(float amount)
    {
        if (currentGameData.economyData.currentMoney >= amount)
        {
            currentGameData.economyData.currentMoney -= amount;
            return true;
        }

        Debug.Log("[DataManager] Không đủ tiền.");
        return false;
    }

    public void AddMoney(float amount)
    {
        currentGameData.economyData.currentMoney += amount;
    }

    // ══════════════════════════════════════════════════
    //  WORLD
    // ══════════════════════════════════════════════════

    public void AdvanceTime(float hours)
    {
        currentGameData.worldData.gameTime += hours;

        if (currentGameData.worldData.gameTime >= 24f)
        {
            currentGameData.worldData.gameTime -= 24f;
            currentGameData.worldData.currentDay++;
        }

        currentGameData.worldData.isNight =
            currentGameData.worldData.gameTime >= 18f ||
            currentGameData.worldData.gameTime < 6f;
    }

    public void DiscoverFishingSpot(FishingSpot spot)
    {
        spot.isDiscovered = true;
        if (!currentGameData.worldData.discoveredFishingSpots.Contains(spot))
            currentGameData.worldData.discoveredFishingSpots.Add(spot);
    }

    public void UnlockArea(string areaID)
    {
        if (!Progression.unlockedAreas.Contains(areaID))
        {
            Progression.unlockedAreas.Add(areaID);
        }
    }

    // ══════════════════════════════════════════════════
    //  PROGRESSION
    // ══════════════════════════════════════════════════

    public void DiscoverSpecies(string speciesID)
    {
        if (!Progression.discoveredSpecies.Contains(speciesID))
        {
            Progression.discoveredSpecies.Add(speciesID);
            WorldState.Increment("species_discovered");
        }
    }
}
