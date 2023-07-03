using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyAI_Peaceful : GenericEnemy
{
    public float moveSpeed;

    private Rigidbody2D rb2d;
    private Collider2D collider;
    private bool movingRight;
    private Animator animator;
    [SerializeField] private AudioSource walkSFX;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();
        animator = this.GetComponent<Animator>();
        rb2d.velocity = new Vector2(moveSpeed, 0);
        movingRight = true;
        
        base.Initialize();
    }
    
    void Update()
    {
        if (movingRight)
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
        }

        if (moveSpeed > 0)
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

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "hazard" || other.gameObject.tag == "transition")
        {
            movingRight = !movingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        else if (other.gameObject.tag == "wallHazard" || other.gameObject.tag == "Untagged")
        {
            Physics2D.IgnoreCollision(collider, other.collider);
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "enemyWall")
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            movingRight = !movingRight;
        }
    }
}
