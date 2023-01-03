using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip projectileAttack;

    public void ProjectileAttack()
    {
        audioSource.clip = projectileAttack;
        audioSource.Play();
    }
}
