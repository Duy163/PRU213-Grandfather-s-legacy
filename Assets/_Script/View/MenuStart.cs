using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void NewGame()
    {
        DataManager.Instance.CreateNewGame();
        SceneManager.LoadScene("MainScene");
    }
}
