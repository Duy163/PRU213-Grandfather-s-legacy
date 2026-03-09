using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : Singleton<TriggerManager>
{
    [Header("UI")]
    [SerializeField] private EndingView endingView;
    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowEnding()
    {
        endingView.Show();
        InputManager.Instance.EnableEnding();
    }
}
