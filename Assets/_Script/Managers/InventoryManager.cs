using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private PlayerInventoryController playerController;
    [SerializeField] private OtherInventoryController otherController;

    private bool hasOtherInventory = false;

    void Start()
    {
        playerController.Initialize(DataManager.Instance.currentGameData.inventoryData);
    }

    void OnEnable()
    {
        InputEvent.OnOpenInventoryPressed += OnOpenInventoryPressed;
        InputEvent.OnCloseInventoryPressed += OnCloseInventoryPressed;

        InventoryEvent.OnPickItem += HandlePickItem;
        InventoryEvent.OnDropItem += HandleDropItem;
        InventoryEvent.OnCanPlace += HandleCanPlace;
        InventoryEvent.OnRemoveItem += HandleRemoveItem;
        InventoryEvent.OnInitOtherInventory += HandleInitOtherInventory;
        InventoryEvent.OnDestroyOtherInventory += HandleDestroyOtherInventory;
        InventoryEvent.OnAddItem += HandleAddItem;
    }

    void OnDisable()
    {
        InputEvent.OnOpenInventoryPressed -= OnOpenInventoryPressed;
        InputEvent.OnCloseInventoryPressed -= OnCloseInventoryPressed;

        InventoryEvent.OnPickItem -= HandlePickItem;
        InventoryEvent.OnDropItem -= HandleDropItem;
        InventoryEvent.OnCanPlace -= HandleCanPlace;
        InventoryEvent.OnRemoveItem -= HandleRemoveItem;
        InventoryEvent.OnInitOtherInventory -= HandleInitOtherInventory;
        InventoryEvent.OnDestroyOtherInventory -= HandleDestroyOtherInventory;
        InventoryEvent.OnAddItem -= HandleAddItem;
    }

    // ============= Input Handling =============

    void OnOpenInventoryPressed()
    {
        playerController.Show();

        if (hasOtherInventory)
        {
            otherController.Show();
        }

        InputManager.Instance.EnableUIInput(true);
    }

    void OnCloseInventoryPressed()
    {
        InputManager.Instance.EnableUIInput(false);
    }

    void OnInteractPressed()
    {

    }

    // ============= Event Routing =============
    void HandlePickItem(ItemUI item)
    {
        GetController(item.isBelongPlayer).PickItem(item);
        SetAllRaycast(false);
    }

    void HandleDropItem(ItemUI item, DropSlot dropSlot)
    {
        GetController(dropSlot.isBelongPlayer).DropItem(item, dropSlot);
        SetAllRaycast(true);
    }

    bool HandleCanPlace(ItemUI item, DropSlot dropSlot)
    {
        return GetController(dropSlot.isBelongPlayer).CanPlace(item.itemData, dropSlot.x, dropSlot.y);
    }

    void HandleRemoveItem()
    {
        SetAllRaycast(true);
    }

    void HandleInitOtherInventory(InventoryData inventory)
    {
        otherController.Initialize(inventory);
        hasOtherInventory = true;
    }

    void HandleDestroyOtherInventory()
    {
        otherController.Dispose();
        hasOtherInventory = false;
    }

    void HandleAddItem(ItemData item)
    {
        playerController.AddItem(item);
    }

    // ============= Helpers =============
    BaseInventoryController GetController(bool isPlayer)
    {
        return isPlayer ? playerController : otherController;
    }

    void SetAllRaycast(bool value)
    {
        playerController.SetRaycast(value);
        if (hasOtherInventory)
        {
            otherController.SetRaycast(value);
        }
    }
}
