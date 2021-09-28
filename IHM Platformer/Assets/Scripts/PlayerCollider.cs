using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    Collider2D playerCollider;
    PlayerController playerController;
    Bounds bounds;

    public LayerMask colliderMask;

    public Vector2 bottomLeftPoint;
    public Vector2 bottomRightPoint;
    public Vector2 topLeftPoint;
    public Vector2 topRightPoint;

    public int numberOfRays = 5;
    public float distance = 0.1f;


    public float xJumpFlexibility = 0.1f;
    public float yJumpFlexibility = 0.5f;
    public int jumpNumberOfRays = 20;


    public float collisionDistance;

    public bool debug = true;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerController = GetComponent<PlayerController>();
        if (!playerCollider)
        {
            Debug.LogError("collider in PlayerCollider is an empty object");
        }

        UpdateBounds();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBounds();
        BottomCollision();
        RightCollision();
        LeftCollision();
        TopCollision();
        CanJump();
    }




    private void UpdateBounds()
    {
        bounds = playerCollider.bounds;
        bounds.Expand(collisionDistance);
        bottomLeftPoint = new Vector2(bounds.min.x, bounds.min.y);
        bottomRightPoint = new Vector2(bounds.max.x, bounds.min.y);
        topLeftPoint = new Vector2(bounds.min.x, bounds.max.y);
        topRightPoint = new Vector2(bounds.max.x, bounds.max.y);

        if (debug)
        {
            Debug.DrawLine(bottomLeftPoint, bottomRightPoint, Color.blue, 0.1f);
            Debug.DrawLine(bottomRightPoint, topRightPoint, Color.blue, 0.1f);
            Debug.DrawLine(topRightPoint, topLeftPoint, Color.blue, 0.1f);
            Debug.DrawLine(topLeftPoint, bottomLeftPoint, Color.blue, 0.1f);
        }
    }

    private void LeftCollision()
    {
        RaycastHit2D hit = DetectCollision(topLeftPoint, bottomLeftPoint, Vector2.left, distance, numberOfRays);
        if (hit)
        {
            if (!playerController.leftDirectionLocked)
            {
                playerController.Move(Vector2.left * hit.distance);
                playerController.leftDirectionLocked = true;
            }
        }
        else
        {
            playerController.leftDirectionLocked = false;
        }
    }

    private void RightCollision()
    {
        RaycastHit2D hit = DetectCollision(bottomRightPoint, topRightPoint, Vector2.right, distance, numberOfRays);
        if (hit)
        {
            if (!playerController.rightDirectionLocked)
            {
                playerController.Move(Vector2.right * hit.distance);
                playerController.rightDirectionLocked = true;
            }
        }
        else
        {
            playerController.rightDirectionLocked = false;
        }
    }

    private void BottomCollision()
    {
        RaycastHit2D hit = DetectCollision(bottomLeftPoint, bottomRightPoint, -Vector2.up, distance*2, numberOfRays);
        if (hit)
        {
            if (!playerController.bottomDirectionLocked)
            {
                playerController.Move(-Vector2.up * hit.distance);
                playerController.bottomDirectionLocked = true;
            }
        }
        else
        {
            playerController.bottomDirectionLocked = false;
        }
    }

    private void TopCollision()
    {
        RaycastHit2D hit = DetectCollision(topRightPoint, topLeftPoint, Vector2.up, distance, numberOfRays);
        if (hit)
        {
            if (!playerController.topDirectionLocked)
            {
                playerController.Move(Vector2.up * hit.distance);
                playerController.topDirectionLocked = true;
            }
        }
        else
        {
            playerController.topDirectionLocked = false;
        }
    }


    private RaycastHit2D DetectCollision (Vector2 startPoint, Vector2 endPoint, Vector2 direction, float rayDistance, int rayNumber)
    {
        float x = startPoint.x;
        float y = startPoint.y;
        RaycastHit2D hit;
        for (int i = 0; i < rayNumber; i++)
        {
            x += (endPoint.x - startPoint.x) / (rayNumber + 1);
            y += (endPoint.y - startPoint.y) / (rayNumber + 1);


            RayInfo ray;
            ray.origin = new Vector2(x, y);
            ray.direction = direction;
            ray.distance = rayDistance;
            hit = Raycast(ray);

            if (hit)
            {
                return hit;
            }
            
        }

        return new RaycastHit2D();
    }


    private void CanJump()
    {
        Vector2 startPoint = bottomLeftPoint;
        Vector2 endPoint = bottomRightPoint;

        if (!playerController.leftDirectionLocked)
        {
            startPoint.x -= xJumpFlexibility;
        }
        if (!playerController.rightDirectionLocked)
        {
            endPoint.x += xJumpFlexibility;
        }
        RaycastHit2D hit = DetectCollision(startPoint, endPoint, -Vector2.up, yJumpFlexibility, jumpNumberOfRays);


        if (hit)
        {
               
            playerController.canJump = true;
        }
        else
        {
            playerController.canJump = false;
        }
    }


    private RaycastHit2D Raycast(RayInfo ray)
    {
        if (debug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * ray.distance, Color.red, 0.1f);
        }

        return Physics2D.Raycast(ray.origin, ray.direction, ray.distance);
    }

    public struct RayInfo
    {
        public Vector2 origin;
        public Vector2 direction;
        public float distance;
    }
}
