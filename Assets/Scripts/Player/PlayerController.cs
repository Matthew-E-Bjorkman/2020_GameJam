using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;
    public float jumpSpeed;
    public bool canMove;
    public GameObject bulletPrefab;
    public GameObject bulletParent;
    public int currentCheckpoint = 0;
    public List<Transform> playerCheckpoints;
    public UnityEvent PlayerDied;
    public GameObject blood;
    
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;
    private bool grounded;
    private int faceDirection; //-1: left, 1:right
    private float moveHorizontal;
    private float timeSinceLastShot;
    private Animator animator;
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource landSFX;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private AudioSource deathSFX;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        sprite = this.GetComponent<SpriteRenderer>();
        grounded = true;
        canMove = true;
        faceDirection = 1;
        animator = GetComponent<Animator>();
        timeSinceLastShot = 0;
    }

    void FixedUpdate()
    {
        UpdateHorizontalInput();
        SetDirection();
        if (canMove) HorizontalMove();
        if (canMove && Math.Abs(rb2d.velocity.y) < 0.1f && grounded && Input.GetKey(KeyCode.Space)) Jump();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        //Check for shooting   
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (timeSinceLastShot > .15f)
            {
                FireBullet();
                timeSinceLastShot = 0;
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "ground" && Math.Abs(rb2d.velocity.y) < 0.1f)
        {
            landSFX.Play();
            grounded = true;

            animator.SetBool("Jumping", false);
        }
        
        if (other.gameObject.CompareTag("wallHazard") || other.gameObject.CompareTag("hazard") ||
            other.gameObject.CompareTag("semiAggro") || other.gameObject.CompareTag("aggressive") ||
            other.gameObject.CompareTag("boss"))
        {
            Die();
        }
    }

    public void Die()
    {
        deathSFX.Play();
        StartCoroutine(KillPlayer());
    }

    private IEnumerator KillPlayer()
    {
        animator.ResetTrigger("Respawn");
        animator.SetTrigger("DoDeath");
        yield return new WaitForSeconds(1f);
        //Spawn blood
        Instantiate(blood, transform.position, Quaternion.identity);
        animator.ResetTrigger("DoDeath");
        animator.SetTrigger("Respawn");
        PlayerDied?.Invoke();
        transform.position = playerCheckpoints[currentCheckpoint].position;
        grounded = true;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "hazard")
        {
            Die();
        }

        if (other.transform.parent != null)
        {
            if (other.transform.parent.gameObject.tag == "semiAggro")
            {
                other.transform.parent.gameObject.GetComponent<EnemyAI_SemiAggro>().Aggro();
            }
            else if (other.transform.parent.gameObject.tag == "aggressive")
            {
                other.transform.parent.gameObject.GetComponent<EnemyAI_Aggressive>().Aggro();
            }
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.tag == "aggressive")
        {
            other.transform.parent.gameObject.GetComponent<EnemyAI_Aggressive>().ResetAggro();
        }
    }

    void FireBullet ()
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
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.bulletSpeed = 10;
        bulletScript.bulletDirection = faceDirection;
    }

    void Jump()
    {
        jumpSFX.Play();
        Vector2 jumpMovement = new Vector2(0.0f, jumpSpeed);
        rb2d.AddForce(jumpMovement);
        grounded = false;

        animator.SetBool("Jumping", true);
    }

    void HorizontalMove()
    {
        //Apply horizontal movement
        rb2d.velocity = new Vector2(Mathf.Clamp(moveHorizontal*moveSpeed, -maxSpeed, maxSpeed), rb2d.velocity.y);

        if (moveHorizontal != 0)
        {
            animator.SetBool("Walking", true);
            if (!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
        }
        else
        {
            animator.SetBool("Walking", false);
            walkSFX.Stop();
        }
        //Vector2 movement = new Vector2(moveHorizontal, 0.0f);
        // rb2d.AddForce(movement * moveSpeed);
        // //Enforce max speed constraint
        // if (rb2d.velocity.x > maxSpeed)
        // {
        //     rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        // }
        // else if (rb2d.velocity.x < -maxSpeed)
        // {
        //     rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        // }
    }

    void SetDirection()
    {
        //Check which direction player is moving
        if (moveHorizontal > 0.0f) //Moving Right
        {
            faceDirection = 1;
            sprite.flipX = false;
        }
        else if (moveHorizontal < 0.0f) //Moving Left
        {
            faceDirection = -1;
            sprite.flipX = true;
        }
    }

    void UpdateHorizontalInput()
    {
        //Check Movement
        moveHorizontal = Input.GetAxisRaw("Horizontal");
    }
}
