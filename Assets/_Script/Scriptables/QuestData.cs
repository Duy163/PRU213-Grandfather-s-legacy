using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questName;
    public bool isMainQuest;
    public string nextQuestID;
    public List<string> prerequisiteQuestIDs;
    public List<QuestStepData> steps;
}

[System.Serializable]
public class QuestStepData
{
    public string stepDescription;
    public List<ObjectiveData> objectives;
}

[System.Serializable]
public class ObjectiveData
{
    public string description;
    public ObjectiveType type;
    public string targetID;
    public int targetAmount;
}

public enum ObjectiveType
{
    TalkToNPC,
    ReachArea,
    CheckInventory,
    CheckCurrency,
}