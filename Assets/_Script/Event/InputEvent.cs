using System;
using UnityEngine;

public static class InputEvent
{
    // Player Actions
    public static event Action<Vector2> OnMove;
    public static event Action<Vector2> OnLook;
    public static event Action<bool> OnCameraRotate; // true = pressed, false = released

    // Gameplay Actions
    public static event Action OnInteractPressed;
    public static event Action OnCatchFishPressed;

    // UI Actions
    public static event Action OnOpenInventoryPressed;
    public static event Action OnCloseInventoryPressed;
    public static event Action OnRotateItemPressed;
    public static event Action OnRemoveItemPressed;


    internal static void TriggerMove(Vector2 movement) => OnMove?.Invoke(movement);
    internal static void TriggerLook(Vector2 lookDelta) => OnLook?.Invoke(lookDelta);
    internal static void TriggerCameraRotate(bool isPressed) => OnCameraRotate?.Invoke(isPressed);
    internal static void TriggerInteract()
    {
        OnInteractPressed?.Invoke();
        OnOpenInventoryPressed?.Invoke();
    }
    internal static void TriggerCatchFish() => OnCatchFishPressed?.Invoke();
    internal static void TriggerOpenInventory() => OnOpenInventoryPressed?.Invoke();
    internal static void TriggerCloseInventory()
    {
        OnCloseInventoryPressed?.Invoke();
        FishingEvent.OnUnableFishing?.Invoke();
    }
    internal static void TriggerRotateItem() => OnRotateItemPressed?.Invoke();
    internal static void TriggerRemoveItem() => OnRemoveItemPressed?.Invoke();
}
