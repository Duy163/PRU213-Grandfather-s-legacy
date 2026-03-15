using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("------------------------------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("------------------------------------")]
    public AudioClip menu;
    public AudioClip background;
    public AudioClip catchSuccess;
    public AudioClip catchFail;

    public List<AudioClip> listVillageSound;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        PlayMusic("menu");
    }

    public void PlayMusic(string nameMusic)
    {
        switch (nameMusic)
        {
            case "background":
                musicSource.clip = background;
                break;
            case "menu":
                musicSource.clip = menu;
                break;
        }
        musicSource.Play();
    }

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
}
