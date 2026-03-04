using System.Collections.Generic;
using UnityEngine;

public class WorldDataManager : Singleton<WorldDataManager>
{
    [SerializeField] List<FishingSpotInteractable> fishingSpots;

    [SerializeField] private TimeView timeView;

    public WorldData worldData;
    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        InputEvent.OnCloseDockPressed += EndDocking;
    }

    void OnDisable()
    {
        InputEvent.OnCloseDockPressed -= EndDocking;
    }

    void Start()
    {
        LoadWorldData();

    }

    public void StartDocking(string dockName)
    {
        timeView.Show();
        CameraManager.Instance.EnterIslandView(dockName);
        InputManager.Instance.EnableDock();
    }

    private void EndDocking()
    {
        timeView.Hide();
        CameraManager.Instance.NormalView();
        InputManager.Instance.EnableShip();
    }

    public void UpdateClock(float timeOfDay)
    {
        timeView.UpdateClockText(timeOfDay);
    }

    public void LoadSpotWithTimeOfDay(bool isDay)
    {
        foreach (var spot in fishingSpots)
        {
            if (spot.item.timeType == TimeType.Day)
            {
                spot.SetAvailable(isDay);
            }
            else if (spot.item.timeType == TimeType.Night)
            {
                spot.SetAvailable(!isDay);
            }
        }
        Debug.Log($"[WorldDataManager] Loaded fishing spots for {(isDay ? "day" : "night")} time.");
    }

    public void LoadWorldData()
    {
        worldData = DataManager.Instance.currentGameData.worldData;

    }
}
