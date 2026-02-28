using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/NPC")]
public class NPCData : ScriptableObject
{
    public string npcID;
    public string displayName;
    public List<ConditionalDialogue> dialoguePool;
}

[System.Serializable]
public class ConditionalDialogue
{
    public DialogueData dialogue;
    public int priority;
    public bool oneTimeOnly;
    public List<StoryCondition> conditions;
    [TextArea]
    public string editorNote;

    public CheckType checkType;    // kiểm tra gì khi player talk
    public string checkQuestID; // quest nào cần check
    public int checkStep;    // đang ở step nào
}

public enum CheckType
{
    None,                  // không kiểm tra gì, show dialogue bình thường
    CheckQuestStep,        // kiểm tra player đang ở step nào
    CheckObjectivesDone,   // kiểm tra objectives của step hiện tại xong chưa

}