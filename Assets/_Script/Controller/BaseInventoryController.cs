using UnityEngine;

public abstract class BaseInventoryController : MonoBehaviour
{
    [SerializeField] protected RectTransform inventoryFrame;
    [SerializeField] protected InventoryView inventoryView;

    protected InventoryViewModel viewModel;

    public virtual void Initialize(InventoryData data)
    {
        viewModel = new InventoryViewModel(data, IsPlayerInventory());
        inventoryView.Bind(inventoryFrame, viewModel, IsPlayerInventory());
    }

    public virtual void Show()
    {
        viewModel.AddItemFormList();
    }

    public virtual void Hide()
    {
        // Hide logic
    }

    public void PickItem(ItemUI item)
    {
        viewModel.RemoveItem(item.itemData, item.originalX, item.originalY);
    }

    public void DropItem(ItemUI item, DropSlot dropSlot)
    {
        viewModel.PlaceItem(item.itemData, dropSlot.x, dropSlot.y);
        inventoryView.SetParent(item.GetComponent<RectTransform>());
        item.isBelongPlayer = IsPlayerInventory();
    }

    public bool CanPlace(ItemData itemData, int x, int y)
    {
        return viewModel.CanPlace(itemData, x, y);
    }

    public void SetRaycast(bool value)
    {
        inventoryView.SetRaycast(value);
    }

    public void AddItem(ItemData item)
    {
        viewModel.AddItem(item);
    }

    protected abstract bool IsPlayerInventory();

    public virtual void Dispose()
    {
        inventoryView.UnBind();
        viewModel?.Dispose();
    }
}
