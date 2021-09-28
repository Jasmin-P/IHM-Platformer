using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector2 position;
    public Vector2 velocity;

    public Vector2 gravity = new Vector2(0, -5f);

    public bool bottomDirectionLocked;
    public bool topDirectionLocked;
    public bool leftDirectionLocked;
    public bool rightDirectionLocked;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            velocity += new Vector2(0, 0.1f);
        }

        UpdateVelocity();
        UpdatePosition();
    }

    public void Move(Vector2 translatePosition)
    {
        position += translatePosition;
    }

    private void UpdateVelocity()
    {
        velocity += gravity * Time.deltaTime;

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
}
