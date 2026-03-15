using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private StoryDirector storyDirector;
    private NPCData data;
    private StoryDatabase database;

    void OnEnable()
    {
        StoryEvent.OnStartDialogue -= Interact;
        StoryEvent.OnStartDialogue += Interact;
    }

    void OnDisable()
    {
        StoryEvent.OnStartDialogue -= Interact;
    }
    public void Interact(NPCData data, StoryDatabase database)
    {
        Debug.Log($"Interact from {gameObject.name} instance {GetInstanceID()}");
        this.data = data;
        this.database = database;

        InputManager.Instance.EnableDialogue();

        // Ưu tiên 2: tìm trong dialoguePool
        ConditionalDialogue best = FindBestDialogue();
        if (best == null)
        {
            Debug.Log($"[NPC] {data.displayName} không có gì để nói.");
            return;
        }

        DialogueData toPlay = ResolveDialogue(best);
        if (toPlay != null)
        {
            AudioManager.Instance.PlaySoundVillage();
            dialogueManager.StartDialogue(toPlay);
        }
        else Debug.Log("no dialogue to play");
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

            if (!storyDirector.CheckConditions(cd.conditions))
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
        if (cd.oneTimeOnly && result != null && cd.dialogue.dialogueID == result.dialogueID)
            DataManager.Instance.WorldState
                       .SetFlag($"dialogue_seen_{cd.dialogue.dialogueID}", true);
        return result;
    }

    // NPC kiểm tra player đang ở step nào
    private DialogueData ResolveByQuestStep(ConditionalDialogue cd)
    {
        var quest = questManager.GetActiveQuest(cd.checkQuestID);
        if (quest == null)
            return GetDialogue(cd.dialogue.dialogueID);

        if (quest.currentStep == cd.checkStep)
            questManager.ReportProgress(ObjectiveType.TalkToNPC, data.npcID);

        return GetDialogue(cd.dialogue.dialogueID);
    }

    private DialogueData ResolveByObjectives(ConditionalDialogue cd)
    {
        bool passed = questManager.EvaluateQuest(
                          cd.checkQuestID,
                          cd.checkStep);

        if (!passed) Debug.Log(cd.dialogue.dialogueID + "_reminder");

        if (!passed)
            return GetDialogue(cd.dialogue.dialogueID + "_reminder");

        // Đủ điều kiện → consume và advance
        questManager.ConsumeAndAdvance(cd.checkQuestID, cd.checkStep);

        return GetDialogue(cd.dialogue.dialogueID);
    }

    private DialogueData GetDialogue(string dialogueID)
        => database.allDialogues.Find(d => d.dialogueID == dialogueID);
}
