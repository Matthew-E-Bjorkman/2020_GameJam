using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HomingBall : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public GameObject player;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().PlayerDied.AddListener(DestroyOnPlayerDeath);
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        rb2d.velocity = transform.up * moveSpeed * Time.deltaTime;
        Vector3 playerVector = player.transform.position - transform.position;
        float rotatingIndex = Vector3.Cross(playerVector, transform.up).z;
        rb2d.angularVelocity = -1 * rotatingIndex * rotateSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
    }

    private void DestroyOnPlayerDeath()
    {
        Destroy(gameObject);
    }
}

