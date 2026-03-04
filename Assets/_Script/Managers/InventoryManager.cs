using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Controllers")]
    [SerializeField] private PlayerInventoryController playerController;
    [SerializeField] private OtherInventoryController otherController;

    [SerializeField] private FishDatabase fishDatabase;

    private bool hasOtherInventory = false;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // DataManager.Instance.currentGameData.inventoryData.items.Clear(); // tạm test

        playerController.Initialize(DataManager.Instance.currentGameData.inventoryData, fishDatabase);
    }

    void OnEnable()
    {
        InputEvent.OnOpenInventoryPressed += OnOpenInventoryPressed;

        InputEvent.OnCloseInventoryPressed += OnCloseInventoryPressed;

        InventoryEvent.OnPickItem += HandlePickItem;
        InventoryEvent.OnDropItem += HandleDropItem;
        InventoryEvent.OnCanPlace += HandleCanPlace;
        InventoryEvent.OnRemoveItem += HandleRemoveItem;
        // InventoryEvent.OnInitOtherInventory += HandleInitOtherInventory;
        // InventoryEvent.OnDestroyOtherInventory += HandleDestroyOtherInventory;
    }

    void OnDisable()
    {
        InputEvent.OnOpenInventoryPressed -= OnOpenInventoryPressed;

        InputEvent.OnCloseInventoryPressed -= OnCloseInventoryPressed;

        InventoryEvent.OnPickItem -= HandlePickItem;
        InventoryEvent.OnDropItem -= HandleDropItem;
        InventoryEvent.OnCanPlace -= HandleCanPlace;
        InventoryEvent.OnRemoveItem -= HandleRemoveItem;
        // InventoryEvent.OnInitOtherInventory -= HandleInitOtherInventory;
        // InventoryEvent.OnDestroyOtherInventory -= HandleDestroyOtherInventory;
    }

    // ============= Input Handling =============

    public void OnOpenInventoryPressed()
    {
        playerController.Show();
        InputManager.Instance.EnableCargo();
    }

    public void OnCloseInventoryPressed()
    {
        playerController.Hide();

        if (hasOtherInventory)
        {
            otherController.Hide();
            otherController.SaleAllItem();
            otherController.Dispose();
            hasOtherInventory = false;
        }

        InputManager.Instance.EnableShip();
    }

    public void OpenOtherInventory(InventoryData inventory)
    {
        hasOtherInventory = true;
        otherController.Initialize(inventory, fishDatabase);
        otherController.Show();
        OnOpenInventoryPressed();
    }

    // ============= Public =============
    public int CountItemInPlayerInventory(string itemID)
    {
        return playerController.CountItem(itemID);
    }

    public void DestroyListItemInPlayerInventory(string itemID, int amount)
    {
        playerController.DestroyListItem(itemID, amount);
    }

    public void AddItemForPlayer(ItemData item)
    {
        playerController.AddItem(item);
        DataManager.Instance.Save();
    }

    // ============= Event Routing =============
    void HandlePickItem(ItemUI item)
    {
        GetController(item.isBelongPlayer).PickItem(item);
        SetAllRaycast(false);
        DataManager.Instance.Save();
    }

    void HandleDropItem(ItemUI item, DropSlot dropSlot)
    {
        GetController(dropSlot.isBelongPlayer).DropItem(item, dropSlot);
        SetAllRaycast(true);
        DataManager.Instance.Save();
    }

    bool HandleCanPlace(ItemUI item, DropSlot dropSlot)
    {
        return GetController(dropSlot.isBelongPlayer).CanPlace(item.itemData, dropSlot.x, dropSlot.y);
    }

    void HandleRemoveItem()
    {
        SetAllRaycast(true);
    }

    // void HandleInitOtherInventory(InventoryData inventory)
    // {
    //     otherController.Initialize(inventory);
    //     hasOtherInventory = true;
    // }

    void HandleDestroyOtherInventory()
    {
        otherController.Dispose();
        hasOtherInventory = false;
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
