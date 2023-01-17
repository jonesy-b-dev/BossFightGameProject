using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    //Boss components
    [Header("Components")]
    private Rigidbody2D rb;
    //private ParticleSystem sweepParticle;
    private CircleCollider2D sweepAttackCollider;
    private BoxCollider2D slamCollider;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D mainCollider;
    BossAudio bossAudioScript;

    //Player reffrences
    [Header("Player Ref")]
    GameObject target;
    PlayerController playerScript;
    Transform playerTransform;

    [Header("Boss stats")]
    public int bossHP = 1000;
    public bool chaseActivated = true;
    [SerializeField] float bossSpeed;
    Vector2 moveDirection;
    int previousAttack = 1;

    //Attack assets
    [Space]
    [Header("Boss attack assets")]
    [SerializeField] GameObject projectileRainProjectile;
    [SerializeField] GameObject projectileAttackProjectile;
    [SerializeField] GameObject dieParticle;

    private bool inAttckstage = false;
    private bool canSlamDamage;
    private bool canSweepDamage;
    private bool canChase = true;

    Vector2 posTemp;

    private void Awake()
    {
        //Get boss component
        rb = GetComponent<Rigidbody2D>();
        //sweepParticle = GetComponent<ParticleSystem>();
        sweepAttackCollider = GetComponent<CircleCollider2D>();
        slamCollider = GetComponent<BoxCollider2D>();

        //Set player reffrences
    }
    private void Start()
    {
        bossAudioScript = GetComponent<BossAudio>();
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<PlayerController>();
        playerTransform = target.transform;

        posTemp = transform.position;

    }
    void Update()
    {
        Vector2 pos = transform.position;

        //Sprite flip
        if (pos.x > posTemp.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            posTemp = pos;
        }
        else if (pos.x < posTemp.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            posTemp = pos;
        }

        //Debug.Log(rb.velocity.y);
        if (!chaseActivated)
        {
            //Attack state
            if (!inAttckstage)
            {
                inAttckstage = true;
                mainCollider.enabled = true;
                StartCoroutine(StartAttackPhase());
            }
            else if (inAttckstage && canChase)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                moveDirection = direction;
            }
            //if (canSlamDamage && (GroundCheckL.GetComponent<BoxCollider2D>. || GroundCheckR.GetComponent<BoxCollider2D>))
            //{
            //    BodySlamAudio();
            //}
        }
    }

    private void FixedUpdate()
    {
        if (inAttckstage && canChase)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * bossSpeed;
        }
    }

    public void Damage(int d)
    {
        bossHP -= d;
        Debug.Log("You hit the boss! HP = " + bossHP);
        if (bossHP <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("sdf");
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().enabled = false;
        playerScript.Win();
    }
    private void BodySlamAudio()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1);
    }



    IEnumerator StartAttackPhase()
    {
        while (bossHP >= 0)
        {
            //Set anim variables 
            //animator.SetBool("CanMelee", false);


            sweepAttackCollider.enabled = false;
            mainCollider.enabled = true;
            int nextAttack = Random.Range(0, 5);
            if (nextAttack != previousAttack || nextAttack != 2)
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
            yield return new WaitForSeconds(Random.Range(3, 5));
        }
    }

    #region Attacks
    private IEnumerator SweepAttack()
    {
        canSweepDamage = true;
        sweepAttackCollider.enabled = true;
        animator.SetBool("CanMelee", true);
        //sweepParticle.Play();
        //Damage is handeld in collision event
        yield return new WaitForSeconds(1);
    }

    private void BodySlam()
    {
        canChase = false;
        canSlamDamage = true;
        slamCollider.enabled = true;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        rb.gravityScale = 1.5f;
        //Damage is handeld in collision event
    }

    private void ProjectileAttack()
    {
        bossAudioScript.ProjectileAttack();
        Instantiate(projectileAttackProjectile, transform.position, Quaternion.identity);
    }

    private void ProjectileRainAttack()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(projectileRainProjectile);
        }
    }
    #endregion

    //Collision events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canSlamDamage && rb.velocity.y < 0)
        {
            playerScript.Damage(5);
            slamCollider.enabled = false;
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
            canChase = true;
        }
    }
}