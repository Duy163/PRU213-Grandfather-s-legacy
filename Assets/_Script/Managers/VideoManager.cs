using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("------------------------------------")]
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Camera camera;

    [Header("------------------------------------")]
    [SerializeField] VideoClip intro;

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
        videoPlayer.gameObject.SetActive(true);


        StartCoroutine(PlayAfterEnable());
    }

    System.Collections.IEnumerator PlayAfterEnable()
    {
        yield return new WaitForSeconds(1f); // Chờ 1 frame

        Debug.Log("VideoPlayer enabled: " + videoPlayer.enabled);
        Debug.Log("VideoPlayer GO active: " + videoPlayer.gameObject.activeSelf);

        videoPlayer.clip = intro;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }


    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
    }
}
