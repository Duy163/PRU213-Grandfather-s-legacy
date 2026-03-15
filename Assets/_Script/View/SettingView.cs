using UnityEngine;
using UnityEngine.UI;

public class SettingView : BasePanel
{
    [SerializeField] private SettingManager settingManager;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    public void SetVolumeMusic()
    {
        settingManager.ChangeMusic(sliderMusic.value);
    }
    public void SetVolumeSFX()
    {
        settingManager.ChangeSFX(sliderSFX.value);
    }

    public void Load(float music, float sfx)
    {
        sliderMusic.value = music;
        sliderSFX.value = sfx;
    }
    public void QuitButton()
    {
        AudioManager.Instance.PlayMusic("menu");
        settingManager.ReturnMenu();
    }
}
