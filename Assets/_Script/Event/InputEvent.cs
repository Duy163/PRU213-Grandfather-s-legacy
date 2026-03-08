using System;
using UnityEngine;

public static class InputEvent
{
    // Ship
    public static event Action<Vector2> OnMove;
    public static event Action<bool> OnCameraRotate;
    public static event Action OnInteractPressed;
    public static event Action OnOpenInventoryPressed;
    public static event Action OnOpenSettingPressed;

    // Fishing
    public static event Action OnCatchFishPressed;
    public static event Action OnCloseFishingPressed;

    // Cargo
    public static event Action OnCloseInventoryPressed;

    // Dialogue
    public static event Action OnNextDialoguePressed;
    public static event Action OnCloseDialoguePressed;

    // Dock
    public static event Action OnCloseDockPressed;

    //Setting
    public static event Action OnCloseSettingPressed;

    public static event Action OnRotateItemPressed;
    public static event Action OnRemoveItemPressed;
    public static event Action OnRightClickPressed;



    // Ship
    internal static void TriggerMove(Vector2 movement) => OnMove?.Invoke(movement);
    internal static void TriggerCameraRotate(bool isPressed) => OnCameraRotate?.Invoke(isPressed);
    internal static void TriggerInteract() => OnInteractPressed?.Invoke();
    internal static void TriggerOpenInventory() => OnOpenInventoryPressed?.Invoke();
    internal static void TriggerOpenSetting() => OnOpenSettingPressed?.Invoke();

    // Cargo
    internal static void TriggerCloseInventory() => OnCloseInventoryPressed?.Invoke();

    // Fishing
    internal static void TriggerCloseFishing() => OnCloseFishingPressed?.Invoke();


    internal static void TriggerCatchFish() => OnCatchFishPressed?.Invoke();

    // Dialogue
    internal static void TriggerCloseDialogue() => OnCloseDialoguePressed?.Invoke();
    internal static void TriggerNextDialogue() => OnNextDialoguePressed?.Invoke();

    // Dock
    internal static void TriggerCloseDock() => OnCloseDockPressed?.Invoke();

    // Setting
    internal static void TriggerCloseSetting() => OnCloseSettingPressed?.Invoke();

    internal static void TriggerRotateItem() => OnRotateItemPressed?.Invoke();
    internal static void TriggerRemoveItem() => OnRemoveItemPressed?.Invoke();
    internal static void TriggerRightClick() => OnRightClickPressed?.Invoke();

}
