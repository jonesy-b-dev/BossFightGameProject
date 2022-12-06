using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using System.Threading.Tasks;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private int bossMovementSpeed = 5;
    [SerializeField] private bool chaseActivated = true;
    [SerializeField] private int bossHP = 1000;
    [SerializeField] private GameObject projectileRainProjectile;

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

        bool canDoNextAttack = true;
    IEnumerator StartAttackPhase()
    {
        //seems like 2 and 4 is barely choosen so that will be more powerfull attacks

        while (bossHP >= 0)
        {
            if (canDoNextAttack)
            {
                int nextAttack = Random.Range(0, 5);
                nextAttack = 4;
                canDoNextAttack = false;
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
                        for (int i = 0; i < 50; i++)
                        {
                            Invoke("ProjectileRainAttack", 1f);
                        }                       
                        break;
                    default:
                        break;
                }
            } 
            yield return new WaitForSeconds(Random.Range(0, 4));
        }
    }


    //
    private void SweepAttack()
    {
        Debug.Log("1");
    }

    private void BodySlam()
    {
        Debug.Log("2");
    }

    private void ProjectileAttack()
    {
        Debug.Log("3");
    }

    private void ProjectileRainAttack()
    {


        Instantiate(projectileRainProjectile);
       
        Debug.Log("4");

    }
}   