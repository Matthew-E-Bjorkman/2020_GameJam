using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEffect : MonoBehaviour
{
    public Material[] frames;

    public float framesPerSecond;
    public Skybox skybox;
    void Update()
    {
        int index = (int)(Time.time * framesPerSecond);
        index = index % frames.Length;
        skybox.material = frames[index];
    }
}
