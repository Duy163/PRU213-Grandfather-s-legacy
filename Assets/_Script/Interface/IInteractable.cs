using UnityEngine;

public interface IInteractable
{
    string GetInteractPrompt(); // "Press E to Fish", "Press E to Talk"
    void Interact(GameObject player);
    bool CanInteract(); // Kiểm tra điều kiện
    Transform GetTransform();
}
