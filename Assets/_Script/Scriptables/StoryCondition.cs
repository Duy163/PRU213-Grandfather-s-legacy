
public enum ConditionType
{
    HasFlag,              // flag == true
    FlagIsFalse,          // flag == false
    ValueGreaterThan,     // value > x
    ValueEquals,          // value == x
    QuestCompleted,       // quest đã xong
    QuestActive,          // quest đang làm
    QuestStepActive,    // step của quest đang làm
    AreaUnlocked,         // đã đến vùng này
}

public enum TriggerAction
{
    StartQuest,
    CompleteQuestStep,
    QueueNPCDialogue,
    RevealMapArea,
    SetFlag,
    IncrementValue,
    UnlockEncyclopediaEntry,
}

[System.Serializable]
public class StoryCondition
{
    public ConditionType type;
    public string key;
    public int value;   // dùng cho ValueGreaterThan / ValueEquals
}
