using System;
using UnityEngine;

public class FishingSpotInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private FishingManager fishingManager;
    [SerializeField] private bool isAvailable = true;
    [SerializeField] public ItemData item;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] public int fishInSchool = 1;

    public string GetInteractPrompt()
    {
        return "Giữ [F] Câu cá";
    }

    public bool CanInteract()
    {
        return isAvailable;
    }

    public void Interact()
    {
        fishingManager.StartFishing(item, this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetAvailable(bool available)
    {
        isAvailable = available;
        meshRenderer.enabled = available;
        if (available)
        {
            fishInSchool = item.schoolSize;
        }
    }
}
