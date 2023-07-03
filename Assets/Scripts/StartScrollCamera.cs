using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScrollCamera : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            if (cameraController != null)
            {
                cameraController.StartScroll();
            }
        }
    }
}
