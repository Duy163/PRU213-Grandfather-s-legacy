using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : BasePanel
{
    [Header("References")]
    private InventoryViewModel vm;
    [SerializeField] private UIController uiController;

    [Header("Frame")]
    private RectTransform slotFrame;
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform itemFrame;

    private Dictionary<Vector2, ItemUI> listItem;

    private List<DropSlot> listSlot = new List<DropSlot>();

    [Header("Prefab")]
    [SerializeField] private GameObject slotFramePrefab;
    [SerializeField] private GameObject itemFramePrefab;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject blockerPrefab;
    [SerializeField] private GameObject itemUIPrefab;

    public void Bind(Transform parent, InventoryViewModel vm, bool isPlayer)
    {
        this.vm = vm;
        vm.OnCellChanged += OnCellChange;
        vm.OnAddItem += OnAddItem;
        vm.OnRemoveItem += OnRemoveItem;
        vm.OnDestroyItem += OnDestroyItem;

        Refresh(parent, isPlayer);
    }

    public void UnBind()
    {
        vm.OnCellChanged -= OnCellChange;
        vm.OnAddItem -= OnAddItem;
        vm.OnRemoveItem -= OnRemoveItem;
        vm.OnDestroyItem -= OnDestroyItem;
        vm = null;

        Clear();
    }

    void Refresh(Transform parent, bool isPlayer)
    {
        listItem = new Dictionary<Vector2, ItemUI>();
        slotFrame = Instantiate(slotFramePrefab, parent).GetComponent<RectTransform>();
        gridLayoutGroup = slotFrame.GetComponent<GridLayoutGroup>();
        itemFrame = Instantiate(itemFramePrefab, parent).GetComponent<RectTransform>();
        Vector2 gridSize = vm.GetGridSize();
        ResizeFrame(gridSize);
        InitSlots(gridSize, isPlayer);
    }

    void Clear()
    {
        // Destroy slot frame
        if (slotFrame != null)
        {
            Destroy(slotFrame.gameObject);
            slotFrame = null;
        }

        // Destroy item frame
        if (itemFrame != null)
        {
            Destroy(itemFrame.gameObject);
            itemFrame = null;
        }

        gridLayoutGroup = null;
    }

    void ResizeFrame(Vector2 gridSize)
    {
        Vector2 cellSize = gridLayoutGroup.cellSize;

        float width = gridSize.x * cellSize.x + (gridSize.x - 1);
        float height = gridSize.y * cellSize.y + (gridSize.y - 1);

        slotFrame.sizeDelta = new Vector2(width, height);
        itemFrame.sizeDelta = new Vector2(width, height);
    }

    void InitSlots(Vector2 gridSize, bool isBelongPlayer)
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject slot = Instantiate(itemSlotPrefab, slotFrame);
                slot.name = "ItemSlot_" + x + "_" + y;
                slot.GetComponent<DropSlot>().Init(isBelongPlayer, x, y);
                slot.GetComponent<DropSlot>().uiController = uiController;

                listSlot.Add(slot.GetComponent<DropSlot>());
            }
        }
    }

    public DropSlot GetSlot(int x, int y)
    {
        for (int i = 0; i < slotFrame.childCount; i++)
        {
            DropSlot slot = slotFrame.GetChild(i).GetComponent<DropSlot>();
            if (slot.x == x && slot.y == y)
            {
                return slot;
            }
        }
        return null;
    }

    public void SetRaycast(bool value)
    {
        if (itemFrame == null)
            return;
        foreach (RectTransform itemUI in itemFrame)
        {
            itemUI.GetComponent<CanvasGroup>().blocksRaycasts = value;
        }
    }

    public bool IsOutsideInventory(RectTransform item)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(itemFrame, item.position, null, out localPos);

        return !itemFrame.rect.Contains(localPos);
    }

    public void SetParent(RectTransform item)
    {
        item.SetParent(itemFrame, false);
    }

    void OnCellChange(int x, int y)
    {
        var slot = GetSlot(x, y);
        slot.SetOccupied(!slot.isOccupied);
    }

    void OnAddItem(ItemData item, int x, int y, bool isBelongPlayer)
    {
        GameObject itemObject = Instantiate(itemUIPrefab, itemFrame);
        ItemUI itemUI = itemObject.GetComponent<ItemUI>();

        DragItem dragItem = itemObject.GetComponent<DragItem>();
        dragItem.uiController = uiController;

        itemUI.Init(item, isBelongPlayer);
        itemUI.Resize(gridLayoutGroup.cellSize.x);
        listItem.Add(new Vector2(x, y), itemUI);

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(SnapItemAfterLayout(itemUI, x, y));
    }

    IEnumerator SnapItemAfterLayout(ItemUI itemUI, int x, int y)
    {
        // Wait for end of frame để layout system chạy xong
        yield return new WaitForEndOfFrame();

        // Force rebuild layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(slotFrame);
        Canvas.ForceUpdateCanvases();

        // Get slot and snap
        var slot = GetSlot(x, y);
        if (slot != null)
        {
            itemUI.SnapToSlot(slot);
        }
        else
        {
            Debug.LogError($"Slot not found at ({x}, {y})");
        }
    }

    void OnPlaceItem(ItemData itemData, int x, int y)
    {
        Vector2 posSlot = new Vector2(x, y);

        if (listItem.Count <= 0 || !listItem.ContainsKey(posSlot)) return;

    }

    void OnRemoveItem(int x, int y)
    {
        Vector2 posSlot = new Vector2(x, y);

        if (listItem.Count <= 0 || !listItem.ContainsKey(posSlot)) return;
        listItem.Remove(posSlot);
    }

    void OnDestroyItem(int x, int y)
    {
        Vector2 posSlot = new Vector2(x, y);

        if (listItem.Count <= 0 || !listItem.ContainsKey(posSlot)) return;

        ItemUI itemUI = listItem[posSlot];
        itemUI.Remove();
        listItem.Remove(posSlot);
    }
}
