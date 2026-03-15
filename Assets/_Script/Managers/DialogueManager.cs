using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private QuestManager questManager;
    [Header("UI")]
    [SerializeField] private DialogueView dialogueView; // Tham chiếu đến DialogueView

    private DialogueData current;
    private int lineIndex;
    private bool isPlaying;

    void OnEnable()
    {
        InputEvent.OnNextDialoguePressed += OnNextLine;
    }

    void OnDisable()
    {
        InputEvent.OnNextDialoguePressed -= OnNextLine;
    }

    void OnNextLine()
    {
        if (!isPlaying) return;

        NextLine();
    }

    // ── BẮT ĐẦU DIALOGUE ──────────────────────────────
    public void StartDialogue(DialogueData data)
    {
        if (data == null || data.lines.Count == 0) return;

        current = data;
        lineIndex = 0;
        isPlaying = true;

        dialogueView.Show(); // Hiển thị panel dialogue

        ShowLine();
    }

    // ── HIỂN THỊ DÒNG HIỆN TẠI ────────────────────────
    private void ShowLine()
    {
        var line = current.lines[lineIndex];

        dialogueView.UpdateDialogue(line);

        bool hasNextLine = lineIndex < current.lines.Count - 1;
        dialogueView.ShowContinuePrompt(hasNextLine);
    }

    // ── SANG DÒNG TIẾP THEO ────────────────────────────
    public void NextLine()
    {
        // Xử lý hành động của dòng vừa xong
        ProcessLineActions(current.lines[lineIndex]);

        lineIndex++;

        if (lineIndex >= current.lines.Count)
            EndDialogue();
        else
            ShowLine();
    }

    // ── XỬ LÝ FLAG / QUEST SAU MỖI DÒNG ──────────────
    private void ProcessLineActions(DialogueLine line)
    {
        if (!string.IsNullOrEmpty(line.setFlagAfter))
        {
            DataManager.Instance.WorldState.SetFlag(line.setFlagAfter, true);
        }

        if (!string.IsNullOrEmpty(line.triggerQuestID))
            questManager.TryStartQuest(line.triggerQuestID);

        if (!string.IsNullOrEmpty(line.triggerID))
            TriggerManager.Instance.TryTrigger(line.triggerID);

    }

    // ── KẾT THÚC DIALOGUE ─────────────────────────────
    private void EndDialogue()
    {
        isPlaying = false;
        current = null;
        lineIndex = 0;

        dialogueView.Hide(); // Ẩn panel dialogue

        dialogueView.ShowContinuePrompt(false);
        InputManager.Instance.EnableShip();
        DataManager.Instance.Save();
    }
}