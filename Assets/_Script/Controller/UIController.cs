using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private Transform ItemDragCanvas;
    public bool isHoldingItem = false;
    public ItemUI itemHolding;
    private Vector2 offset;

    void Awake()
    {
        inputManager = InputManager.Instance;
    }
    void Update()
    {
        if (!isHoldingItem) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        itemHolding.GetComponent<RectTransform>().position = mousePos - offset;

        if (inputManager.RemoveItem())
        {
            RemoveHoldingItem();
        }
    }

    public void StartHoldingItem(ItemUI item)
    {
        itemHolding = item;
        itemHolding.SetRaycast(false);
        RectTransform rt = item.GetComponent<RectTransform>();
        offset = new Vector2(rt.rect.width / 8, -rt.rect.height / 8);
        rt.SetParent(ItemDragCanvas, false);

        InventoryEvent.OnPickItem?.Invoke(itemHolding);
        isHoldingItem = true;
    }

    public void StopHoldingItem(DropSlot dropSlot)
    {
        if (!InventoryEvent.OnCanPlace.Invoke(itemHolding, dropSlot))
        {
            Debug.Log("Cannot place here");
            return;
        }

        isHoldingItem = false;
        InventoryEvent.OnDropItem?.Invoke(itemHolding, dropSlot);
        itemHolding.SnapToSlot(dropSlot);
        itemHolding.SetRaycast(true);

        itemHolding = null;
    }

    public void RemoveHoldingItem()
    {
        InventoryEvent.OnRemoveItem?.Invoke();
        isHoldingItem = false;
        itemHolding.Remove();
        itemHolding = null;
    }
}
