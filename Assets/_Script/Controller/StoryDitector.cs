using System.Collections.Generic;
using UnityEngine;

public class StoryDirector : Singleton<StoryDirector>
{
    [SerializeField] private StoryDatabase database;

    private ProgressionData progression => DataManager.Instance.Progression;
    private WorldStateManager worldState => DataManager.Instance.WorldState;

    protected override void Awake()
    {
        base.Awake();
        if (database == null)
            Debug.LogError("StoryDirector: No StoryDatabase assigned!");
    }

    // ── KIỂM TRA ĐIỀU KIỆN ────────────────────────────
    public bool CheckConditions(List<StoryCondition> conditions)
    {
        foreach (var c in conditions)
        {
            switch (c.type)
            {
                case ConditionType.HasFlag:
                    if (!worldState.GetFlag(c.key)) return false;
                    break;

                case ConditionType.FlagIsFalse:
                    if (worldState.GetFlag(c.key)) return false;
                    break;

                case ConditionType.ValueGreaterThan:
                    if (worldState.GetValue(c.key) <= c.value) return false;
                    break;

                case ConditionType.ValueEquals:
                    if (worldState.GetValue(c.key) != c.value) return false;
                    break;

                case ConditionType.QuestCompleted:
                    if (!progression.completedQuests.Contains(c.key)) return false;
                    break;

                case ConditionType.QuestActive:
                    if (progression.activeQuests.Find(q => q.questID == c.key) == null)
                        return false;
                    break;

                case ConditionType.QuestStepActive:
                    var quest = progression.activeQuests.Find(q => q.questID == c.key);
                    if (quest == null || quest.currentStep != c.value) return false;
                    break;

                case ConditionType.AreaUnlocked:
                    if (!progression.unlockedAreas.Contains(c.key)) return false;
                    break;
            }
        }
        return true;
    }

    // ── BẮN TRIGGER ───────────────────────────────────
    // private void FireTrigger(StoryTriggerData trigger)
    // {
    //     // Đánh dấu đã bắn — không bao giờ bắn lại
    //     worldState.SetFlag($"trigger_fired_{trigger.triggerID}", true);

    //     Debug.Log($"[StoryDirector] Fired: {trigger.triggerID}");

    //     switch (trigger.action)
    //     {
    //         case TriggerAction.StartQuest:
    //             // QuestManager.Instance.TryStartQuest(trigger.targetID);
    //             break;

    //         case TriggerAction.QueueNPCDialogue:
    //             NPCManager.Instance.QueueDialogue(trigger.targetID, trigger.extraData);
    //             break;

    //         case TriggerAction.RevealMapArea:
    //             progression.unlockedAreas.Add(trigger.targetID);
    //             break;

    //         case TriggerAction.SetFlag:
    //             worldState.SetFlag(trigger.targetID, true);
    //             break;

    //         case TriggerAction.IncrementValue:
    //             worldState.Increment(trigger.targetID);
    //             break;

    //         case TriggerAction.UnlockEncyclopediaEntry:
    //             progression.encyclopediaEntries++;
    //             break;
    //     }

    //     SaveManager.Instance.Save();
    // }
}