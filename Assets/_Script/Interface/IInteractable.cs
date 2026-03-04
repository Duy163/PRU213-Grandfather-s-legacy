using UnityEngine;

public interface IInteractable
{
    string GetInteractPrompt(); // "Press E to Fish", "Press E to Talk"
    void Interact();
    bool CanInteract(); // Kiểm tra điều kiện
    Transform GetTransform();
}
