using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderManager : MonoBehaviour
{
    public static LevelLoaderManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene("Level " + sceneNum);
    }

    public void LoadSceneWithName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Level 001");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitApplication()
    {
        Debug.Log("Application has Quit");
        Application.Quit();
    }

}
