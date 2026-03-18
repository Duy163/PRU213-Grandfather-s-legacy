using UnityEngine;

public class NPCInteracttive : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCData data;
    [SerializeField] private StoryDatabase database;

    public string GetInteractPrompt()
    {
        return "giữ [F] tương tác";
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        StoryEvent.OnStartDialogue?.Invoke(data, database);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetName()
    {
        return data.npcID;
    }
}
