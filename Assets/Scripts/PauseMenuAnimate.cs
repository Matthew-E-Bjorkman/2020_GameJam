using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnimate : MonoBehaviour
{
    private RectTransform rectTransform;

    private bool shouldOpen = false;
    private bool shouldClose = false;
    private bool animating;

    private float t;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (animating)
        {
            if (shouldOpen)
            {
                rectTransform.localScale = new Vector3(1f, Mathf.Lerp(0f, 1f, t), 1f);
            }
            else if (shouldClose)
            {
                rectTransform.localScale = new Vector3(1f, Mathf.Lerp(1f, 0f, t), 1f);
            }

            t += 10f * Time.unscaledDeltaTime;
            
            if (t > 2.0f)
            {
                shouldOpen = false;
                animating = false;
                t = 0f;
                if (shouldClose)
                {
                    shouldClose = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void Hide()
    {
        shouldClose = true;
        shouldOpen = false;
        animating = true;
    }

    public void Show()
    {
        shouldOpen = true;
        shouldClose = false;
        animating = true;
        gameObject.SetActive(true);
    }
}
