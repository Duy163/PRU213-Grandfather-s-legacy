using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public float gameTime; // Tính bằng giờ trong game
    public int currentDay;
    public bool isNight;

    public List<FishingSpot> discoveredFishingSpots;

    public WorldData()
    {
        gameTime = 6f; // Bắt đầu lúc 6h sáng
        currentDay = 1;
        isNight = false;
        discoveredFishingSpots = new List<FishingSpot>();
    }
}

[System.Serializable]
public class FishingSpot
{
    public string spotID;
    public Vector3 position;
    public FishingSpotType spotType;
    public List<string> availableFish;
    public bool isDiscovered;
    public bool isDepleted;

    public FishingSpot(string id, Vector3 pos, FishingSpotType type)
    {
        spotID = id;
        position = pos;
        spotType = type;
        availableFish = new List<string>();
        isDiscovered = false;
        isDepleted = false;
    }
}

[System.Serializable]
public enum FishingSpotType
{
    Shallow,
    Coastal,
    Oceanic,
    Abyssal,
    Mangrove,
}

[System.Serializable]
public class POIData
{
    public string poiID;
    public string poiName;
    public Vector3 position;
    public POIType type;
    public bool isDiscovered;
    public bool isCompleted;

    public POIData(string id, string name, Vector3 pos, POIType poiType)
    {
        poiID = id;
        poiName = name;
        position = pos;
        type = poiType;
        isDiscovered = false;
        isCompleted = false;
    }
}

[System.Serializable]
public enum POIType
{
    Wreck,
    Relic,
    Shrine,
    Dock,
    Lighthouse,
    Cave
}