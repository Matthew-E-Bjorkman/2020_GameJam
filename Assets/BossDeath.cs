using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    [SerializeField] private AudioSource deathSFX;
    public void playDeath()
    {
        deathSFX.Play();
    }
}
