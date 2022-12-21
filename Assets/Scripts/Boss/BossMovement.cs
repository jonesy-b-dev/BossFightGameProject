using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    //Boss components
    private Rigidbody2D rb;
    private ParticleSystem sweepParticle;
    private CircleCollider2D sweepAttackCollider;
    private BoxCollider2D slamCollider;
    [SerializeField] BoxCollider2D mainCollider;

    //Player reffrences
    GameObject target;
    PlayerController playerScript;
    Transform playerTransform;

    public int bossHP = 1000;
    public bool chaseActivated = true;
    [SerializeField] int bossSpeed;
    Vector2 moveDirection;
    int previousAttack = 1;

    //Attack assets
    [Space]
    [Space]
    [SerializeField] GameObject projectileRainProjectile;
    [SerializeField] GameObject projectileAttackProjectile;

    private bool inAttckstage = false;
    private bool canSlamDamage;
    private bool canSweepDamage;
    private bool canChase = true;

    private void Awake()
    {
        //Get boss component
        rb = GetComponent<Rigidbody2D>();
        sweepParticle = GetComponent<ParticleSystem>();
        sweepAttackCollider = GetComponent<CircleCollider2D>();
        slamCollider = GetComponent<BoxCollider2D>();

        //Set player reffrences
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<PlayerController>();
        playerTransform = target.transform;
    }
    void Update()
    {
        //Debug.Log(rb.velocity.y);
        if (!chaseActivated)
        {
            //Attack state
            if (!inAttckstage)
            {
                inAttckstage = true;
                StartCoroutine(StartAttackPhase());
            }
            else if (inAttckstage && canChase)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                moveDirection = direction;
            }
        }
    }

    private void FixedUpdate()
    {
        if (inAttckstage && canChase)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * bossSpeed;
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
        canSweepDamage = true;
        sweepAttackCollider.enabled = true;
        sweepParticle.Play();
        //Damage is handeld in collision event
        yield return new WaitForSeconds(1);
    }

    private void BodySlam()
    {
        canChase = false;
        canSlamDamage = true;
        slamCollider.enabled = true;
        mainCollider.enabled = true;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        rb.gravityScale = 1.5f;
        //Damage is handeld in collision event
    }

    private void ProjectileAttack()
    {
        Instantiate(projectileAttackProjectile, transform.position, Quaternion.identity);
    }

    private void ProjectileRainAttack()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(projectileRainProjectile);
        }
    }

    //Collision events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canSlamDamage && rb.velocity.y < 0)
        {
            playerScript.Damage(5);
            slamCollider.enabled = false;
            mainCollider.enabled = false;
            canSlamDamage = false;
            canChase = true;
        }

        else if (collision.gameObject.CompareTag("Player") && canSweepDamage)
        {
            playerScript.Damage(4);
            sweepAttackCollider.enabled = false;
            canSweepDamage = false;
        }

        else
        {
            mainCollider.enabled = false;
            canChase = true;
        }
    }
}   