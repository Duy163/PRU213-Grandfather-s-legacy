using UnityEngine;

public class Billboard : BasePanel
{
    public Transform target;
    public float heightOffset = 2f;

    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void LateUpdate()
    {
        Vector3 worldPos = target.position + Vector3.up * heightOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        bool isVisible = screenPos.z > 0;
        _canvasGroup.alpha = isVisible ? 1f : 0f;
        _canvasGroup.blocksRaycasts = isVisible;

        if (isVisible)
            transform.position = screenPos;
    }
}