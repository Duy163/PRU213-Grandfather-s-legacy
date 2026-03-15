using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager.Instance.PlayMusic("background");

        SceneManager.LoadScene("MainScene");
    }

    public void NewGame()
    {
        AudioManager.Instance.PlayMusic("background");

        DataManager.Instance.CreateNewGame();
        SceneManager.LoadScene("MainScene");
    }
}
