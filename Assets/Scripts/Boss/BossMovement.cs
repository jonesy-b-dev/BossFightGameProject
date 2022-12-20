using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bossMovementSpeed = 5;
    [SerializeField] private int bossHP = 1000;
    public bool chaseActivated = true;
    int previousAttack = 1;

    //Attack assets
    [Space]
    [Space]
    [SerializeField] private GameObject projectileRainProjectile;
    [SerializeField] private GameObject projectileAttackProjectile;
    [SerializeField] private BoxCollider2D sweepAttackCollider;

    private bool inAttckstage = false;

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

    IEnumerator StartAttackPhase()
    {
        while (bossHP >= 0)
        {
            sweepAttackCollider.enabled = false;
            int nextAttack = Random.Range(0, 5);
            if (nextAttack != previousAttack)
            {
                previousAttack = nextAttack;
                switch (nextAttack)
                {
                    case 1:
                        StartCoroutine(SweepAttack());
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
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
            yield return new WaitForSeconds(Random.Range(1, 2));
        }
    }

    //Attacks
    private IEnumerator SweepAttack()
    {
        sweepAttackCollider.enabled = true;
        yield return new WaitForSeconds(1);
    }

    private void BodySlam()
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        rb.gravityScale = 1.5f;
    }

    private void ProjectileAttack()
    {
        Instantiate(projectileAttackProjectile, transform.position, Quaternion.identity);
    }

    private void ProjectileRainAttack()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(projectileRainProjectile);
        }
    }
}   