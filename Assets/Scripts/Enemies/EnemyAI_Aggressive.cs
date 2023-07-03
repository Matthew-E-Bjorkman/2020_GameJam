using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Aggressive : GenericEnemy
{
    public float moveSpeed;

    private Rigidbody2D rb2d;
    private Collider2D collider;
    [SerializeField] private GameObject bulletPrefab;
    private GameObject bulletParent;
    private GameObject player;
    private bool movingRight;
    private bool aggrod;
    private float timeSinceLastShot;
    private int faceDirection; //-1: Left, 1: Right
    private Animator animator;
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioSource shootSFX;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
        bulletParent = GameObject.Find("Bullets");
        rb2d.velocity = new Vector2(moveSpeed, 0);
        movingRight = true;
        aggrod = false;
        faceDirection = 1;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!aggrod)
        {
            if (movingRight)
            {
                rb2d.velocity = new Vector2(moveSpeed, 0);
                spriteRenderer.flipX = false;
                faceDirection = 1;
            }
            else
            {
                rb2d.velocity = new Vector2(-moveSpeed, 0);
                spriteRenderer.flipX = true;
                faceDirection = -1;
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > 0.75f)
            {
                FireBullet();
                timeSinceLastShot = 0f;
            }
        }

        if (rb2d.velocity.x != 0)
        {
            animator.SetBool("isMoving", true);
            if (!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            walkSFX.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "hazard" || other.gameObject.tag == "transition")
        {
            movingRight = !movingRight;
        }
        else if (other.gameObject.tag == "wallHazard" || other.gameObject.tag == "Untagged")
        {
            Physics2D.IgnoreCollision(collider, other.collider);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemyWall")
        {
            movingRight = !movingRight;
        }
    }

    void FireBullet()
    {
        shootSFX.Play();
        float spawnOffset = .7f;
        Vector3 spawnPos = new Vector3();
        if (faceDirection == 1)
        {
            spawnPos = new Vector3(rb2d.position.x + spawnOffset, rb2d.position.y, 0);
        }
        else if (faceDirection == -1)
        {
            spawnPos = new Vector3(rb2d.position.x - spawnOffset, rb2d.position.y, 0);
        }

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity, bulletParent.transform);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        bulletScript.bulletSpeed = 10;
        bulletScript.bulletDirection = faceDirection;
    }

    public void Aggro()
    {
        aggrod = true;
        if (player.transform.position.x > this.transform.position.x)
        {
            faceDirection = 1;
        }
        else
        {
            faceDirection = -1;
        }
    }

    public void ResetAggro()
    {
        aggrod = false;
    }
}
