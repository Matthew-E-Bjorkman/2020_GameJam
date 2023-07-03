using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //loads inputted level
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
        Time.timeScale = 1;
    }

    //loads inputted level additively
    public void LoadLevelAdd(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
    }

    //unloads addititve scene
    public void UnloadAddLevel(string level)
    {
        SceneManager.UnloadSceneAsync(level);
    }

    //Quit the Game
    public void QuitGame()
    {
        Application.Quit();
    }
}
