using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraDebug : MonoBehaviour
{
    private CameraController script;
    // Start is called before the first frame update
    void Start()
    {
        script = this.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("World_Phase1");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("World_Phase2");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("World_Phase3");
        }
    }
}
