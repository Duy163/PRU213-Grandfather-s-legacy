using UnityEngine;

public class NPCInteracttive : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCData data;
    [SerializeField] private StoryDatabase database;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

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
        TriggerAnimTalking();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetName()
    {
        return data.npcID;
    }

    public void TriggerAnimTalking()
    {
        anim.SetTrigger("TriggerTalking");
    }

    public void TriggerAnimCompleteQuest()
    {
        anim.SetTrigger("TriggerCompleteQuest");
    }
}
