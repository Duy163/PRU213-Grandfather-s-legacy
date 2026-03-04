using UnityEngine;

[CreateAssetMenu(
    fileName = "NewItemData",
    menuName = "Inventory/Item Data",
    order = 1
)]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemId;          // ID duy nhất (dùng cho save/load)
    public string itemName;
    [TextArea]
    public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Grid Shape")]
    public ItemShape itemShape;

    [Header("Stats")]
    public float weight;
    public float value;
    public float MAX_WEIGHT;
    public float MAX_VALUE;

    [Header("Type")]
    public FishType fishType;
    public TimeType timeType;
    public ItemType itemType;
    public MinigameType minigameType;
}

public enum FishType
{
    Coastal,
    Shallow,
    Oceanic,
    Mangrove,
    Depth
}

public enum TimeType
{
    Day,
    Night,
    Any
}

public enum ItemType
{
    Consumable,
    Equipment,
    Quest,
    Material
}

public enum MinigameType
{
    TimingBar,
    none
}