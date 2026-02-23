using UnityEngine;

public class DockInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isAvailable = true;
    [SerializeField] private ItemData item;

    InventoryData inventory = new InventoryData(5, 5);

    public string GetInteractPrompt()
    {
        return "Hold [F] Open Chest";
    }

    public bool CanInteract()
    {
        return isAvailable;
    }

    public void Interact(GameObject player)
    {
        InventoryEvent.OnInitOtherInventory?.Invoke(inventory);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
