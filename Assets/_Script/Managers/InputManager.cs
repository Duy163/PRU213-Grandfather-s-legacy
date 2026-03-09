using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // ================= State =============================
    private InputActionMap a_ship;
    private InputActionMap a_fishing;
    private InputActionMap a_dialogue;
    private InputActionMap a_cargo;
    private InputActionMap a_dock;
    private InputActionMap a_setting;
    private InputActionMap a_ending;

    private InputAction m_Move;
    private InputAction m_Look;
    private InputAction m_Interact;
    private InputAction m_Catch;
    private InputAction m_Cargo;
    private InputAction m_CloseCargo;
    private InputAction m_CloseFishing;
    private InputAction m_CloseDialogue;
    private InputAction m_RotateItemAction;
    private InputAction m_RemoveItemAction;
    private InputAction m_RightClickAction;
    private InputAction m_Next;
    private InputAction m_CloseDock;
    private InputAction m_OpenSetting;
    private InputAction m_CloseSetting;

    // ================= Public Properties =================
    public InputActionAsset inputActions;

    // ================= Unity Lifecycle ===================

    protected override void Awake()
    {
        base.Awake();

        InitializeActionMaps();
        InitializeActions();
        SubscribeToInputSystem();

        EnableShip();
    }

    // ================= Core Logic ========================

    void InitializeActionMaps()
    {
        a_ship = inputActions.FindActionMap("Ship", true);
        a_fishing = inputActions.FindActionMap("Fishing", true);
        a_dialogue = inputActions.FindActionMap("Dialogue", true);
        a_cargo = inputActions.FindActionMap("Cargo", true);
        a_dock = inputActions.FindActionMap("Dock", true);
        a_setting = inputActions.FindActionMap("Setting", true);
        a_ending = inputActions.FindActionMap("Ending", true);
    }

    void InitializeActions()
    {
        //ship
        m_Move = a_ship.FindAction("Move", true);
        m_Look = a_ship.FindAction("Look", true);
        m_Interact = a_ship.FindAction("Interact", true);
        m_Cargo = a_ship.FindAction("Cargo", true);
        m_OpenSetting = a_ship.FindAction("Setting", true);

        //fishing
        m_Catch = a_fishing.FindAction("Catch", true);
        m_CloseFishing = a_fishing.FindAction("Close", true);

        //cargo
        m_CloseCargo = a_cargo.FindAction("Close", true);

        //dialogue
        m_Next = a_dialogue.FindAction("Next", true);
        m_CloseDialogue = a_dialogue.FindAction("Close", true);

        //dock
        m_CloseDock = a_dock.FindAction("Close", true);

        //setting
        m_CloseSetting = a_setting.FindAction("Close", true);

        // m_RotateItemAction = a_cargo.FindAction("RotateItem", true);
        // m_RemoveItemAction = a_cargo.FindAction("RemoveItem", true);
        // m_RightClickAction = a_ship.FindAction("RightClick", true);

    }

    void SubscribeToInputSystem()
    {
        //ship
        m_Move.performed += ctx => InputEvent.TriggerMove(ctx.ReadValue<Vector2>());
        m_Move.canceled += ctx => InputEvent.TriggerMove(Vector2.zero);
        m_Look.started += ctx => InputEvent.TriggerCameraRotate(true);
        m_Look.canceled += ctx => InputEvent.TriggerCameraRotate(false);
        m_Interact.performed += ctx => InputEvent.TriggerInteract();
        m_Cargo.performed += ctx => InputEvent.TriggerOpenInventory();
        m_OpenSetting.performed += ctx => InputEvent.TriggerOpenSetting();

        //fishing
        m_Catch.performed += ctx => InputEvent.TriggerCatchFish();
        m_CloseFishing.performed += ctx => InputEvent.TriggerCloseFishing();

        //cargo
        m_CloseCargo.performed += ctx => InputEvent.TriggerCloseInventory();


        //dialogue
        m_Next.performed += ctx => InputEvent.TriggerNextDialogue();
        m_CloseDialogue.performed += ctx => InputEvent.TriggerCloseDialogue();

        //dock
        m_CloseDock.performed += ctx => InputEvent.TriggerCloseDock();

        //setting
        m_CloseSetting.performed += ctx => InputEvent.TriggerCloseSetting();

        // m_RotateItemAction.performed += ctx => InputEvent.TriggerRotateItem();
        // m_RemoveItemAction.performed += ctx => InputEvent.TriggerRemoveItem();
        // m_RightClickAction.started += ctx => InputEvent.TriggerRightClick();
    }

    // ══════════════════════════════════════════════════
    //  SWITCH CONTEXT — gọi khi đổi trạng thái game
    // ══════════════════════════════════════════════════

    public void EnableShip()
    {
        a_ship.Enable();
        a_fishing.Disable();
        a_dialogue.Disable();
        a_cargo.Disable();
        a_dock.Disable();
        a_setting.Disable();
        a_ending.Disable();
        Debug.Log("[Input] Ship mode");
    }

    public void EnableFishing()
    {
        a_ship.Disable();
        a_fishing.Enable();
        a_dialogue.Disable();
        a_cargo.Disable();
        a_dock.Disable();
        a_setting.Disable();
        a_ending.Disable();
        Debug.Log("[Input] Fishing mode");
    }

    public void EnableDialogue()
    {
        a_ship.Disable();
        a_fishing.Disable();
        a_dialogue.Enable();
        a_dock.Disable();
        a_cargo.Disable();
        a_setting.Disable();
        a_ending.Disable();
        Debug.Log("[Input] Dialogue mode");
    }

    public void EnableCargo()
    {
        a_ship.Disable();
        a_fishing.Disable();
        a_dialogue.Disable();
        a_dock.Disable();
        a_cargo.Enable();
        a_setting.Disable();
        a_ending.Disable();
        Debug.Log("[Input] Cargo mode");
    }

    public void EnableDock()
    {
        a_ship.Disable();
        a_fishing.Disable();
        a_dialogue.Disable();
        a_cargo.Disable();
        a_dock.Enable();
        a_setting.Disable();
        a_ending.Disable();
        Debug.Log("[Input] Dock mode");
    }

    public void EnableSetting()
    {
        a_ship.Disable();
        a_fishing.Disable();
        a_dialogue.Disable();
        a_cargo.Disable();
        a_dock.Disable();
        a_setting.Enable();
        a_ending.Disable();
        Debug.Log("[Input] Setting mode");
    }

    public void EnableEnding()
    {
        a_ship.Disable();
        a_fishing.Disable();
        a_dialogue.Disable();
        a_cargo.Disable();
        a_dock.Disable();
        a_setting.Disable();
        a_ending.Enable();
        Debug.Log("[Input] Ending mode");
    }
    public Vector2 GetMovement()
    {
        return m_Move.ReadValue<Vector2>();
    }

    public bool RemoveItem()
    {
        return m_RemoveItemAction.WasPressedThisFrame();
    }
}
