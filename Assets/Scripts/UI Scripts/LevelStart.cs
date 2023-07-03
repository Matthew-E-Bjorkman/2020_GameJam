using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    private CameraController camScript;
    private PlayerController player;
    private Canvas canvas;
    private bool started = false;
    void Start()
    {
        //Instapause game
        Time.timeScale = 0;

        camScript = GameObject.Find("Camera").GetComponent<CameraController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.PlayerDied.AddListener(RestartLevel);
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check for any user input
        if (Input.anyKey && !started)
        {
            //Start the level
            StartLevel();
        }
    }

    private void StartLevel()
    {
        //Unpause timescale
        Time.timeScale = 1;

        //Start camera scroll
        camScript.StartScroll();
        //Remove canvas renderer
        canvas.enabled = false;
        //Flag started
        started = true;
    }

    private void RestartLevel()
    {
        Time.timeScale = 0;
        canvas.enabled = true;
        started = false;
    }
}

