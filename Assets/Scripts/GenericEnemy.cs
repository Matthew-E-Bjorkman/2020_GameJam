using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GenericEnemy : MonoBehaviour
{
    public uint Health { get; set; }
    public bool IsAlive;
    public GameObject BloodSpatter;
    public ParticleSystem ParticleSystem;
    public SpriteRenderer spriteRenderer;
    
    
    protected Vector3? initPosition = null;
    protected uint currentHealth;
    
    
    public void Reset()
    {
        //Reset position
        //Reset health
        //Enable renderer
        //Enable behaviour
        currentHealth = Health;
        if (initPosition != null)
            transform.position = (Vector3)initPosition;

        IsAlive = true;
    }

    public void Kill()
    {
        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            IsAlive = false;
            if (ParticleSystem != null)
                ParticleSystem.Play(true);
            Instantiate(BloodSpatter, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(uint damage)
    {
        
    }

    protected void Initialize()
    {
        if (initPosition == null)
            initPosition = transform.position;
        
        currentHealth = Health;
        IsAlive = true;

        ParticleSystem = gameObject.GetComponent<ParticleSystem>();
        //Debug.Log("Enemy initiated at" + initPosition);
    }
}
