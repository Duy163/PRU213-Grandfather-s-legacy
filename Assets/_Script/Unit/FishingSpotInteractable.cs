using System;
using UnityEngine;

public class FishingSpotInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isAvailable = true;
    [SerializeField] public ItemData item;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private int fishNumber = 2;

    private int numberFishCatched = 0;

    public string GetInteractPrompt()
    {
        return "Hold [F] Fishing";
    }

    public bool CanInteract()
    {
        return isAvailable;
    }

    public void Interact()
    {
        FishingManager.Instance.StartFishing(item);
        numberFishCatched++;
        if (numberFishCatched >= fishNumber)
        {
            SetAvailable(false);
        }
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
            numberFishCatched = 0;
        }
    }
}
