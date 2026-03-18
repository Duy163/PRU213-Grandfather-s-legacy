using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("------------------------------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource seaSource;
    [SerializeField] private AudioSource shipMoveSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource crashAudioSource;

    [Header("------------------------------------")]
    public AudioClip menu;
    public AudioClip seaWave;
    public AudioClip shipMove;
    public AudioClip catchSuccess;
    public AudioClip catchFail;
    public AudioClip woodenThud;

    public List<AudioClip> listVillageSound;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartMenu")
        {
            PlayMusic("menu");
        }

        if (scene.name == "MainScene")
        {
            PlayMusic("background");
        }

    }

    public void PlayMusic(string nameMusic)
    {
        switch (nameMusic)
        {
            case "background":

                musicSource.clip = seaWave;
                break;
            case "menu":

                musicSource.clip = menu;
                break;
        }
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ContinueMusic()
    {
        musicSource.Play();
    }


    public void ChangeVolumeMusic(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeVolumeSFX(float value)
    {
        SFXSource.volume = value;
    }

    public float GetVolumeMusic()
    {
        return musicSource.volume;
    }

    public float GetVolumeSFX()
    {
        return SFXSource.volume;
    }

    public void PlayShipMove(bool isPlay)
    {
        if (isPlay)
        {
            shipMoveSource.clip = shipMove;
            shipMoveSource.Play();
        }
        else
        {
            shipMoveSource.Stop();
        }

    }

    public void UpdateShipEnginePitch(float speedPercent)
    {
        if (shipMoveSource != null)
        {
            // Thiết lập dải pitch: 
            // 0.8f là tiếng rầm rì khi chạy chậm
            // 1.5f là tiếng rú gắt khi chạy max tốc độ
            float minPitch = 0.8f;
            float maxPitch = 1.5f;

            // Dùng Mathf.Lerp để chuyển đổi phần trăm tốc độ (0 -> 1) thành mức pitch tương ứng
            shipMoveSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);
        }
    }
    //-------------------SFX--------------------------

    public void PlaySoundVillage()
    {
        AudioClip temp = listVillageSound[Random.Range(0, listVillageSound.Count)];
        SFXSource.PlayOneShot(temp);
    }

    public void PlaySoundFishing(bool isSuccess)
    {
        AudioClip temp;
        if (isSuccess) temp = catchSuccess;
        else temp = catchFail;

        SFXSource.PlayOneShot(temp);
    }

    public void PlaySoundDialogue(AudioClip clip)
    {
        SFXSource.Stop();
        SFXSource.PlayOneShot(clip);
    }

    public void PlayShipCrash(float force = 5f)
    {
        if (crashAudioSource != null)
        {
            // Quy đổi lực tông (ví dụ từ 2 đến 10) thành âm lượng (0.2 đến 1.0)
            float calculatedVolume = Mathf.Clamp(force / 10f, 0.2f, 1f);

            // Phát âm thanh va chạm 1 lần (PlayOneShot giúp các tiếng tông không bị đứt quãng nếu đụng liên tục)
            crashAudioSource.PlayOneShot(woodenThud, calculatedVolume);
        }
    }
}
