using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector2 position;
    private Vector2 lastPosition;
    public Vector2 velocity;

    public static PlayerController instance;

    private PlayerCollider playerCollider;
    

    public bool bottomDirectionLocked;
    public bool topDirectionLocked;
    public bool leftDirectionLocked;
    public bool rightDirectionLocked;

    // Jump
    public bool canJump;
    private bool onJump;
    private float timeStartJump;
    public float timeDurationJump = 0.2f;
    private float previousTimeJump;
    public float jumpForce = 250f;
    private float variableJumpForce;
    public bool released = false;

    // grab
    private bool onGrab;
    public float grabFallingSpeed;
    private Vector2 grabDirection;

    public Vector2 wallJumpForce;
    public bool onWallJump;

    public float gravity = -100f;

    private float minVelocity = 0.1f;
    
    public float groundAcceleration;
    public float maxXspeed;
    public float maxYspeed;

    public float groundDecelerationCoefficient;
    public float airDecelerationCoefficient;
    public float groundDeceleration;
    public float airDeceleration;



    public float timeDivider = 0.001f;

    private bool onDash = false;
    private float timeStartDash;
    public float timeDurationDash = 0.2f;
    public float dashForce = 20f;
    
    private Vector2 dashDirection;

    public float resultYForce;

    int i = 0;

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
        position = transform.position;
        playerCollider = GetComponent<PlayerCollider>();
    }

    // Update is called once per frame
    void Update()
    {
 
        UpdateVelocity();
        UpdatePosition();
        
    }

    

    private void UpdateVelocity()
    {

        // pas de gravité pendant le dash
        if (onDash)
        {
            velocity += dashForce * dashDirection;
            UpdateDash();
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (onJump)
        {
            UpdateJump();
        }

        if (onGrab)
        {
            velocity.y = -grabFallingSpeed;
        }

        /*
        if (onGrab)
        {
            if (grabDirection.x < 0 && Input.GetKey(KeyCode.Space))
            {
                WallJump(new Vector2(1, 0));
            }
        }
        */


        float deceleration = GroundDeceleration();
        velocity.x += deceleration * Time.deltaTime;
        

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

        Vector2 movement = position - new Vector2(lastPosition.x, lastPosition.y);

        playerCollider.UpdateCollisions(ref movement);

        position = new Vector2(lastPosition.x + movement.x, lastPosition.y + movement.y);

        lastPosition = position;
        transform.position = position;

    }

    public void Move(Vector2 translatePosition)
    {
        position += translatePosition;
    }


    public void RightKeyPressed()
    {
        velocity.x += groundAcceleration * Time.deltaTime;
        Grab(new Vector2(1,0));

        if (onGrab)
        {
            if (grabDirection.x < 0)
            {
                WallJump(new Vector2(1, 0));
            }
        }
    }

    public void LeftkeyPressed()
    {
        velocity.x -= groundAcceleration * Time.deltaTime;
        Grab(new Vector2(-1, 0));


        
    }

    public void MoveX(Vector2 xDirection)
    {
        velocity.x += groundAcceleration * Mathf.Sign(xDirection.x);

        Grab(xDirection);
    }

    private float GroundDeceleration()
    {
        if (bottomDirectionLocked)
        {
            //deceleration normale
            if (velocity.x > minVelocity)
            {
                return -(groundDecelerationCoefficient * velocity.x * velocity.x + groundDeceleration);
            }
            else if (velocity.x >= -minVelocity)
            {
                velocity.x = 0;
                return 0;
            }
            else
            {
                return (groundDecelerationCoefficient * velocity.x * velocity.x + groundDeceleration);
            }
        }
        else
        {
            //deceleration dans les airs
            if (velocity.x > minVelocity)
            {
                return -(airDecelerationCoefficient * velocity.x * velocity.x + airDeceleration);
            }
            else if (velocity.x >= -minVelocity)
            {
                velocity.x = 0;
                return 0;
            }
            else
            {
                return (airDecelerationCoefficient * velocity.x * velocity.x + airDeceleration);
            }
        }
        
    }

    // Jump
    public void Jump()
    {
        if (canJump)
        {
            velocity.y = jumpForce;
            variableJumpForce = jumpForce;
            onJump = true;
            timeStartJump = Time.time;
        }

        else if (onGrab)
        {
            if (grabDirection.x >= 0)
            {
                WallJump(new Vector2(-1, 0));
            }
            else
            {
                WallJump(new Vector2(1, 0));
            }
        }

        
    }

    public void JumpRelease()
    {
        released = true;
    }

    private void UpdateJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onJump = false;
            released = false;
        }
        else
        {
              velocity.y = variableJumpForce;
        }

        if (released && Time.time - timeStartJump > timeDurationJump * 0.5f)
        {
            variableJumpForce = jumpForce * 0.5f;
        }

        previousTimeJump = Time.time;
    }



    // pas utilisé
    private void LimitVelocity()
    {
        if (velocity.x > maxXspeed)
        {
            velocity.x = maxXspeed;
        }
        else if (velocity.x < -maxXspeed)
        {
            velocity.x = -maxXspeed;
        }

        if (velocity.y > maxYspeed)
        {
            velocity.y = maxYspeed;
        }
        else if (velocity.y < -maxYspeed)
        {
            velocity.y = -maxYspeed;
        }
    }

    public void Dash(Vector2 dashDirection)
    {
        onDash = true;
        this.dashDirection = dashDirection;
        timeStartDash = Time.time;
    }

    private void UpdateDash()
    {
        if (Time.time - timeStartDash > timeDurationDash)
        {
            onDash = false;
        }
    }

    public void Grab(Vector2 direction)
    {
        if (leftDirectionLocked && direction.x < 0 && !onJump && velocity.y < 0)
        {
            onGrab = true;
            grabDirection = direction;
        }
        else if (rightDirectionLocked && direction.x > 0 && !onJump && velocity.y < 0)
        {
            onGrab = true;
            grabDirection = direction;
        }
        else
        {
            onGrab = false;
        }
    }

    public void WallJump(Vector2 direction)
    {
        onWallJump = true;
        velocity = new Vector2(direction.x * wallJumpForce.x, wallJumpForce.y);
    }
}
