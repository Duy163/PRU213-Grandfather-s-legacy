using System;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public GameData currentGameData;

    [SerializeField] private ItemData fish;

    private string saveFilePath;
    private const string SAVE_FILE_NAME = "savegame.json";

    protected override void Awake()
    {
        base.Awake();
        InitializeDataManager();
    }

    private void InitializeDataManager()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        Debug.Log("Save file path: " + saveFilePath);

        // Load existing data or create new
        if (SaveFileExists())
        {
            LoadGame();
        }
        else
        {
            CreateNewGame();
        }
    }

    // ============ SAVE FUNCTIONS ============

    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(currentGameData, true);

            // Optional: Simple encryption (XOR)
            // string encryptedJson = EncryptDecrypt(json);

            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game saved successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }

    public void SaveGameAsync()
    {
        // For auto-save without freezing game
        System.Threading.Tasks.Task.Run(() => SaveGame());
    }

    // ============ LOAD FUNCTIONS ============

    public void LoadGame()
    {
        try
        {
            if (SaveFileExists())
            {
                string json = File.ReadAllText(saveFilePath);

                // Optional: Decrypt if encrypted
                // string decryptedJson = EncryptDecrypt(json);

                currentGameData = JsonUtility.FromJson<GameData>(json);
                Debug.Log("Game loaded successfully!");
            }
            else
            {
                Debug.LogWarning("Save file not found. Creating new game.");
                CreateNewGame();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load game: " + e.Message);
            CreateNewGame();
        }
    }

    // ============ NEW GAME ============

    public void CreateNewGame()
    {
        currentGameData = new GameData();

        debug(currentGameData);
        Debug.Log("New game created!");
    }

    void debug(GameData temp)
    {
        InventoryItemData fish = new InventoryItemData(this.fish, new Vector2Int(3, 4));

        temp.inventoryData.AddListItem(fish);
    }

    // ============ UTILITY FUNCTIONS ============

    public bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
    }

    public void DeleteSaveFile()
    {
        if (SaveFileExists())
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted!");
            CreateNewGame();
        }
    }

    // Simple XOR encryption (optional)
    private string EncryptDecrypt(string data)
    {
        string key = "YourSecretKey123"; // Change this!
        char[] output = new char[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            output[i] = (char)(data[i] ^ key[i % key.Length]);
        }

        return new string(output);
    }

    // ============ QUICK ACCESS FUNCTIONS ============

    // Ship
    public void UpdateShipPosition(Vector3 position, Quaternion rotation)
    {
        currentGameData.playerShipData.position = position;
        currentGameData.playerShipData.rotation = rotation;
    }

    public void UpdatePanicLevel(float level)
    {
        currentGameData.playerShipData.panicLevel = Mathf.Clamp(level, 0f, 100f);
    }

    // Inventory
    public void AddItemToInventory(InventoryItemData item)
    {
        currentGameData.inventoryData.items.Add(item);
    }

    public void RemoveItemFromInventory(string itemID)
    {
        // currentGameData.inventoryData.items.RemoveAll(x => x.itemID == itemID);
    }

    // Economy
    public bool SpendMoney(float amount)
    {
        if (currentGameData.economyData.currentMoney >= amount)
        {
            currentGameData.economyData.currentMoney -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(float amount)
    {
        currentGameData.economyData.currentMoney += amount;
    }

    // World
    public void DiscoverFishingSpot(FishingSpot spot)
    {
        spot.isDiscovered = true;
        if (!currentGameData.worldData.discoveredFishingSpots.Contains(spot))
        {
            currentGameData.worldData.discoveredFishingSpots.Add(spot);
        }
    }

    public void AdvanceTime(float hours)
    {
        currentGameData.worldData.gameTime += hours;

        // Check if new day
        if (currentGameData.worldData.gameTime >= 24f)
        {
            currentGameData.worldData.gameTime -= 24f;
            currentGameData.worldData.currentDay++;
        }

        // Update day/night
        currentGameData.worldData.isNight =
            currentGameData.worldData.gameTime >= 18f ||
            currentGameData.worldData.gameTime < 6f;
    }

    // Progression
    public void CompleteQuest(string questID)
    {
        currentGameData.progressionData.completedQuests.Add(questID);
        currentGameData.progressionData.activeQuests.RemoveAll(x => x.questID == questID);
    }

    public void UnlockArea(string areaID)
    {
        if (!currentGameData.progressionData.unlockedAreas.Contains(areaID))
        {
            currentGameData.progressionData.unlockedAreas.Add(areaID);
        }
    }
}
