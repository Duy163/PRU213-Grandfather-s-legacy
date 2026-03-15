using System;
using TMPro;
using UnityEngine;

public class QuestView : BasePanel
{
    [SerializeField] TextMeshProUGUI text;
    public void SetContent(String content)
    {
        text.text = content;
    }
}
