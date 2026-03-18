using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string dialogueID;
    public List<DialogueLine> lines;
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea]
    public string text;
    public Sprite portrait;
    public AudioClip clip;

    // Hành động sau khi nói dòng này
    public string setFlagAfter;      // bật flag
    public string triggerQuestID;    // mở quest
    public string triggerID;
}