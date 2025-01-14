using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] float      m_superJumpForce = 15.0f;
    [SerializeField] float      m_superJumpChargeTime = 1.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] float      m_wallJumpBufferTime = 0.05f;
    [SerializeField] int        m_maxJumps = 2;
    [SerializeField] bool       m_hasSuperJump = true;
    [SerializeField] bool       m_hasDoubleJump = true;
    [SerializeField] bool       m_hasAirDashed = true;
    [SerializeField] float      m_airDashSpeed = 15.0f;
    [SerializeField] float      m_airDashDuration = 0.2f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private bool                m_isBlocking = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private bool                m_isDead=false;
    private float               m_jumpHoldTime = 0.0f;
    private bool                m_isChargingSuperJump = false;
    private float               m_wallJumpBufferTimeCounter = 0.0f;
    private int                 m_jumpCount = 0;
    private float               m_airDashCooldown = 0.5f; // Cooldown between dashes
    private float               m_airDashTimer = 0.1f;
    private bool                m_isAirDashing = false;

    public int health = 100;
    public GameObject deathEffect;

    HeroWeapon weapon = new HeroWeapon();

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        if (m_isWallSliding)
        {
            // If the player is in wall slide state, we start the buffer timer
            m_wallJumpBufferTimeCounter += Time.deltaTime;
        }
        else
        {
            // Reset the buffer timer if not wall sliding
            m_wallJumpBufferTimeCounter = 0.0f;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_jumpCount = 0;
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling && !m_isDead && !m_isAirDashing && !m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        bool isTouchingRightWall = m_wallSensorR1.State() && m_wallSensorR2.State();
        bool isTouchingLeftWall = m_wallSensorL1.State() && m_wallSensorL2.State();
        bool isTouchingWall = isTouchingRightWall || isTouchingLeftWall;

        bool isMovingAwayFromWall = (isTouchingRightWall && inputX < 0) || (isTouchingLeftWall && inputX > 0);

        // Start wall sliding if touching a wall and falling
        if (isTouchingWall && m_body2d.velocity.y < 0 && !m_grounded && !isMovingAwayFromWall)
        {
            m_isWallSliding = true;
        }
        else // Stop wall sliding when conditions are not met
        {
            m_isWallSliding = false;
        }

        // Set the wall slide animation state
        m_animator.SetBool("WallSlide", m_isWallSliding);


        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            m_isDead = true;
        }
        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && !m_isDead)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_isBlocking = true;
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            m_isBlocking = false;
            m_animator.SetBool("IdleBlock", false);
        }

        if (m_airDashTimer > 0)
        {
            m_airDashTimer -= Time.deltaTime;
        }

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isAirDashing && !m_isWallSliding && !m_isDead)
        {
            if (m_grounded)
            {
                // Roll logic
                m_rolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }
            else if (!m_grounded && !m_isWallSliding && !m_isAirDashing && m_airDashTimer <= 0 && !m_isDead)
            {
                m_isAirDashing = true;
                //  m_animator.SetTrigger("Roll"); // Reuse the roll animation for air dash
                StartCoroutine(PerformAirDash());
            }
        }

        // -- Super Jump Logic --
        if (Input.GetKey(KeyCode.Space) && m_grounded && !m_rolling && !m_isDead)
        {
            m_isChargingSuperJump = true;
            m_jumpHoldTime += Time.deltaTime;

            // Cap the jump hold time to prevent overcharging
            if (m_jumpHoldTime >= m_superJumpChargeTime)
                m_jumpHoldTime = m_superJumpChargeTime;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !m_isWallSliding && m_jumpCount < m_maxJumps)
        {
            m_jumpCount++;
            float jumpForce = Mathf.Lerp(m_jumpForce, m_superJumpForce, m_jumpHoldTime / m_superJumpChargeTime);
            if (m_grounded)
            {
                jumpForce = Mathf.Lerp(m_jumpForce, m_superJumpForce, m_jumpHoldTime / m_superJumpChargeTime);
                m_isChargingSuperJump = false; // Reset super jump charge for grounded jump
            }
            else
            {
                jumpForce = m_jumpForce; // Use standard jump force for the double jump
            }

            // Perform the jump with the calculated force
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, jumpForce);

            // Reset jump charge variables
            m_jumpHoldTime = 0.0f;
            m_isChargingSuperJump = false;

            // Temporarily disable the ground sensor to prevent immediate landing
            m_groundSensor.Disable(0.2f);
        }

        // Wall Jump
        if (!isTouchingWall)
        {
            m_wallJumpBufferTimeCounter += Time.deltaTime;
        }
        else
        {
            m_wallJumpBufferTimeCounter = 0f;  // Reset the buffer timer when touching the wall again
        }

        // Wall jump logic with buffer time
        if (m_isWallSliding && m_wallJumpBufferTimeCounter < m_wallJumpBufferTime && Input.GetKeyDown(KeyCode.Space))
        {
            // Determine the direction to jump away from the wall
            int wallJumpDirection = isTouchingRightWall ? -1 : 1;  // Right wall -> move left, Left wall -> move right

            // Apply both horizontal and vertical velocity components for the jump
            float horizontalForce = wallJumpDirection * m_rollForce;  // Move away from the wall horizontally
            float verticalForce = m_jumpForce;  // Standard vertical jump force

            // Apply the jump forces
            m_body2d.velocity = new Vector2(horizontalForce, verticalForce);

            // Trigger the jump animation
            m_animator.SetTrigger("Jump");

            // Stop wall sliding after the jump
            m_isWallSliding = false;

            // Temporarily disable the wall sensors to prevent immediate re-detection after wall jump
            if (wallJumpDirection == -1) // Jumping left (away from right wall)
            {
                m_wallSensorR1.Disable(0.2f);
                m_wallSensorR2.Disable(0.2f);
            }
            else if (wallJumpDirection == 1) // Jumping right (away from left wall)
            {
                m_wallSensorL1.Disable(0.2f);
                m_wallSensorL2.Disable(0.2f);
            }

            // Reset the buffer time counter after the jump
            m_wallJumpBufferTimeCounter = 0.0f;
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    public void TakeDamage(int damage)
    {
        if (m_isBlocking || m_rolling) return;

        health -= damage;

        m_animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator PerformAirDash()
    {
        // Store original gravity scale and velocity
        float originalGravityScale = m_body2d.gravityScale;
        Vector2 originalVelocity = m_body2d.velocity;

        // Disable gravity during the air dash
        m_body2d.gravityScale = 0;
        m_body2d.velocity = Vector2.zero;

        // Apply air dash velocity in the direction the character is facing
        m_body2d.velocity = new Vector2(originalVelocity.x * m_airDashSpeed, 0);
  
        // Wait for the air dash duration
        yield return new WaitForSeconds(m_airDashDuration);

        // Restore gravity and vertical velocity (if any)
        m_body2d.gravityScale = originalGravityScale;
        m_body2d.velocity = new Vector2(originalVelocity.x, m_body2d.velocity.y);

        // End air dash and start cooldown
        m_isAirDashing = false;
        m_airDashTimer = m_airDashCooldown;
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
