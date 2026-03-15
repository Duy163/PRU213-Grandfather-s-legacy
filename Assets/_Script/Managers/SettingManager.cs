using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private SettingView settingView;
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

    public void ToggleSettingPressed()
    {
        settingView.Toggle();
    }

    public void ChangeMusic(float value)
    {
        AudioManager.Instance.ChangeVolumeMusic(value);
    }

    public void ChangeSFX(float value)
    {
        AudioManager.Instance.ChangeVolumeSFX(value);
    }

    public void ReturnMenu()
    {
        DataManager.Instance.Save();
        WorldDataManager.ResetSingleton();
        InventoryManager.ResetSingleton();
        TriggerManager.ResetSingleton();
        QuestManager.ResetSingleton();

        DataManager.ResetSingleton();

        SceneManager.LoadScene("StartMenu");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
