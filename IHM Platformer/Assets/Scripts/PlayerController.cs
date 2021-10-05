using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector2 position;
    public Vector2 velocity;

    public static PlayerController instance;
    

    public bool bottomDirectionLocked;
    public bool topDirectionLocked;
    public bool leftDirectionLocked;
    public bool rightDirectionLocked;
    public bool canJump;


    private float minVelocity = 0.1f;
    public float groundDeceleration;
    public float groundAcceleration;
    public float maxXspeed;
    public float maxYspeed;


    public float gravity = -100f;
    private float jumpForce;

    public float timeDivider = 0.001f;

    private bool onDash = false;
    private float timeStartDash;
    public float timeDurationDash = 0.2f;
    public float dashForce = 20f;
    
    private Vector2 dashDirection;

    public float resultYForce;

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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateVelocity();
        UpdatePosition(); 
    }

    public void Move(Vector2 translatePosition)
    {
        position += translatePosition;
    }

    private void UpdateVelocity()
    {


        if (!onDash)
        {
            // pas de gravité pendant le dash
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity += dashForce * dashDirection;
            UpdateDash();
        }
        
        
        velocity.y += jumpForce * Time.deltaTime;

        float deceleration = GroundDeceleration();
        velocity.x += deceleration * Time.deltaTime;
        

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

        if (!onDash)
        {
            LimitVelocity();
        }
    }

    private void UpdatePosition()
    {
        /*
        if (!leftDirectionLocked && velocity.x < 0)
        {
            position.x += velocity.x * Time.deltaTime;
        }
        else if (!rightDirectionLocked && velocity.x > 0)
        {
            position.x += velocity.x * Time.deltaTime;
        }


        if (!bottomDirectionLocked && velocity.y < 0)
        {
            position.y += velocity.y * Time.deltaTime;
        }
        else if (!topDirectionLocked && velocity.y > 0)
        {
            position.y += velocity.y * Time.deltaTime;
        }
        */
        position += velocity * Time.deltaTime;



        transform.position = position;
    }


    public void RightKeyPressed()
    {
        if (velocity.x < maxXspeed)
        {
            velocity.x += groundAcceleration * Time.deltaTime;
        }
    }

    public void LeftkeyPressed()
    {
        if (velocity.x > -maxXspeed)
        {
            velocity.x -= groundAcceleration * Time.deltaTime;
        }
    }

    private float GroundDeceleration()
    {
        if (bottomDirectionLocked)
        {
            //deceleration dans les airs
            if (velocity.x > minVelocity)
            {
                return -groundDeceleration * 1.3f;
            }
            else if (velocity.x >= -minVelocity)
            {
                velocity.x = 0;
                return 0;
            }
            else
            {
                return groundDeceleration * 1.3f;
            }
        }
        else
        {
            //deceleration normale
            if (velocity.x > minVelocity)
            {
                return -groundDeceleration;
            }
            else if (velocity.x >= -minVelocity)
            {
                velocity.x = 0;
                return 0;
            }
            else
            {
                return groundDeceleration;
            }
        }
        
    }

    public void Jump()
    {
        if (canJump)
        {
            velocity.y = 0;
            jumpForce = 250f;
            StartCoroutine(JumpCoroutine());
            
        }
    }

    

    private IEnumerator JumpCoroutine()
    {
        while (jumpForce > 0)
        {
            jumpForce -= 25f * Time.deltaTime * 50;

            if (jumpForce < 0)
            {
                jumpForce = 0;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }


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
}
