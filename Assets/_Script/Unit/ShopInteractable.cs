using UnityEngine;

public class ShopInteractable : MonoBehaviour, IInteractable
{
    private InventoryData shopInventory;

    void Start()
    {
        // Khởi tạo kho hàng của shop với một số item mẫu
        shopInventory = new InventoryData(5, 5);
    }
    public string GetInteractPrompt()
    {
        return "Hold [F] Open Shop";
    }
    public void Interact()
    {
        InventoryManager.Instance.OpenOtherInventory(shopInventory);
    }
    public bool CanInteract()
    {
        return true;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
