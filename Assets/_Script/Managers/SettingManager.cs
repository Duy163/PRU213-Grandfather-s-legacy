using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private SettingView settingView;

    protected override void Awake()
    {
        base.Awake();
    }
    void OnEnable()
    {
        InputEvent.OnOpenSettingPressed += OnOpenSettingPressed;
        InputEvent.OnCloseSettingPressed += OnCLoseSettingPressed;
    }

    void OnDisable()
    {
        InputEvent.OnOpenSettingPressed -= OnOpenSettingPressed;
        InputEvent.OnCloseSettingPressed -= OnCLoseSettingPressed;
    }

    void Start()
    {
        settingView.Load(AudioManager.Instance.GetVolumeMusic(), AudioManager.Instance.GetVolumeSFX());
    }

    private void OnOpenSettingPressed()
    {
        settingView.Show();
        InputManager.Instance.EnableSetting();
    }

    private void OnCLoseSettingPressed()
    {
        settingView.Hide();
        InputManager.Instance.EnableShip();
    }

    public void ChangeMusic(float value)
    {
        AudioManager.Instance.ChangeVolumeMusic(value);
    }

    public void ChangeSFX(float value)
    {
        AudioManager.Instance.ChangeVolumeSFX(value);
    }
}
