using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // ================= CONSTANTS =========================

    // ================= Serialized Fields =================

    // ================= State =============================

    private InputAction m_MoveAction;
    private InputAction m_RotateCameraAction;
    private InputAction m_InteractAction;
    private InputAction m_CatchFishAction;
    private InputAction m_OpenInventoryAction;
    private InputAction m_CloseInventoryAction;
    private InputAction m_RotateItemAction;
    private InputAction m_RemoveItemAction;
    private InputAction m_RightClickAction;
    private InputAction m_NextDialogue;

    // ================= Public Properties =================
    public InputActionAsset inputActions;

    // ================= Unity Lifecycle ===================

    protected override void Awake()
    {
        base.Awake();
        EnableUIInput(false);

        InitializeActions();
        SubscribeToInputSystem();
    }

    // ================= Input Handling ====================

    // ================= Initialization ====================

    // ================= Core Logic ========================

    private void OnEnablePlayer()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisablePlayer()
    {
        inputActions.FindActionMap("Player").Disable();
    }
    private void OnEnableUI()
    {
        inputActions.FindActionMap("UI").Enable();
    }
    private void OnDisableUI()
    {
        inputActions.FindActionMap("UI").Disable();
    }

    void InitializeActions()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_RotateCameraAction = InputSystem.actions.FindAction("RotateCamera");
        m_InteractAction = InputSystem.actions.FindAction("Interact");
        m_CatchFishAction = InputSystem.actions.FindAction("CatchFish");
        m_OpenInventoryAction = InputSystem.actions.FindAction("OpenInventory");
        m_CloseInventoryAction = InputSystem.actions.FindAction("CloseInventory");
        m_RotateItemAction = InputSystem.actions.FindAction("RotateItem");
        m_RemoveItemAction = InputSystem.actions.FindAction("RemoveItem");
        m_RightClickAction = InputSystem.actions.FindAction("RightClick");
        m_NextDialogue = InputSystem.actions.FindAction("NextDialogue");
    }

    void SubscribeToInputSystem()
    {
        m_MoveAction.performed += ctx => InputEvent.TriggerMove(ctx.ReadValue<Vector2>());
        m_MoveAction.canceled += ctx => InputEvent.TriggerMove(Vector2.zero);

        // Player actions
        m_RotateCameraAction.started += ctx => InputEvent.TriggerCameraRotate(true);
        m_RotateCameraAction.canceled += ctx => InputEvent.TriggerCameraRotate(false);
        m_InteractAction.performed += ctx => InputEvent.TriggerInteract();
        m_CatchFishAction.performed += ctx => InputEvent.TriggerCatchFish();

        // UI actions
        m_OpenInventoryAction.performed += ctx => InputEvent.TriggerOpenInventory();
        m_CloseInventoryAction.performed += ctx => InputEvent.TriggerCloseInventory();
        m_RotateItemAction.performed += ctx => InputEvent.TriggerRotateItem();
        m_RemoveItemAction.performed += ctx => InputEvent.TriggerRemoveItem();
        m_RightClickAction.started += ctx => InputEvent.TriggerRightClick();
        m_NextDialogue.performed += ctx => InputEvent.TriggerNextDialogue();
    }

    // ================= Subsystem =========================

    // ================= Event Handlers ====================

    // ================= Public API ========================

    public void EnableUIInput(bool enable)
    {
        if (enable)
        {
            OnEnableUI();
            OnDisablePlayer();
        }
        else
        {
            OnDisableUI();
            OnEnablePlayer();
        }
    }

    // ================= Helpers ===========================

    // ================= Debug / Editor ====================


    public Vector2 GetMovement()
    {
        return m_MoveAction.ReadValue<Vector2>();
    }

    public bool RemoveItem()
    {
        return m_RemoveItemAction.WasPressedThisFrame();
    }
}
