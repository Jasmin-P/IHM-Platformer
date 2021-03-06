using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    private PlayerCollider playerCollider;
    private Dash dashManager;
    private Jump jumpManager;

    //Feedback
    public SpriteRenderer spriteRenderer;
    public ParticleSystem trailLeftParticle; 
    public ParticleSystem trailRightParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem spawnParticle;


    public Killer killerManager;

    private Vector3 startingPosition;

    public Vector2 position;
    private Vector2 lastPosition;
    public Vector2 velocity;
    public Vector2 velocity_n;
    
    public bool bottomDirectionLocked;
    public bool topDirectionLocked;
    public bool leftDirectionLocked;
    public bool rightDirectionLocked;

    // Jump
    public int jumpCount;
    public bool onJump;
    private bool jumpPushed;
    private float timeJumpPushed;
    public float timeBufferJump;


    // grab
    public bool onGrab;
    public float grabFallingSpeed;
    public Vector2 grabDirection;


    public float gravity = -100f;

    private float minVelocity = 0.1f;
    
    public float groundAcceleration;
    public float walkSpeed;
    public float sprintSpeed;
    private bool onSprint = false;
    public float actualMaxSpeed;
    public float maxYspeed;

    public float groundProportionalDecelerationX;
    public float airProportionalDecelerationX;
    public float groundFlatDecelerationX;
    public float airFlatDecelerationX;
    private float flatDecelerationX;
    private float proportionalDecelerationX;



    public float timeDivider = 0.001f;

    public bool onDash = false;
    public bool canDash = true;

    private float timeFlexibilityWallJump = 0.2f;
    private float timeLastGrab;
    private bool wallJumpPossible = false;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        position = transform.position;
        actualMaxSpeed = walkSpeed;
        playerCollider = GetComponent<PlayerCollider>();
        dashManager = GetComponent<Dash>();
        jumpManager = GetComponent<Jump>();

        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
    }

    // Update is called once per frame
    void Update()
    {
 
        UpdateVelocity();
        UpdatePosition();

        if (canDash)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }

        if (onJump)
        {
            jumpParticle.Play();
        }

        
        


        float velocity_sqrMagnitude = velocity.sqrMagnitude;
        velocity_n = velocity.normalized;
        
        if (velocity_sqrMagnitude > 1f) //d?s qu'on bouge
        {
            spriteRenderer.size = new Vector2(1 + 0.2f * (2 * velocity_n.x * velocity_n.x - 1), 1 + 0.2f * (2 * velocity_n.y * velocity_n.y - 1));
            
            /*
            if (onSprint)
            {
                if(velocity_n.x > 0)
                {
                    trailLeftParticle.Play();
                }
                else
                {
                    trailRightParticle.Play();
                }
            }
            */
        }
        else //On bouge pas
        {
            spriteRenderer.size = new Vector2(1f, 1f);
        }
        

        if (transform.position.x > 33 && !VictoryController.Instance.onVictory)
        {
            VictoryController.Instance.Pause();
        }

    }

    

    

    private void UpdateVelocity()
    {

        Vector2 deceleration = new Vector2(ComputeDecelerationX(), 0);

        // pas de gravit? pendant le dash
        if (onDash)
        {
            dashManager.UpdateDash();
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        if (jumpPushed)
        {
            Jump();
        }

        if (onJump)
        {
            jumpManager.UpdateJump();
        }
        /*
        else if (onSecondJump)
        {
            UpdateSecondJump();
        }
        else if (onWallJump)
        {
            UpdateWallJump();
        }
        */

        if (onGrab)
        {
            velocity.y = -grabFallingSpeed;
        }
        else if (wallJumpPossible)
        {
            TestIfWallJumpStillPossible();
        }


        velocity += deceleration * Time.deltaTime;

        


        /*
        if (bottomDirectionLocked && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else if (topDirectionLocked && velocity.y > 0)
        {
            velocity.y = 0;
        }

        if (leftDirectionLocked && velocity.x < 0)
        {
            velocity.x = 0;
        }
        else if (rightDirectionLocked && velocity.x > 0)
        {
            velocity.x = 0;
        }
        */
    }

    private void UpdatePosition()
    {
        position += velocity * Time.deltaTime;

        Vector2 movement = position - new Vector2(transform.position.x, transform.position.y);

        playerCollider.UpdateCollisions(ref movement);

        position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);

        transform.position = position;
        
    }

    public void Move(Vector2 translatePosition)
    {
        position += translatePosition;
    }


    public void RightKeyPressed()
    {
        MoveX(new Vector2(1, 0));
    }

    public void LeftkeyPressed()
    {
        MoveX(new Vector2(-1, 0));
    }

    public void MoveX(Vector2 xDirection)
    {
        float speedAugmentation = groundAcceleration * Mathf.Sign(xDirection.x) * Time.deltaTime;
        
        if (Mathf.Abs(speedAugmentation) > actualMaxSpeed - Mathf.Abs(velocity.x))
        {
            velocity.x = actualMaxSpeed * Mathf.Sign(xDirection.x);
        }
        else
        {
            velocity.x += speedAugmentation;
        }
        Grab(xDirection);
    }

    private float ComputeDecelerationX()
    {
        // deceleration au sol
        if (bottomDirectionLocked)
        {
            flatDecelerationX = groundFlatDecelerationX;
            proportionalDecelerationX = groundProportionalDecelerationX;
        }
        // deceleration dans les airs
        else
        {
            flatDecelerationX = airFlatDecelerationX;
            proportionalDecelerationX = airProportionalDecelerationX;
        }

        if (onDash)
        {
            return 0;
        }


        if (velocity.x > minVelocity)
        {
            return (-proportionalDecelerationX * velocity.x - flatDecelerationX);
        }
        else if (velocity.x >= -minVelocity)
        {
            velocity.x = 0;
            return 0;
        }
        else
        {
            return (-proportionalDecelerationX * velocity.x + flatDecelerationX);
        }
    }

    public void GroundTouched()
    {
        jumpCount = 2;
        if (!onDash)
        {
            canDash = true;
        }
    }

    // JumpButtonPressed
    public void Jump()
    {
        if (jumpManager.StartJump(jumpCount, wallJumpPossible))
        {
            jumpPushed = false;
        }
        else
        {
            if (!jumpPushed)
            {
                jumpPushed = true;
                timeJumpPushed = Time.time;
            }
        }

        if (Time.time - timeJumpPushed > timeBufferJump)
        {
            jumpPushed = false;
        }
    }

    // JumpButtonReleased
    public void JumpRelease()
    {
        jumpManager.JumpRelease();
    }

    // DashButtonPressed
    public void Dash(Vector2 dashDirection)
    {
        if (canDash)
        {
            dashManager.StartDash(dashDirection);
            canDash = false;
            onDash = true;
        }
    }


    public void Grab(Vector2 direction)
    {
        if ((leftDirectionLocked && direction.x < 0 && !onJump && velocity.y < 0) || (rightDirectionLocked && direction.x > 0 && !onJump && velocity.y < 0))
        {
            onGrab = true;
            wallJumpPossible = true;
            timeLastGrab = Time.time;
            grabDirection = direction;
        }
        else
        {
            onGrab = false;
        }

        if (rightDirectionLocked)
        {
            wallJumpPossible = true;
            timeLastGrab = Time.time;
            grabDirection = new Vector2(1, 0);
        }
        else if (leftDirectionLocked)
        {
            wallJumpPossible = true;
            timeLastGrab = Time.time;
            grabDirection = new Vector2(-1, 0);
        }

    }

    private void TestIfWallJumpStillPossible()
    {
        if (Time.time - timeLastGrab > timeFlexibilityWallJump)
        {
            wallJumpPossible = false;
        }
    }

    

    public void Sprint()
    {
        onSprint = true;
        actualMaxSpeed = sprintSpeed;
        StartCoroutine("SprintTrail");
    }

    public void StopSprinting()
    {
        onSprint = false;
        actualMaxSpeed = walkSpeed;
        StopCoroutine("SprintTrail");
    }

    IEnumerator SprintTrail()
    {
        while (true)
        {
            if(velocity.x > 0)
            {
                trailLeftParticle.Play();
            }
            else
            {
                trailRightParticle.Play();
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }


    public void KillPlayer()
    {
        killerManager.KillPlayer(transform.position);
        spawnParticle.Play();
    }
}
