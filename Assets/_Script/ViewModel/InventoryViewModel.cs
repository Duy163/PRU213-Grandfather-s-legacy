using UnityEngine;

public class InventoryViewModel
{
    private InventoryModel model;
    private InventoryData data;
    private FishDatabase fishDatabase;

    private bool isBelongPlayer = false;

    public event System.Action<int, int> OnCellChanged;
    public event System.Action<ItemData, int, int, bool> OnAddItem;
    public event System.Action<ItemData, int, int> OnPlaceItem;
    public event System.Action<int, int> OnRemoveItem;
    public event System.Action<int, int> OnDestroyItem;

    public InventoryViewModel(InventoryData data, FishDatabase fishDatabase, bool isBelongPlayer)
    {
        this.data = data;
        model = new InventoryModel(new Vector2(data.column, data.row));
        this.fishDatabase = fishDatabase;

        this.isBelongPlayer = isBelongPlayer;
    }

    public void Dispose()
    {
        OnCellChanged = null;
        OnAddItem = null;
        OnPlaceItem = null;
    }

    public bool CanPlace(ItemData itemData, int startX, int startY)
    {
        var shape = itemData.itemShape;

        for (int y = 0; y < shape.height; y++)
        {
            for (int x = 0; x < shape.width; x++)
            {
                if (!shape.Occupies(x, y))
                    continue;

                int gx = startX + x;
                int gy = startY + y;

                if (!model.IsInBounds(gx, gy))
                    return false;

                if (IsOccupied(gx, gy))
                    return false;
            }
        }
        return true;
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(data.column, data.row);
    }

    public int GetItemCount(string id)
    {
        int count = 0;
        foreach (var item in data.items)
        {
            if (item.itemData.itemId == id)
                count++;
        }
        return count;
    }

    public void AddItem(ItemData itemData)
    {
        Vector2 place = FindPlaceForItem(itemData);
        if (place.Equals(Vector2.negativeInfinity))
        {
            Debug.Log($"No place found for item {itemData.itemName}");
            return;
        }


        PlaceItem(itemData, (int)place.x, (int)place.y);
        OnAddItem?.Invoke(itemData, (int)place.x, (int)place.y, isBelongPlayer);
    }

    public void PlaceItem(ItemData itemData, int x, int y)
    {
        var item = new InventoryItemData(itemData, new Vector2Int(x, y));
        data.items.Add(item);

        SetOccupiedSlot(itemData, x, y, true);

    }

    public void RemoveItem(ItemData itemData, int x, int y)
    {
        var item = data.items.Find(i => i.position == new Vector2(x, y));
        SetOccupiedSlot(itemData, item.position.x, item.position.y, false);
        data.items.Remove(item);
        OnRemoveItem?.Invoke(x, y);
    }

    public void AddItemFormList()
    {
        if (model.wasOpen) return;
        model.wasOpen = true;
        var listItem = data.items;
        foreach (InventoryItemData item in listItem)
        {
            ItemData data = fishDatabase.GetByID(item.itemID);
            SetOccupiedSlot(data, item.position.x, item.position.y, true);
            OnAddItem?.Invoke(data, item.position.x, item.position.y, isBelongPlayer);
        }
    }

    public void DestroyItem(int x, int y)
    {
        var item = data.items.Find(i => i.position == new Vector2(x, y));
        if (item == null) return;

        SetOccupiedSlot(item.itemData, item.position.x, item.position.y, false);
        data.items.Remove(item);
        OnDestroyItem?.Invoke(x, y);
    }

    public void DestroyItem(string itemID)
    {
        var item = data.items.Find(i => i.itemData.itemId == itemID);
        if (item == null) return;

        SetOccupiedSlot(item.itemData, item.position.x, item.position.y, false);
        data.items.Remove(item);
        OnDestroyItem?.Invoke(item.position.x, item.position.y);
    }

    public void DestroyListItem(string itemID, int amount)
    {
        if (GetItemCount(itemID) < amount)
        {
            Debug.Log($"[InventoryViewModel] Không đủ item {itemID} để destroy. Yêu cầu: {amount}, có: {GetItemCount(itemID)}");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            DestroyItem(itemID);
        }
    }

    public int SaleItem(InventoryItemData item)
    {
        int price = (int)item.itemData.value;
        DestroyItem(item.itemData.itemId);
        return price;
    }

    public int SaleAllItem()
    {
        int totalPrice = 0;
        foreach (var item in data.items)
        {
            totalPrice += (int)item.itemData.value;
        }

        data.items.Clear();

        return totalPrice;
    }
    //===========================PRIVATE========================================

    Vector2 FindPlaceForItem(ItemData itemData)
    {
        for (int y = 0; y <= data.row - itemData.itemShape.height; y++)
        {
            for (int x = 0; x <= data.column - itemData.itemShape.width; x++)
            {
                if (CanPlace(itemData, x, y))
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.negativeInfinity;
    }

    void SetOccupiedSlot(ItemData item, int startX, int startY, bool isPlace)
    {
        var shape = item.itemShape;

        for (int y = 0; y < shape.height; y++)
        {
            for (int x = 0; x < shape.width; x++)
            {
                if (!shape.Occupies(x, y))
                    continue;

                int gx = startX + x;
                int gy = startY + y;

                OnCellChanged?.Invoke(gx, gy);

                model.SetCell(gx, gy, isPlace);
            }
        }
    }

    bool IsOccupied(int x, int y)
    {
        return model.GetCell(x, y);
    }
}
