using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private CameraController camScript;
    public EnemyManager EnemyManager;
    void Start()
    {
        camScript = this.transform.parent.transform.parent.GetComponent<CameraController>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (this.tag == "transition" && other.gameObject.tag == "player")
        {
            camScript.TransitionTriggered(false);
        }

        if (this.tag == "bossTransition" && other.gameObject.tag == "player")
        {
            camScript.TransitionTriggered(true);
            EnemyManager.RemoveAllEnemies();
            //Kill all bunnies
            
        }
    }
}
