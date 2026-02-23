using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int row;
    public int column;
    public int[,] cells;
    public List<InventoryItemData> items = new();

    public InventoryData()
    {
        row = 6;
        column = 7;
        cells = new int[column, row];
    }
    public InventoryData(int row, int column)
    {
        this.row = row;
        this.column = column;
        cells = new int[column, row];
    }

    public void AddListItem(InventoryItemData item)
    {
        items.Add(item);
    }
}

[System.Serializable]
public class InventoryItemData
{
    public ItemData itemData;
    public Vector2Int position;

    public InventoryItemData(ItemData item, Vector2Int pos)
    {
        itemData = item;
        position = pos;
    }
}
