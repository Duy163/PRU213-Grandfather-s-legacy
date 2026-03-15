using UnityEngine;

public class UpgradeInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] ShipManager shipManager;
    public string GetInteractPrompt()
    {
        return "Hold [F] Open Upgrade";
    }
    public void Interact()
    {
        shipManager.OnOpenUpgrade();
    }
    public bool CanInteract()
    {
        return true;
    } // Kiểm tra điều kiện
    public Transform GetTransform()
    {
        return transform;
    }
}
