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
        Save();
    }

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
                currentGameData.loginCount++;
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

        Save();
        Debug.Log("[DataManager] New game created.");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);

        CreateNewGame();
        Debug.Log("[DataManager] Save deleted.");
    }
}
