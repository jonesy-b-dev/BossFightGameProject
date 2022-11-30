using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private int bossMovementSpeed = 5;
    [SerializeField] private bool chaseActivated = true;
    [SerializeField] private int bossHP = 1000;

    private bool inAttckstage = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chaseActivated)
        {
            //Chasing state
            transform.Translate(Vector2.right * bossMovementSpeed * Time.deltaTime);
        }
        else
        {
            //Attack state
            if (!inAttckstage)
            {
                inAttckstage = true;
                StartCoroutine(StartAttackPhase());
            }
        }
    }

    IEnumerator StartAttackPhase()
    {
        System.Random rand = new System.Random();

        while (bossHP >= 0)
        {
            int nextAttack = rand.Next(1, 5);
            switch (nextAttack)
            {
                case 1:
                    SweepAttack();
                    break;
                case 2:
                    BodySlam();
                    break;
                case 3:
                    ProjectileAttack();
                    break;
                case 4:
                    ProjectileRainAttack();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(2);
        }
    }

    private void ProjectileRainAttack()
    {
        Debug.Log("1");
    }

    private void ProjectileAttack()
    {
        Debug.Log("2");
    }

    private void BodySlam()
    {
        Debug.Log("3");
    }

    private void SweepAttack()
    {
        Debug.Log("4");
    }
}   
