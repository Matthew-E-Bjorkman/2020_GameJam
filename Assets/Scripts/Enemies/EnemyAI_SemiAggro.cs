using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_SemiAggro : GenericEnemy
{
    public float moveSpeed;
    public float attackSpeed;

    private Rigidbody2D rb2d;
    private Collider2D smallCollider;
    private PlayerController player;
    private bool movingRight;
    private bool aggrod;
    private Animator animator;
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioSource chargeSFX;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        smallCollider = this.GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rb2d.velocity = new Vector2(moveSpeed, 0);
        movingRight = true;
        aggrod = false;

        player.PlayerDied.AddListener(ResetAggro);
        base.Initialize();
    }

    private void OnEnable()
    {
        aggrod = false;
    }

    void Update()
    {
        if (!aggrod)
        {
            if (!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
            if (movingRight)
            {
                rb2d.velocity = new Vector2(moveSpeed, 0);
                spriteRenderer.flipX = false;
            }
            else
            {
                rb2d.velocity = new Vector2(-moveSpeed, 0);
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (!chargeSFX.isPlaying)
            {
                chargeSFX.Play();
            }
            if (this.transform.position.x > player.transform.position.x)
            {
                rb2d.velocity = new Vector2(-attackSpeed, 0);
                spriteRenderer.flipX = true;
            }
            else
            {
                rb2d.velocity = new Vector2(attackSpeed, 0);
                spriteRenderer.flipX = false;
            }
        }
        
        if (moveSpeed > 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("hazard") || other.gameObject.CompareTag("transition"))
        {
            movingRight = !movingRight;
        }
        else if (other.gameObject.CompareTag("wallHazard") || other.gameObject.CompareTag("Untagged"))
        {
            if (!aggrod) Physics2D.IgnoreCollision(smallCollider, other.collider);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemyWall"))
        {
            movingRight = !movingRight;
        }
    }

    public void Aggro()
    {
        moveSpeed = 1.4f;
        aggrod = true;
    }

    public void ResetAggro()
    {
        aggrod = false;
    }
}
