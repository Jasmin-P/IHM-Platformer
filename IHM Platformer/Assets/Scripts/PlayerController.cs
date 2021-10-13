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
    public float walkSpeed;
    public float sprintSpeed;
    private float actualMaxSpeed;
    public float maxYspeed;

    public float groundProportionalDecelerationX;
    public float airProportionalDecelerationX;
    public float groundFlatDecelerationX;
    public float airFlatDecelerationX;
    private float flatDecelerationX;
    private float proportionalDecelerationX;



    public float timeDivider = 0.001f;

    public bool onDash = false;
    private float timeStartDash;

    public float timeFreezeOnDash = 0.1f;
    public float timeInsideDash = 0.2f;
    public float timeDecelerationAfterDash = 0.1f;

    public float positionDisplacement = 4;
    
    public float dashSpeed = 20f;
    
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
        actualMaxSpeed = walkSpeed;
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

        Vector2 deceleration = new Vector2(ComputeDecelerationX(), 0);

        // pas de gravité pendant le dash
        if (onDash)
        {
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
    /*
    private void LimitVelocity()
    {
        if (velocity.x > actualMaxSpeed)
        {
            velocity.x = ac;
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
    */

    public void Dash(Vector2 dashDirection)
    {
        if (!onDash)
        {
            if (dashDirection == new Vector2(0, 0))
            {
                dashDirection = Vector2.right;
            }
            onDash = true;
            this.dashDirection = dashDirection;
            timeStartDash = Time.time;
        }
    }

    private void UpdateDash()
    {
        float currentTime = Time.time - timeStartDash;

        if (currentTime < timeFreezeOnDash)
        {
            velocity = new Vector2(0, 0);
        }
        else if (currentTime < timeFreezeOnDash + timeInsideDash)
        {
            velocity = GetCurrentDashSpeed(currentTime) * dashDirection;
        }

        else if (currentTime < timeFreezeOnDash + timeInsideDash + timeDecelerationAfterDash)
        {
            velocity = dashDirection * actualMaxSpeed;
        }

        else
        {
            onDash = false;
        }
    }

    public float GetCurrentDashSpeed(float currentTime)
    {
        float currentDashSpeed = positionDisplacement / timeInsideDash;
        return currentDashSpeed;
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

    public void Sprint()
    {
        actualMaxSpeed = sprintSpeed;
    }

    public void StopSprinting()
    {
        actualMaxSpeed = walkSpeed;
    }
}
