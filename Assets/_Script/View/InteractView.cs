using TMPro;
using UnityEngine;

public class InteractView : MonoBehaviour
{
    public void SetText(string content)
    {
        transform.GetComponent<TextMeshProUGUI>().text = content;
    }
}
