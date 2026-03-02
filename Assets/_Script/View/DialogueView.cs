using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : BasePanel
{
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private GameObject continuePrompt; // "Nhấn Space để tiếp"

    public void UpdateDialogue(DialogueLine line)
    {
        speakerText.text = line.speakerName;
        dialogueText.text = line.text;
        // Cập nhật portrait nếu cần
        if (portraitImage != null)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.enabled = line.portrait != null;
        }
    }

    public void ShowContinuePrompt(bool show)
    {
        continuePrompt.SetActive(show);
    }
}
