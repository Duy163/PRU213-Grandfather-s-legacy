using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private InteractView interactView;

    private IInteractable currentInteractable;

    void Start()
    {
        InputEvent.OnInteractPressed += OnInteractivePress;
    }

    void Update()
    {
        CheckForInteractable();
    }

    void OnInteractivePress()
    {
        if (currentInteractable != null)
        {
            if (currentInteractable.CanInteract())
            {
                currentInteractable.Interact(gameObject);
            }
        }
    }

    void CheckForInteractable()
    {
        // Tìm object gần nhất có thể tương tác
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

        float closestDistance = float.MaxValue;
        IInteractable closest = null;

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = interactable;
                }
            }
        }

        if (closest != currentInteractable)
        {
            currentInteractable = closest;
            if (currentInteractable != null)
                interactView.SetText(currentInteractable.GetInteractPrompt());
            else
                interactView.SetText("");
        }
    }
}
