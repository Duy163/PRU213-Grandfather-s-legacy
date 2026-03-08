using UnityEngine;
using UnityEngine.UI;

public class SettingView : BasePanel
{
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    public void SetVolumeMusic()
    {
        SettingManager.Instance.ChangeMusic(sliderMusic.value);
    }
    public void SetVolumeSFX()
    {
        SettingManager.Instance.ChangeSFX(sliderSFX.value);
    }

    public void Load(float music, float sfx)
    {
        sliderMusic.value = music;
        sliderSFX.value = sfx;
    }
}
