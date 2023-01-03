using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip rangedOne;
    [SerializeField] AudioClip rangedTwo;
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
}
