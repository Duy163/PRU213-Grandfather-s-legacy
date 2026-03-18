using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private StoryDatabase database;
    [SerializeField] private QuestView questView;

    private ProgressionData progression => DataManager.Instance.Progression;
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (progression.activeQuests.Count > 0)
        {
            questView.Show();
            questView.SetContent(progression.activeQuests[0].questName);
        }
    }
    // ── MỞ QUEST ──────────────────────────────────────
    public bool TryStartQuest(string questID)
    {
        // Đã làm rồi hoặc đang làm thì bỏ qua
        if (progression.completedQuests.Contains(questID)) return false;
        if (progression.activeQuests.Exists(q => q.questID == questID)) return false;

        QuestData data = database.allQuests.Find(q => q.questID == questID);
        if (data == null)
        {
            Debug.LogWarning($"[QuestManager] Không tìm thấy quest: {questID}");
            return false;
        }

        // Kiểm tra prerequisite
        foreach (var preID in data.prerequisiteQuestIDs)
        {
            if (!progression.completedQuests.Contains(preID))
                return false;
        }

        // Tạo QuestProgress từ step đầu tiên
        var progress = new QuestProgress(data.questID, data.questName, data.isMainQuest);
        LoadObjectives(progress, data, stepIndex: 0);

        progression.activeQuests.Add(progress);
        Debug.Log($"[QuestManager] Bắt đầu quest: {data.questName}");

        questView.Show();
        questView.SetContent(data.questName);

        DataManager.Instance.Save();
        return true;
    }

    // ── BÁO TIẾN ĐỘ ───────────────────────────────────
    public void ReportProgress(ObjectiveType type, string targetID, int amount = 1)
    {
        foreach (var quest in progression.activeQuests.ToList())
        {
            QuestData data = database.allQuests.Find(q => q.questID == quest.questID);
            if (data == null) continue;

            var stepObjectives = data.steps[quest.currentStep].objectives;

            for (int i = 0; i < stepObjectives.Count; i++)
            {
                if (stepObjectives[i].type != type) continue;
                if (stepObjectives[i].targetID != targetID) continue;

                quest.objectives[i].currentProgress += amount;

                if (quest.objectives[i].currentProgress >= quest.objectives[i].targetProgress)
                    quest.objectives[i].isCompleted = true;
            }

            TryAdvanceStep(quest, data);
        }
    }

    public bool EvaluateQuest(string questID, int stepIndex)
    {
        var quest = progression.activeQuests.Find(q => q.questID == questID);
        if (quest == null) return false;

        var data = database.allQuests.Find(q => q.questID == questID);
        if (data == null) return false;

        var stepObjectives = data.steps[stepIndex].objectives;

        foreach (var obj in stepObjectives)
        {
            bool result = EvaluateSingleObjective(obj, quest);
            if (!result) return false;
        }

        return true;
    }

    private bool EvaluateSingleObjective(ObjectiveData obj, QuestProgress quest)
    {
        switch (obj.type)
        {
            case ObjectiveType.TalkToNPC:
                // Được đánh dấu bởi ReportProgress() khi player interact NPC
                // Tìm objective tương ứng trong quest.objectives và check isCompleted
                return quest.objectives
                            .Find(o => o.description == obj.description)
                            ?.isCompleted ?? false;

            case ObjectiveType.ReachArea:
                // Được đánh dấu khi player đến vùng đó
                return quest.objectives
                            .Find(o => o.description == obj.description)
                            ?.isCompleted ?? false;

            case ObjectiveType.CheckInventory:
                int count = InventoryManager.Instance.CountItemInPlayerInventory(obj.targetID);
                Debug.Log($"[QuestManager] Kiểm tra inventory: {obj.targetID} x {count}/{obj.targetAmount}");

                return count >= obj.targetAmount;

            case ObjectiveType.CheckCurrency:
                float currentMoney = currencyManager.GetCurrentMoney();
                Debug.Log($"[QuestManager] Kiểm tra tiền: {currentMoney}/{obj.targetAmount}");

                return currentMoney >= obj.targetAmount;

            default:
                Debug.LogWarning($"[QuestManager] Chưa xử lý ObjectiveType: {obj.type}");
                return false;
        }
    }

    // ── ADVANCE STEP ───────────────────────────────────

    public void ConsumeAndAdvance(string questID, int stepIndex)
    {
        var quest = progression.activeQuests.Find(q => q.questID == questID);
        var data = database.allQuests.Find(q => q.questID == questID);
        if (quest == null || data == null) return;

        var stepObjectives = data.steps[stepIndex].objectives;

        // Consume từng objective có hệ thống
        for (int i = 0; i < stepObjectives.Count; i++)
        {
            switch (stepObjectives[i].type)
            {
                case ObjectiveType.CheckInventory:
                    InventoryManager.Instance
                                    .DestroyListItemInPlayerInventory(stepObjectives[i].targetID,
                                                                        stepObjectives[i].targetAmount);

                    // ── THÊM: đánh dấu xong ───────────────
                    quest.objectives[i].currentProgress = stepObjectives[i].targetAmount;
                    quest.objectives[i].isCompleted = true;
                    break;

                case ObjectiveType.CheckCurrency:
                    bool success = currencyManager.SpendMoney(stepObjectives[i].targetAmount);
                    if (!success) return;

                    // ── THÊM: đánh dấu xong ───────────────
                    quest.objectives[i].currentProgress = stepObjectives[i].targetAmount;
                    quest.objectives[i].isCompleted = true;
                    break;

                    // TalkToNPC, ReachArea, CatchFish
                    // đã được set bởi ReportProgress() rồi → không đụng vào
            }
        }

        TryAdvanceStep(quest, data);
    }
    private void TryAdvanceStep(QuestProgress quest, QuestData data)
    {
        if (!quest.objectives.TrueForAll(o => o.isCompleted)) return;

        quest.currentStep++;

        if (quest.currentStep >= data.steps.Count)
        {
            CompleteQuest(quest, data);
        }
        else
        {
            // Load objectives của step mới
            LoadObjectives(quest, data, quest.currentStep);
            Debug.Log($"[QuestManager] {quest.questName} → Step {quest.currentStep}");
        }
    }

    // ── HOÀN THÀNH QUEST ───────────────────────────────
    private void CompleteQuest(QuestProgress quest, QuestData data)
    {
        progression.activeQuests.Remove(quest);
        progression.completedQuests.Add(quest.questID);

        Debug.Log($"[QuestManager] Hoàn thành: {quest.questName}");

        questView.Hide();

        // Tự mở quest tiếp theo trong chuỗi
        if (!string.IsNullOrEmpty(data.nextQuestID))
            TryStartQuest(data.nextQuestID);
        DataManager.Instance.Save();
    }

    // ── HELPER ─────────────────────────────────────────
    private void LoadObjectives(QuestProgress quest, QuestData data, int stepIndex)
    {
        quest.objectives.Clear();
        foreach (var obj in data.steps[stepIndex].objectives)
        {
            quest.objectives.Add(new QuestObjective
            {
                description = obj.description,
                targetProgress = obj.targetAmount,
                currentProgress = 0,
                isCompleted = false
            });
        }
    }

    // ── TIỆN ÍCH ───────────────────────────────────────
    public bool IsQuestActive(string questID)
        => progression.activeQuests.Exists(q => q.questID == questID);

    public bool IsQuestCompleted(string questID)
        => progression.completedQuests.Contains(questID);

    public QuestProgress GetActiveQuest(string questID)
        => progression.activeQuests.Find(q => q.questID == questID);

}
