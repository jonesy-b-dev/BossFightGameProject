using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bossMovementSpeed = 5;
    [SerializeField] private bool chaseActivated = true;
    [SerializeField] private int bossHP = 1000;
    [SerializeField] private GameObject projectileRainProjectile;

    private bool inAttckstage = false;
    private bool canResetBodySlam = false;

    // Update is called once per frame
    void Update()
    {
        if (chaseActivated)
        {
            //Chasing state
            rb.velocity = new Vector2(1 * bossMovementSpeed, 0);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndOfChaseTrigger")
        {
            Debug.Log("chase deactivated");
            chaseActivated = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Debug.Log("hitground");
            Debug.Log(transform.position.y);
            
            rb.gravityScale = 0;
            moveBossUp();
        }
    }

    private void moveBossUp()
    {

        if (transform.position.y < 5)
        {
            rb.velocity = new Vector2(0, 5);
            moveBossUp();
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    IEnumerator StartAttackPhase()
    {
        //seems like 2 and 4 is barely choosen so that will be more powerfull attacks
        while (bossHP >= 0)
        {
            int nextAttack = Random.Range(0, 5);
            nextAttack = 2;
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
                        ProjectileRainAttack();
                    }
                    break;
                default:
                    break;
            }
                
            yield return new WaitForSeconds(Random.Range(4, 8));
        }
    }


    //
    private void SweepAttack()
    {
        Debug.Log("1");
    }

    private void BodySlam()
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        rb.gravityScale = 1.5f;

        canResetBodySlam = true;


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