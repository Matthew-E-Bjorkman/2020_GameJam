using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public float bulletSpeed;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4.75f);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(-bulletSpeed, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }

        if (other.gameObject.tag != "bullet")
        {
            Destroy(gameObject);
        }
    }
}
