using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("------------------------------------")]
    [SerializeField] VideoPlayer videoPlayer;

    [Header("------------------------------------")]
    [SerializeField] VideoClip intro;


    [SerializeField] GameObject worldPanel;
    [SerializeField] GameObject playerPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject skipButton;

    void Start()
    {

        Invoke(nameof(CheckAndPlayIntro), 0.1f);

    }

    void CheckAndPlayIntro()
    {
        if (DataManager.Instance.currentGameData.loginCount == 0)
        {
            Debug.Log("Play intro");
            PlayIntro();
        }
    }

    public void PlayIntro()
    {
        worldPanel.SetActive(false);
        playerPanel.SetActive(false);
        mainPanel.SetActive(false);
        skipButton.SetActive(true);
        videoPlayer.gameObject.SetActive(true);
        InputManager.Instance.EnableEnding();
        AudioManager.Instance.PauseMusic();

        StartCoroutine(PlayAfterEnable());
    }

    System.Collections.IEnumerator PlayAfterEnable()
    {
        yield return new WaitForSeconds(0.1f); // Chờ 1 frame

        videoPlayer.clip = intro;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }


    void OnVideoEnd(VideoPlayer vp)
    {
        worldPanel.SetActive(true);
        playerPanel.SetActive(true);
        mainPanel.SetActive(true);
        skipButton.SetActive(false);
        InputManager.Instance.EnableShip();
        AudioManager.Instance.ContinueMusic();
        videoPlayer.gameObject.SetActive(false);
        StoryEvent.OnBeginDialogue?.Invoke();
    }

    public void SkipVideo()
    {
        worldPanel.SetActive(true);
        playerPanel.SetActive(true);
        mainPanel.SetActive(true);
        skipButton.SetActive(false);
        InputManager.Instance.EnableShip();
        AudioManager.Instance.ContinueMusic();
        videoPlayer.gameObject.SetActive(false);
        StoryEvent.OnBeginDialogue?.Invoke();
    }
}
