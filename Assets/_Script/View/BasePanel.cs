using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    [SerializeField] private GameObject root; // chính GameObject chứa panel

    public bool IsVisible => root.activeSelf;

    public virtual void Show()
    {
        root.SetActive(true);
    }

    public virtual void Hide()
    {
        root.SetActive(false);
    }

    public void Toggle()
    {
        if (IsVisible) Hide();
        else Show();
    }
}
