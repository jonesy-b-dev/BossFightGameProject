using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Apparently, unless you want other scripts to access them, you generally shouldn't make variables public in case something
    // messes up on another script. Hence, the serialization. It's pretty much just an extra safety measure.

    // I learned this in a Youtube tutorial so correct me if I'm wrong LMAO

    public int hp;
    public int healItemAmount;
    [SerializeField] private int healAmount;

    public GameObject pauseMenu;
    PlayerAudio playerAudioScript;
    public bool isPaused;

    #region Movement Variables
    private float horizontal;
    private int facing = 1;

    [Space(5)]
    [Header("Movement")]

    [Space(10)]
    [SerializeField] private Animator animator;

    [Space(10)]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private float jumpBuffer;
    private float jumpBufferCounter;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [Space(10)]
    [SerializeField] private bool wallSliding;
    [SerializeField] private float wallSlideSpeed;

    [Space(10)]
    private bool wallJumping;
    private int wallJumpDirection;
    [SerializeField] private float xWallForce;
    [SerializeField] private float yWallForce;
    [SerializeField] private float wallJumpTime;

    [Space(10)]
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private LayerMask groundLayer;

    [Space(10)]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckL;
    [SerializeField] private Transform wallCheckR;
    #endregion

    #region Combat Variables
    [Space(10)]
    [Header("Combat")]
    [Space(10)]

    [SerializeField] private Transform meleeCheck;

    [Space(10)]
    [SerializeField] private float rangedKBForce;
    [SerializeField] private float rangedKBTime;
    private Vector2 kbForce;

    [Space(10)]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject shootParticle;

    [Space(10)]
    [SerializeField] private float meleeCooldown;
    private float meleeCooldownTimer;
    [SerializeField] private float rangedCooldown;
    private float rangedCooldownTimer;
    private bool kbActive;

    #endregion

    private void Start()
    {
        playerAudioScript = GetComponent<PlayerAudio>();
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        #region Jumping
        // Coyote Time
        if (IsGrounded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // Jump Buffer
        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter -= Time.deltaTime;

        // Jump mechanic. Checks if both your jumpBuffer and CoyoteTime are active, AND stops you from jumping normally instead of doing a walljump.
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0f && !wallSliding && !wallJumping)
        {
            player.velocity = new Vector2(player.velocity.x, jumpPower);
            jumpBufferCounter = 0f; // Prevents spamming
        }

        // Wall Jump mechanic.
        else if (Input.GetButtonDown("Jump") && wallSliding)
        {
            if (TouchingL()) wallJumpDirection = 1;
            if (TouchingR()) wallJumpDirection = -1;

            wallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse), wallJumpTime);
            // See region Wall Jump in FixedUpdate for more
        }

        // Halves your vertical momentum when you let go of the button to make for more precise jumps.
        if (Input.GetButtonUp("Jump") && player.velocity.y > 0)
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.5f);
            coyoteTimeCounter = 0f; // Prevents spamming
        }

        #endregion

        #region Combat
        // Changes the direction of the meleeCheck transform to match the way you're facing
        switch (facing)
        {
            case 1: meleeCheck.position = new Vector2(meleeCheck.parent.position.x + 1.5f, meleeCheck.position.y); break;
            case -1: meleeCheck.position = new Vector2(meleeCheck.parent.position.x - 1.5f, meleeCheck.position.y); break;
        }
        // Functions
        if (Input.GetButtonDown("Fire1") && meleeCooldownTimer <= 0) Melee();
        if (Input.GetButtonDown("Fire2") && rangedCooldownTimer <= 0) Ranged();
        if (Input.GetButtonDown("Fire3") && healItemAmount > 0) Heal(healAmount);

        // Cooldown Countdowns
        if (meleeCooldownTimer > 0) meleeCooldownTimer -= Time.deltaTime;
        if (rangedCooldownTimer > 0) rangedCooldownTimer -= Time.deltaTime;
        #endregion

        #region Pausing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseMenu.SetActive(true);
                isPaused = true;

                Time.timeScale = 0;
                Debug.Log("Paused");
            }
            else if (isPaused)
            {
                pauseMenu.SetActive(false);
                isPaused = false;

                Time.timeScale = 1;
                Debug.Log("Unpaused");
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region Horizontal Movement
        // This confuses me so much. You're telling me if I try to make it so that you CAN walk when you're NOT against a wall
        // it completely breaks it and turns you all slippery, but when I make it so you CANT walk when you ARE against a wall
        // it works like normal? bruh.

        // Also checks if you're not currently in a wallJump animation, otherwise its all janky.
        if (!(TouchingR() && horizontal > 0) && !(TouchingL() && horizontal < 0) && !wallJumping)
        {
            player.velocity = new Vector2(horizontal * speed, player.velocity.y);

            if (horizontal < 0) facing = -1;
            else if (horizontal > 0) facing = 1;

            // INSERT WALKING ANIMATION
        }
        // it just works
        #endregion

        #region Wall Jump
        // Checks if you are not grounded and touching a wall on any side, then halves your vertical speed and allows you to walljump.
        if (!IsGrounded() && (TouchingL() || TouchingR()))
        {
            player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, -wallSlideSpeed, float.MaxValue));
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        // I wanted to put this in FixedUpdate in case of deltaTime shenanigans, refers back to Line 60-64.
        if (wallJumping)
        {
            player.velocity = new Vector2(xWallForce * wallJumpDirection, yWallForce);
            if (wallJumpDirection == 1 && TouchingR()) wallJumping = false;
            if (wallJumpDirection == -1 && TouchingL()) wallJumping = false;
        }
        #endregion

        // Knockback
        if (kbActive == true) player.velocity = kbForce;
    }

    #region Ground/Wall Checks
    // These all do the same, they draw a box the same size of the empty object in the editor and return true
    // when they collide with something.
    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0, groundLayer);
    }
    public bool TouchingL()
    {
        return Physics2D.OverlapBox(wallCheckL.position, wallCheckL.localScale, 0, groundLayer);
    }
    public bool TouchingR()
    {
        return Physics2D.OverlapBox(wallCheckR.position, wallCheckR.localScale, 0, groundLayer);
    }
    #endregion

    #region Combat Methods
        void Melee()
        {
            playerAudioScript.Melee();
            // INSERT ATTACK ANIMATION

            Collider2D enemy = Physics2D.OverlapBox(meleeCheck.position, meleeCheck.localScale, 0, enemyLayer);
            Gizmos.color = Color.red;

            // Change to: enemy.GetComponent<BossScript>().Damage();
            if (enemy != null) Debug.Log("You hit " + enemy.name);

            meleeCooldownTimer = meleeCooldown;
        }

        void OnDrawGizmos()
        {
            // Draw a yellow cube at the meleeCheck's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(meleeCheck.position, meleeCheck.localScale);
        }

        void Ranged()
        {
            playerAudioScript.Ranged();
            arrow.GetComponent<Arrow>().dirX = facing;
            Instantiate(arrow, new Vector2(transform.position.x + (facing * 0.75f), transform.position.y), Quaternion.identity);
            Instantiate(shootParticle, new Vector2(transform.position.x + (facing * 0.75f), transform.position.y), Quaternion.Euler(0, 90 * facing, 0));


            //Ranged attack
            arrow.GetComponent<Arrow>().dirX = facing;
            Instantiate(arrow, new Vector2(transform.position.x + (facing * 0.75f), transform.position.y), Quaternion.identity);

            Knockback(rangedKBForce * -facing, player.velocity.y, rangedKBTime);

            rangedCooldownTimer = rangedCooldown;

            // Ranged Animation
            animator.SetBool("isShooting", true);
            Invoke(nameof(SetShootingToFalse), 0.1f);

            //Call ranged audio event
            playerAudioScript.Ranged();
        }

        // IMPORTANT
        // Can be executed through any code, gives the player knockback by X/Y amount in dir direction for time seconds.
        public void Knockback(float powerX, float powerY, float time)
        {
            kbActive = true;
            kbForce = new Vector2(powerX, powerY);

            Invoke(nameof(SetKBActiveToFalse), time);
        }
    #endregion

    #region HP Methods
        void Heal(int amount)
        {
            // INSERT HEAL ANIMATION. PROBABLY JUST A FLASHING GREEN OVERLAY BUT WE'LL FIGURE IT OUT LATER
            hp += amount;
            healItemAmount--;
        }

        // IMPORTANT
        // Can be executed through any code, subtracts d amount of damage from the player. Death function will be added later.
        public void Damage(int d)
        {
            animator.SetBool("isHurting", true);
            Invoke(nameof(SetHurtingToFalse), 0.1f);

            hp -= d;
            Debug.Log(hp);
            if (hp <= 0) Die();
        }
        void Die()
        {

        }
    #endregion

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    } // Needed for Invoke() under the Jumping region -> Wall Jump
    void SetKBActiveToFalse() // Needed for Invoke() under the Combat Methods region -> Knockback()
    {
        kbActive = false;
    }
    void SetHurtingToFalse()
    {
        animator.SetBool("isHurting", false);
    }
    void SetShootingToFalse()
    {
        animator.SetBool("isShooting", false);
    }
}
