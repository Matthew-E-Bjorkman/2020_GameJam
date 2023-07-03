using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float flipTimer;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        flipTimer = 0;
    }

    void Update()
    {
        flipTimer += Time.deltaTime;
        if (flipTimer > .1f)
        {
            sprite.flipX = !sprite.flipX;
            flipTimer = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
    }
}
