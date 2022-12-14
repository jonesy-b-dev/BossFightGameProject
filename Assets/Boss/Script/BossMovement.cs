using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bossMovementSpeed = 5;
    public bool chaseActivated = true;
    [SerializeField] private int bossHP = 1000;

    //Attack assets
    [SerializeField] private GameObject projectileRainProjectile;
    [SerializeField] private GameObject projectileAttackProjectile;
    [SerializeField] private GameObject player;
    Vector2 playerPos;

    private bool inAttckstage = false;

    // Update is called once per frame
    void Update()
    {
        if (!chaseActivated)
        {
            //Attack state
            if (!inAttckstage)
            {
                inAttckstage = true;
                StartCoroutine(StartAttackPhase());

            }
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

    }

    IEnumerator StartAttackPhase()
    {
        //seems like 2 and 4 is barely choosen so that will be more powerfull attacks
        while (bossHP >= 0)
        {
            int nextAttack = Random.Range(0, 5);
            nextAttack = 3;
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

        Debug.Log("2");
    }

    private void ProjectileAttack()
    {
        Instantiate(projectileAttackProjectile, transform.position, Quaternion.identity);
    }


    private void ProjectileRainAttack()
    {
        Instantiate(projectileRainProjectile);

        Debug.Log("4");
    }
}   