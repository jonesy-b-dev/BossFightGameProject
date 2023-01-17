using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    //Source
    [SerializeField] AudioSource audioSource;

    //Audio files
    [SerializeField] AudioClip rangedOne;
    [SerializeField] AudioClip rangedTwo;
    [SerializeField] AudioClip melee;
    [SerializeField] AudioClip damage;


    private int random;

    public void Ranged()
    {
        random = Random.Range(1, 2);
        if (random == 1)
        {
            audioSource.clip = rangedOne;
            audioSource.Play();
        }
    }

    public void Melee()
    {
        audioSource.volume = 0.6f;
        audioSource.clip = melee;
        audioSource.Play();
        audioSource.volume = 1;
    }

    public void Damage()
    {
        audioSource.volume = 0.6f;
        audioSource.clip = damage;
        audioSource.Play();
        audioSource.volume = 1;
    }
}
