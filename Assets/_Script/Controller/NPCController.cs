using UnityEngine;

public class NPCController : MonoBehaviour
{
    private NPCData data;
    private StoryDatabase database;

    void OnEnable()
    {
        StoryEvent.OnStartDialogue += Interact;
    }

    void OnDisable()
    {
        StoryEvent.OnStartDialogue -= Interact;
    }
    public void Interact(NPCData data, StoryDatabase database)
    {
        this.data = data;
        this.database = database;

        // Ưu tiên 1: dialogue được StoryDirector queue
        string queuedID = NPCManager.Instance.DequeueDialogue(data.npcID);
        if (!string.IsNullOrEmpty(queuedID))
        {
            DialogueData queued = GetDialogue(queuedID);
            if (queued != null)
            {
                DialogueManager.Instance.StartDialogue(queued);
                return;
            }
        }

        // Ưu tiên 2: tìm trong dialoguePool
        ConditionalDialogue best = FindBestDialogue();
        if (best == null)
        {
            Debug.Log($"[NPC] {data.displayName} không có gì để nói.");
            return;
        }

        DialogueData toPlay = ResolveDialogue(best);
        if (toPlay != null)
            DialogueManager.Instance.StartDialogue(toPlay);
    }

    // ── TÌM DIALOGUE PHÙ HỢP NHẤT ─────────────────────
    private ConditionalDialogue FindBestDialogue()
    {
        ConditionalDialogue best = null;
        foreach (var cd in data.dialoguePool)
        {
            if (cd.oneTimeOnly
                && DataManager.Instance.WorldState
                              .GetFlag($"dialogue_seen_{cd.dialogue.dialogueID}"))
                continue;

            if (!StoryDirector.Instance.CheckConditions(cd.conditions))
                continue;

            if (best == null || cd.priority > best.priority)
                best = cd;
        }
        return best;
    }

    // ── NPC TỰ KIỂM TRA QUEST KHI PLAYER NÓI CHUYỆN ──
    private DialogueData ResolveDialogue(ConditionalDialogue cd)
    {
        DialogueData result = null;

        switch (cd.checkType)
        {
            case CheckType.None:
                result = GetDialogue(cd.dialogue.dialogueID);
                break;

            case CheckType.CheckQuestStep:
                result = ResolveByQuestStep(cd);
                break;

            case CheckType.CheckObjectivesDone:
                result = ResolveByObjectives(cd);
                break;
        }

        // ── Gắn cờ tập trung tại đây, bỏ trong 2 hàm kia ──
        if (cd.oneTimeOnly && result != null)
            DataManager.Instance.WorldState
                       .SetFlag($"dialogue_seen_{cd.dialogue.dialogueID}", true);

        return result;
    }

    // NPC kiểm tra player đang ở step nào
    private DialogueData ResolveByQuestStep(ConditionalDialogue cd)
    {
        var quest = QuestManager.Instance.GetActiveQuest(cd.checkQuestID);
        if (quest == null)
            return GetDialogue(cd.dialogue.dialogueID);

        if (quest.currentStep == cd.checkStep)
            QuestManager.Instance.ReportProgress(ObjectiveType.TalkToNPC, data.npcID);

        return GetDialogue(cd.dialogue.dialogueID);
    }

    private DialogueData ResolveByObjectives(ConditionalDialogue cd)
    {
        bool passed = QuestManager.Instance.EvaluateQuest(
                          cd.checkQuestID,
                          cd.checkStep);

        if (!passed)
            return GetDialogue(cd.dialogue.dialogueID + "_reminder");

        // Đủ điều kiện → consume và advance
        QuestManager.Instance.ConsumeAndAdvance(cd.checkQuestID, cd.checkStep);

        return GetDialogue(cd.dialogue.dialogueID);
    }

    // private DialogueData ResolveByObjectives(ConditionalDialogue cd)
    // {
    //     var quest = QuestManager.Instance.GetActiveQuest(cd.checkQuestID);
    //     if (quest == null)
    //         return GetDialogue(cd.dialogue.dialogueID);

    //     bool allDone = quest.objectives.TrueForAll(o => o.isCompleted);

    //     if (!allDone)
    //         return GetDialogue(cd.dialogue.dialogueID + "_reminder");

    //     QuestManager.Instance.ReportProgress(ObjectiveType.TalkToNPC, data.npcID);

    //     return GetDialogue(cd.dialogue.dialogueID);
    // }

    private DialogueData GetDialogue(string dialogueID)
        => database.allDialogues.Find(d => d.dialogueID == dialogueID);
}
