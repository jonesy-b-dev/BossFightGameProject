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
    [SerializeField] AudioClip[] wallSlide;


    private int random;
    private int previous = 0;

    public void Ranged()
    {
        random = Random.Range(1, 3);

        if (random == 1)
        {
            audioSource.volume = 1;
            audioSource.loop = false;
            audioSource.clip = rangedOne;
            audioSource.Play();
        }
        else
        {
            audioSource.volume = 1;
            audioSource.loop = false;
            audioSource.clip = rangedTwo;
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
        random = Random.Range(0, 3);
        if (random != previous)
        {
            previous = random;
            switch (random)
            {
                case 0:
                    audioSource.clip = wallSlide[0];
                    break;
                case 1:
                    audioSource.clip = wallSlide[1];
                    break;
                case 2:
                    audioSource.clip = wallSlide[2];
                    break;
                default:
                    break;
            }
        }
        else
        {
            WallSlide();
        }
        
        audioSource.volume = 0.6f;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void StopWallSlide()
    {
        if (audioSource.clip == wallSlide[0] || audioSource.clip == wallSlide[1] || audioSource.clip == wallSlide[2])
        {
            audioSource.Stop();
        }
    }
}
