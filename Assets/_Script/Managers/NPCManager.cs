using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    // Queue dialogue chờ — StoryDirector gọi vào đây
    private Dictionary<string, Queue<string>> pendingDialogues
        = new Dictionary<string, Queue<string>>();

    protected override void Awake()
    {
        base.Awake();
    }

    // StoryDirector gọi khi trigger bắn
    public void QueueDialogue(string npcID, string dialogueID)
    {
        if (!pendingDialogues.ContainsKey(npcID))
            pendingDialogues[npcID] = new Queue<string>();

        pendingDialogues[npcID].Enqueue(dialogueID);
        Debug.Log($"[NPCManager] Queued {dialogueID} for {npcID}");
    }

    // NPCController gọi khi player interact
    public string DequeueDialogue(string npcID)
    {
        if (pendingDialogues.ContainsKey(npcID)
            && pendingDialogues[npcID].Count > 0)
            return pendingDialogues[npcID].Dequeue();

        return null;
    }
}