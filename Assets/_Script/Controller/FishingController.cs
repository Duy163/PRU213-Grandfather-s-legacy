using UnityEngine;

public class FishingController : MonoBehaviour
{
    // ================= CONSTANTS =========================

    // ================= Serialized Fields =================

    [SerializeField] TimingBarView timingBarView;

    // ================= State =============================

    private IFishingMinigame vm;
    private ItemData item;
    private bool canFishing = false;

    // ================= Public Properties =================

    // ================= Unity Lifecycle ===================

    void OnEnable()
    {
        // InputEvent.OnOpenFishingPressed += OnOpenFishing;
        // InputEvent.OnCatchFishPressed += OnCatchFish;
        // InputEvent.OnCloseFishingPressed += OnCloseFishing;

        // FishingEvent.OnEnableFishing += OnOpenFishing;
        // FishingEvent.OnUnableFishing += HandleUnableFishing;
    }

    void OnDisable()
    {
        // InputEvent.OnOpenFishingPressed -= OnOpenFishing;
        // InputEvent.OnCatchFishPressed -= OnCatchFish;
        // InputEvent.OnCloseFishingPressed -= OnCloseFishing;

        // FishingEvent.OnEnableFishing -= OnOpenFishing;
        // FishingEvent.OnUnableFishing -= HandleUnableFishing;
    }

    // ================= Input Handling ====================

    void OnOpenFishing(ItemData item)
    {
        // if (item == null || !canFishing) return;
        this.item = item;
        canFishing = true;
        InventoryManager.Instance.OnOpenInventoryPressed();


        Init(this.item);
        InputManager.Instance.EnableFishing();
        vm.Start();
    }

    void OnCatchFish()
    {
        if (item == null || !canFishing) return;

        if (vm == null) return;
        bool result = timingBarView.IsSuccess();
        bool isFinish = vm.Handle(result);

        if (isFinish)
        {
            canFishing = false;
            InventoryEvent.OnAddItem?.Invoke(item);
        }
    }

    void OnCloseFishing()
    {
        InventoryManager.Instance.OnCloseInventoryPressed();
        canFishing = false;
        Clear();
        InputManager.Instance.EnableShip();
    }

    // ================= Initialization ====================

    void Init(ItemData item)
    {
        if (vm != null) vm = null;

        switch (item.minigameType)
        {
            case MinigameType.TimingBar:
                var temp = new TimingBarViewModel(item, CaculatorDificult(item));
                vm = temp;
                timingBarView.Bind(temp);
                timingBarView.Show();
                break;
            case MinigameType.none:
                break;
        }
    }

    // ================= Core Logic ========================

    void Clear()
    {
        if (timingBarView != null)
        {
            timingBarView.Hide();
            timingBarView.UnBind();
        }
        vm = null;
    }

    void HandleEnableFishing(ItemData item)
    {
        this.item = item;
        canFishing = true;
    }

    void HandleUnableFishing()
    {
        item = null;
        canFishing = false;

        Clear();
    }

    // ================= Subsystem =========================

    // ================= Event Handlers ====================

    // ================= Public API ========================

    // ================= Helpers ===========================
    float CaculatorDificult(ItemData item)
    {
        return Mathf.Clamp01(item.weight / item.MAX_WEIGHT * 0.3f + item.value / item.MAX_VALUE * 0.7f);
    }

    // ================= Debug / Editor ====================



}
