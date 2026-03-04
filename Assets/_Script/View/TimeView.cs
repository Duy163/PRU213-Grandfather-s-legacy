using TMPro;
using UnityEngine;

public class TimeView : BasePanel
{
    [SerializeField] private TextMeshProUGUI clock;

    public void UpdateClockText(float time)
    {
        time = Mathf.Repeat(time, 24f);

        int totalMinutes = Mathf.FloorToInt(time * 60f);
        int hour = totalMinutes / 60;
        int minute = totalMinutes % 60;

        clock.text = $"{hour:00}:{minute:00}";
    }
}
