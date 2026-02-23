using System.Collections.Generic;

[System.Serializable]
public class ProgressionData
{
    public List<QuestProgress> activeQuests;
    public List<string> completedQuests;
    public List<string> unlockedAreas;
    public List<string> discoveredSpecies;
    public List<string> viewedDialogues;
    public Dictionary<string, int> npcRelationships;
    public int encyclopediaEntries;
    public string currentMainQuestID;

    public ProgressionData()
    {
        activeQuests = new List<QuestProgress>();
        completedQuests = new List<string>();
        unlockedAreas = new List<string>();
        discoveredSpecies = new List<string>();
        viewedDialogues = new List<string>();
        npcRelationships = new Dictionary<string, int>();
    }
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
