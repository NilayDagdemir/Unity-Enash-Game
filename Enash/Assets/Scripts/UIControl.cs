using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
    }

    public void ChangeScene(string sceneName)
    {
        // After hitting the "retry" button, the user can play the game
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
