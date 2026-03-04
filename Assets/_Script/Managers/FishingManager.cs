using UnityEngine;

public class FishingManager : Singleton<FishingManager>
{
    [SerializeField] TimingBarView timingBarView;

    private IFishingMinigame vm;
    private ItemData item;
    private bool canFishing = false;
    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        InputEvent.OnCatchFishPressed += OnCatchFish;
        InputEvent.OnCloseFishingPressed += StopFishing;
    }

    void OnDisable()
    {
        InputEvent.OnCatchFishPressed -= OnCatchFish;
        InputEvent.OnCloseFishingPressed -= StopFishing;
    }

    public void StartFishing(ItemData item)
    {
        this.item = item;
        canFishing = true;

        InventoryManager.Instance.OnOpenInventoryPressed();
        CameraManager.Instance.EnterFishingView();

        InitAndOpen(item);
        InputManager.Instance.EnableFishing();
        vm.Start();
    }

    public void StopFishing()
    {
        canFishing = false;

        InventoryManager.Instance.OnCloseInventoryPressed();
        CameraManager.Instance.NormalView();

        Clear();
        InputManager.Instance.EnableShip();
        vm = null;
    }

    //private

    void OnCatchFish()
    {
        if (item == null || !canFishing) return;

        if (vm == null) return;
        bool result = timingBarView.IsSuccess();
        bool isFinish = vm.Handle(result);

        if (isFinish)
        {
            canFishing = false;
            InventoryManager.Instance.AddItemForPlayer(item);
        }
    }

    void InitAndOpen(ItemData item)
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

    void Clear()
    {
        if (timingBarView != null)
        {
            timingBarView.Hide();
            timingBarView.UnBind();
        }
    }

    //Helper

    float CaculatorDificult(ItemData item)
    {
        return Mathf.Clamp01(item.weight / item.MAX_WEIGHT * 0.3f + item.value / item.MAX_VALUE * 0.7f);
    }

}
