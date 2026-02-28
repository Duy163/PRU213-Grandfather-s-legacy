using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("UI")]
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject FishingUI;
    [SerializeField] private GameObject OtherInventoryUI;
    [SerializeField] private GameObject TalkUI;


    public enum state
    {
        None,
        Inventory,
        Fishing,
        OtherInventory,
        Talk
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        InputEvent.OnOpenInventoryPressed += OnOpenInventory;
        InputEvent.OnCloseInventoryPressed += OnClose;

        StoryEvent.OnStartDialogue += OnOpenTalk;
        StoryEvent.OnEndDialogue += OnClose;

        FishingEvent.OnEnableFishing += OnOpenFishing;
        InventoryEvent.OnInitOtherInventory += OnOpenOtherInventory;
    }

    void OnOpenInventory()
    {
        SetInventoryUI(true);

    }

    void OnOpenFishing(ItemData item)
    {
        SetFishingUI(true);
        SetInventoryUI(true);

    }

    public void OnOpenOtherInventory(InventoryData inventory)
    {
        SetOtherInventoryUI(true);
        SetInventoryUI(true);

    }

    public void OnOpenTalk(NPCData data, StoryDatabase database)
    {
        SetTalkUI(true);

    }

    void OnClose()
    {
        SetInventoryUI(false);
        SetFishingUI(false);
        SetTalkUI(false);
        SetOtherInventoryUI(false);
    }

    public void SetUI(state ui)
    {
        switch (ui)
        {
            case state.None:
                SetInventoryUI(false);
                SetFishingUI(false);
                SetOtherInventoryUI(false);
                SetTalkUI(false);
                break;
            case state.Inventory:
                SetInventoryUI(true);
                SetFishingUI(false);
                SetOtherInventoryUI(false);
                SetTalkUI(false);
                break;
            case state.Fishing:
                SetInventoryUI(true);
                SetFishingUI(true);
                SetOtherInventoryUI(false);
                SetTalkUI(false);
                break;
            case state.OtherInventory:
                SetInventoryUI(true);
                SetFishingUI(false);
                SetOtherInventoryUI(true);
                SetTalkUI(false);
                break;
            case state.Talk:
                SetInventoryUI(false);
                SetFishingUI(false);
                SetOtherInventoryUI(false);
                SetTalkUI(true);
                break;
        }
    }

    public void SetInventoryUI(bool value)
    {
        InventoryUI.SetActive(value);
    }

    public void SetFishingUI(bool value)
    {
        FishingUI.SetActive(value);
    }

    public void SetOtherInventoryUI(bool value)
    {
        OtherInventoryUI.SetActive(value);
    }

    public void SetTalkUI(bool value)
    {
        TalkUI.SetActive(value);
    }
}
