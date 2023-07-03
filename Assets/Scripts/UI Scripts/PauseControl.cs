using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public PauseMenuAnimate pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseControl();
        }
    }

    //toggles the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenu.Show();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseMenu.Hide();
        }
    }
}
