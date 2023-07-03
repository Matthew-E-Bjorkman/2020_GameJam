using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    const float HORIZONTAL_RATIO = 9f;
    const float TRANSITION_OFFSET = 1.7f;

    public float moveSpeed;
    public float transitionSpeed;
    public float zoomSpeed;
    public float yTransitionSpeed;
    public List<CameraMarker> ScrollMarkers = new List<CameraMarker>();
    public PostProcessVolume PostProcessVolume;

    private int scrollMarkerTargetIndex = 0;
    private CameraMarker lastMarker;
    private bool scrolling;
    private bool transitioningCamera;
    private bool zoomingCamera;
    private bool bossTransition;
    private bool inBoss;
    private bool playMusic = true;
    private GameObject rightWall;
    private GameObject rightWallBig;
    private GameObject boss;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource worldMusic;
    [SerializeField] private AudioSource preloop;
    [SerializeField] private AudioSource loop;
    private PlayerController playerController;
    private Vector3 goalPos;
    private Vector3 bossPos;
    private float camSize;
    private float postProcStep = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //**Change if scene order is moved
        rightWall = this.transform.GetChild(0).GetChild(1).gameObject;
        rightWallBig = this.transform.GetChild(1).GetChild(1).gameObject;
        playerController = player.GetComponent<PlayerController>();
        transitioningCamera = false;
        bossTransition = false;
        inBoss = false;
        playerController.PlayerDied.AddListener(OnPlayerDied);
        bossPos = new Vector3(75.66f, 5.12f, 0f);

        //Set camera to marker 0 (Start of cycle)
        if (ScrollMarkers.Count >= 2)
        {
            var startPosition = ScrollMarkers[scrollMarkerTargetIndex].position;
            lastMarker = ScrollMarkers[scrollMarkerTargetIndex];
            var actualPos = new Vector3(startPosition.x, startPosition.y, -10);
            transform.position = actualPos;
        }
        else
        {
            Debug.LogError("There are less than 2 scroll markers! Make sure you have at least 2!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //apply velocity
        if (scrolling)
        {
            if (ScrollMarkers.Count >= 2)
            {
                //TODO: Look at this once my mental facilities have rejoined me
                if (scrollMarkerTargetIndex != ScrollMarkers.Count)
                {
                    var currentTarget = ScrollMarkers[scrollMarkerTargetIndex].position;
                    currentTarget = new Vector3(currentTarget.x, currentTarget.y, -10);
                    transform.position =
                        Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
                    if (Math.Abs(Vector2.Distance(transform.position, currentTarget)) < 0.0001f)
                    {
                        lastMarker = ScrollMarkers[scrollMarkerTargetIndex];
                        playerController.currentCheckpoint = ScrollMarkers[scrollMarkerTargetIndex].associatedPlayerCP;
                        scrollMarkerTargetIndex++;
                    }
                }
                else if (scrollMarkerTargetIndex >= ScrollMarkers.Count)
                {
                    StartTransition(true);
                }
                //Vector3 newPos = new Vector3(this.transform.position.x + (moveSpeed * Time.deltaTime), 0, -10);
                //this.transform.position = newPos;
                // if (this.transform.position.x > 40f)
                // {
                //     StartTransition();
                // }
            }
        }

        if (transitioningCamera)
        {
            //Activate PostProcessing effects during boss
            PostProcessVolume.weight = Mathf.Lerp(0, 1, postProcStep);
            postProcStep += 0.5f * Time.deltaTime;
            
            if (this.transform.position.x >= goalPos.x)
            {
                StopTransition(bossTransition);
            }
            else
            {
                Vector3 newPos = new Vector3(this.transform.position.x + (transitionSpeed * Time.deltaTime), 0, -10);
                this.transform.position = newPos;
            }
        }

        if (zoomingCamera)
        {
            bool zoomDone = false, xDone = false, yDone = false;
            if (Math.Abs(Camera.main.orthographicSize - camSize) > .05f)
            {
                if (camSize > Camera.main.orthographicSize)
                {
                    Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
                }
                else
                {
                    Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
                }

            }
            else
            {
                zoomDone = true;
            }

            if (Math.Abs(this.transform.position.x - goalPos.x) > .05f)
            {
                Vector3 newPos;
                if (goalPos.x > this.transform.position.x)
                {
                    newPos = new Vector3(this.transform.position.x + (transitionSpeed * Time.deltaTime * 2.2f), this.transform.position.y, -10);
                }
                else
                {
                    newPos = new Vector3(this.transform.position.x - (transitionSpeed * Time.deltaTime * 2.2f), this.transform.position.y, -10);
                }
                this.transform.position = newPos;
            }
            else
            {
                xDone = true;
            }

            if (Math.Abs(this.transform.position.y - goalPos.y) > .05f)
            {
                Vector3 newPos;
                if (goalPos.y > this.transform.position.y)
                {
                    newPos = new Vector3(this.transform.position.x, this.transform.position.y + (yTransitionSpeed * Time.deltaTime * 2), -10);
                }
                else
                {
                    newPos = new Vector3(this.transform.position.x, this.transform.position.y - (yTransitionSpeed * Time.deltaTime * 2), -10);
                }
                this.transform.position = newPos;
            }
            else
            {
                yDone = true;
            }

            if (zoomDone && xDone && yDone)
            {
                zoomingCamera = false;
                StopBossTransition();
            }
        }
        if (playMusic)
        {
            if (inBoss)
            {
                if (!preloop.isPlaying && !loop.isPlaying)
                {
                    loop.Play();
                }
            }
            else
            {
                if (!worldMusic.isPlaying)
                {
                    worldMusic.Play();
                }
            }
        }
    }

    public void OnPlayerDied()
    {
        //Kill boss if live
        if (boss != null)
        {
            Destroy(boss);
        }

        //set currentmarkerindex
        var curMarkerIndex = ScrollMarkers.IndexOf(lastMarker.OnDeathMarker);
        transform.position = ScrollMarkers[curMarkerIndex].OnDeathMarker.position;
        //
        // scrollMarkerTargetIndex = curMarkerIndex+1;
        if (scrolling) StopScroll();

        //Return camera to ondeathmarker position
        ZoomCamera(false);

        //Disabling post processing effects (outside of arena)
        PostProcessVolume.weight = 0;
    }

    public void StartScroll ()
    {
        scrolling = true;
    }

    public void StopScroll()
    {
        scrolling = false;
    }

    public void StartTransition(bool bossTransition)
    {
        StopScroll();
        rightWall.tag = (bossTransition ? "bossTransition" : "transition");
    }

    public void TransitionTriggered(bool bossTransition)
    {
        this.bossTransition = bossTransition;
        playerController.canMove = false;
        transitioningCamera = true;
        float newX = this.transform.position.x + (HORIZONTAL_RATIO * TRANSITION_OFFSET);
        goalPos = new Vector3(newX, 0, -10);
    }

    public void StopTransition(bool isBossScreen)
    {
        worldMusic.Stop();
        rightWall.tag = "wallHazard";
        transitioningCamera = false;
        if (isBossScreen)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            boss = Instantiate(bossPrefab, bossPos, Quaternion.identity);
            ZoomCamera(true);
        }
        else
        {
            playerController.canMove = true;
            StartScroll();
        }
    }

    public void ZoomCamera(bool zoomingOut)
    {
        float goalX, goalY;
        if (zoomingOut)
        {
            worldMusic.Stop();
            preloop.Play();
            inBoss = true;
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            camSize = 10;
            goalY = 5;
            goalX = transform.position.x + 9f;
        }
        else
        {
            inBoss = false;
            loop.Stop();
            preloop.Stop();
            worldMusic.Play();
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
            camSize = 5;
            goalY = 0;
            var curMarkerIndex = ScrollMarkers.IndexOf(lastMarker.OnDeathMarker);
            goalX = ScrollMarkers[curMarkerIndex].OnDeathMarker.position.x;
            if (curMarkerIndex < ScrollMarkers.Count - 1)
                rightWall.tag = "wallHazard";
            else StartTransition(true);
        }
        goalPos = new Vector3(goalX, goalY, -10);

        zoomingCamera = true;
    }

    public void StopBossTransition()
    {
        zoomingCamera = false;
        playerController.canMove = true;
    }

    public void StopMusic()
    {
        playMusic = false;
        worldMusic.Stop();
        loop.Stop();
        preloop.Stop();
    }
}
