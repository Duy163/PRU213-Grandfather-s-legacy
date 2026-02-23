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
        FishingEvent.OnEnableFishing?.Invoke(item);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
