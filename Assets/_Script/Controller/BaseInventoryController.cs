using UnityEngine;

public abstract class BaseInventoryController : MonoBehaviour
{
    [SerializeField] protected RectTransform inventoryFrame;
    [SerializeField] protected InventoryView inventoryView;

    protected InventoryViewModel viewModel;

    public virtual void Initialize(InventoryData data, FishDatabase fishDatabase)
    {
        viewModel = new InventoryViewModel(data, fishDatabase, IsPlayerInventory());

        inventoryView.Bind(inventoryFrame, viewModel, IsPlayerInventory());
    }

    public virtual void Show()
    {
        inventoryView.Show();
        viewModel.AddItemFormList();
    }

    public virtual void Hide()
    {
        inventoryView.Hide();
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

    public int CountItem(string itemID)
    {
        return viewModel.GetItemCount(itemID);
    }

    public void DestroyListItem(string itemID, int amount)
    {
        viewModel.DestroyListItem(itemID, amount);
    }

    public void SaleItem(ItemData item)
    {
        int price = (int)item.value;
        viewModel.DestroyListItem(item.itemId, 1);
        CurrencyManager.Instance.AddMoney(price);
    }

    public void SaleAllItem()
    {
        int totalPrice = viewModel.SaleAllItem();
        CurrencyManager.Instance.AddMoney(totalPrice);
    }

    protected abstract bool IsPlayerInventory();

    public virtual void Dispose()
    {
        inventoryView.UnBind();
        viewModel?.Dispose();
    }
}
