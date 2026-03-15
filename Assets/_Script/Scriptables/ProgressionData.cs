using System.Collections.Generic;

[System.Serializable]
public class ProgressionData
{
    // ── QUEST ──────────────────────────────────────────
    public List<QuestProgress> activeQuests = new List<QuestProgress>();
    public List<string> completedQuests = new List<string>();

    // ── WORLD ──────────────────────────────────────────
    public List<string> unlockedAreas = new List<string>();
    public List<string> discoveredSpecies = new List<string>();
    public List<string> viewedDialogues = new List<string>();

    // ── NPC ────────────────────────────────────────────
    public Dictionary<string, int> npcRelationships = new Dictionary<string, int>();

    // ── MISC ───────────────────────────────────────────
    public int encyclopediaEntries;
    public string currentMainQuestID;
}

[System.Serializable]
public class QuestProgress
{
    public string questID;
    public string questName;
    public int currentStep;
    public List<QuestObjective> objectives;
    public bool isMainQuest;

    public QuestProgress(string id, string name, bool mainQuest)
    {
        questID = id;
        questName = name;
        currentStep = 0;
        objectives = new List<QuestObjective>();
        isMainQuest = mainQuest;
    }
}

[System.Serializable]
public class QuestObjective
{
    public string description;
    public int currentProgress;
    public int targetProgress;
    public bool isCompleted;
}
