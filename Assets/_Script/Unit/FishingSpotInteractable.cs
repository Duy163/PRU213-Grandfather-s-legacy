using UnityEngine;

public class FishingSpotInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isAvailable = true;
    [SerializeField] private ItemData item;

    public string GetInteractPrompt()
    {
        return "Hold [F] Fishing";
    }

    public bool CanInteract()
    {
        return isAvailable;
    }

    public void Interact(GameObject player)
    {
        FishingManager.Instance.StartFishing(item);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
