using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : Singleton<TriggerManager>
{
    [Header("UI")]
    [SerializeField] private EndingView endingView;
    [SerializeField] private Billboard billboard;
    [SerializeField] private CompassView compassView;

    [SerializeField] List<NPCInteracttive> nPCs;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (DataManager.Instance.currentGameData.loginCount == 0)
        {
            TryTrigger("goldfish");
        }
    }

    public void TryTrigger(string triggerID)
    {
        switch (triggerID)
        {
            case "normal_ending":
                ShowEnding();
                break;
            case "goldfish":
                ShowBillBoard(triggerID);
                break;
            case "fisher":
                ShowBillBoard(triggerID);
                break;
            case "village":
                ShowBillBoard(triggerID);
                break;
            case "mayor":
                ShowBillBoard(triggerID);
                break;
            case "hide":
                HideBillBoard();
                break;
        }
    }

    void ShowBillBoard(string id)
    {
        foreach (NPCInteracttive npc in nPCs)
        {
            if (npc.GetName() == id)
            {
                // billboard.target = npc.GetTransform();
                compassView.currentQuestTarget = npc.GetTransform();
                break;
            }
        }
    }

    void HideBillBoard()
    {
        compassView.currentQuestTarget = null;
    }

    void ShowEnding()
    {
        endingView.Show();
        InputManager.Instance.EnableEnding();
    }
}
