using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector2 position;
    public Vector2 velocity;

    

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

    public float resultYForce;

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
        resultYForce = gravity + jumpForce;
        velocity.y += gravity * Time.deltaTime;
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
}
