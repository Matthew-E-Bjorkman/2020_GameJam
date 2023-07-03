using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletDirection; //-1: left, 1:right

    private Rigidbody2D rb2d;
    private Collider2D collider;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<CircleCollider2D>();
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(bulletSpeed * bulletDirection, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "peaceful" || other.gameObject.tag == "semiAggro" || other.gameObject.tag == "aggressive")
        {
            GameObject.Find("EnemySounds").GetComponent<AudioSource>().Play();
            var enemy = other.collider.GetComponent<GenericEnemy>();
            enemy?.Kill();
            //Instead of destroying it, we set it to a "death" state
        }
        Destroy(gameObject);
    }

    //Might be laggy, if lag exists in the future come here
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "vulnerability") other.GetComponent<Vulnerability>().DealDamage();
    }
}
