using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private UIManager uiManager;
    [SerializeField] private string nextLevel;
    void Start()
    {
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //When wall+player collision detected, go to next level
        if (other.gameObject.tag == "player")
        {
            uiManager.LoadLevel(nextLevel);
        }
    }
}
