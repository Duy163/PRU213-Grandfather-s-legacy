using UnityEngine;

public class NPCInteracttive : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCData data;
    [SerializeField] private StoryDatabase database;

    public string GetInteractPrompt()
    {
        return "Hold [F] Talk";
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
}
