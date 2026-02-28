using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/Database")]
public class StoryDatabase : ScriptableObject
{
    public List<StoryTriggerData> allTriggers;

    // Tuần 3 sẽ dùng tiếp
    public List<NPCData> allNPCs;
    public List<DialogueData> allDialogues;
    public List<QuestData> allQuests;
}
