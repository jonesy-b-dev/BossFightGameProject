using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAudio : MonoBehaviour
{
    //Source
    [SerializeField] AudioSource audioSource;

    //Audio files
    [SerializeField] AudioClip rangedOne;
    [SerializeField] AudioClip rangedTwo;
    [SerializeField] AudioClip melee;
    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip wallSlide;


    private int random;

    public void Ranged()
    {
        random = Random.Range(1, 2);
        if (random == 1)
        {
            audioSource.volume = 1;
            audioSource.loop = false;
            audioSource.clip = rangedOne;
            audioSource.Play();
        }
    }

    public void Melee()
    {
        audioSource.clip = melee;
        audioSource.loop = false;
        audioSource.volume = 0.6f;
        audioSource.Play();
    }

    public void Damage()
    {
        audioSource.clip = damage;
        audioSource.loop = false;
        audioSource.volume = 0.6f;
        audioSource.Play();
    }
    
    public void WallSlide()
    {
        audioSource.clip = wallSlide;
        audioSource.volume = 0.6f;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void StopWallSlide()
    {
        if (audioSource.clip == wallSlide)
        {
            audioSource.Stop();
        }
    }
}
