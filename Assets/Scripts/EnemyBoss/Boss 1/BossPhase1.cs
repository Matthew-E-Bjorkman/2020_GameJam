﻿using System;
using System.Collections;
using System.Collections.Generic;
using EnemyBoss;
using UnityEngine;

public class BossPhase1 : MonoBehaviour
{
    /// <summary>
    /// Boss fight starts with player entering the fighting arena.
    /// Boss is introduced through a slight animation highlighting their main attack
    /// Player starts damaging the boss
    /// A boss has several "in-battle phases" consisting of an attack pattern
    /// Boss repeats current attack phase until damage threshold is reached
    /// If boss is defeated, an outro is played back, boss takes no more damage, and disappears from viewport
    /// </summary>
    
    [SerializeField] private int health;

    public AttackPhaseSystem<BossPhase1> PhaseSystem { get; set; }
    private AttackPhase<BossPhase1> currentPhase;
    private GameObject player;
    private bool changeState = false;
    private bool firstUpdate = true;
    private bool playMusic = true;
    private List<AttackPhase<BossPhase1>> remainingAttacks = new List<AttackPhase<BossPhase1>>();
    private List<AttackPhase<BossPhase1>> allAttacks = new List<AttackPhase<BossPhase1>>();
    [SerializeField] private GameObject weakPoint_Horn;
    [SerializeField] private GameObject weakPoint_Heart;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private AudioSource laserSFX;
    [SerializeField] private AudioSource heartSFX;
    [SerializeField] private AudioSource restSFX;
    private BossDeath deathScript;
    private CameraController camScript;

    void Start()
    {
        PhaseSystem = new AttackPhaseSystem<BossPhase1>(this);
        player = GameObject.Find("Player");
        allAttacks.Add(HeartAttack.Instance);
        allAttacks.Add(Rest.Instance);
        allAttacks.Add(LaserAttack.Instance);
        deathScript = GameObject.Find("BossDeath").GetComponent<BossDeath>();
        camScript = GameObject.Find("Camera").GetComponent<CameraController>();
    }
    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            Debug.Log("Intro");
            currentPhase = PhaseSystem.ChangeState(BossIntro.Instance);
        }

        if (changeState)
        {
            //Alternates phases between AddVulnerabilty and other 3
            if (currentPhase == AddVulnerability.Instance || currentPhase == BossIntro.Instance)
            {
                if (remainingAttacks.Count > 0)
                {
                    int nextState = UnityEngine.Random.Range(0, remainingAttacks.Count);
                    currentPhase = PhaseSystem.ChangeState(remainingAttacks[nextState]);
                    if (currentPhase.Equals(LaserAttack.Instance))
                    {
                        laserSFX.Play();
                    }
                    else if (currentPhase.Equals(HeartAttack.Instance))
                    {
                        heartSFX.Play();
                    }
                    else if (currentPhase.Equals(Rest.Instance))
                    {
                        restSFX.Play();
                    }
                    remainingAttacks.RemoveAt(nextState);
                    changeState = false;
                }
                else
                {
                    foreach (AttackPhase<BossPhase1> attack in allAttacks)
                    {
                        remainingAttacks.Add(attack);
                    }
                }
            }
            else
            {
                Debug.Log("Vuln");
                currentPhase = PhaseSystem.ChangeState(AddVulnerability.Instance);
            }
            changeState = false;
        }

        PhaseSystem.Update();
    }

    public void TakeDamage(int value)
    {
        StartCoroutine(HitGraphic());
        if (health > 0)
        {
            health -= value;
        }
        else
        {
            TriggerDeath();
        }
    }

    private IEnumerator HitGraphic()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 105, 105, 255);
        yield return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void TriggerDeath()
    {
        camScript.StopMusic();
        deathScript.playDeath();
        Destroy(this.gameObject);
    }

    public void ChangeState()
    {
        changeState = true;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject[] GetWeakpoints()
    {
        GameObject[] weakPoints = new GameObject[2];
        weakPoints[0] = weakPoint_Horn;
        weakPoints[1] = weakPoint_Heart;
        return weakPoints;
    }

    public GameObject GetHeart()
    {
        return heartPrefab;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            TakeDamage(1);
        }
    }
}