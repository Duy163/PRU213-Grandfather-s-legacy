using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    [SerializeField] private GameObject root; // chính GameObject chứa panel

    public bool IsVisible => root.activeSelf;

    public virtual void Show()
    {
        root.SetActive(true);
        Debug.Log($"[UI] Show: {gameObject.name}");
    }

    public virtual void Hide()
    {
        root.SetActive(false);
        Debug.Log($"[UI] Hide: {gameObject.name}");
    }

    public void Toggle()
    {
        if (IsVisible) Hide();
        else Show();
    }
}
