using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    Collider2D playerCollider;
    PlayerController playerController;
    Bounds bounds;

    public Vector2 bottomLeftPoint;
    public Vector2 bottomRightPoint;
    public Vector2 topLeftPoint;
    public Vector2 topRightPoint;

    public int numberOfRays = 3;
    public float distanceRaycast = 0.1f;


    public float xJumpFlexibility = 0.1f;
    public float yJumpFlexibility = 0.5f;
    public int jumpNumberOfRays = 1;

    private LayerMask mask;


    public float collisionDistance; //Distance fixe. Si la distance à un objet est inférieure ou égale à celle-ci, on considère qu'il y a collision

    public bool debug = true;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        //playerController = GetComponent<PlayerController>();

        UpdateBounds();

        mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        UpdateBounds();
        LeftCollision(distance);
        RightCollision(distance);
        TopCollision(distance);
        BottomCollision(distance);
        DiagonalsLeftCollision(distance);
        DiagonalsRightCollision(distance);
        CanJump();
        */
    }



    private void UpdateBounds()
    {
        bounds = playerCollider.bounds;
        //bounds.Expand(collisionDistance);
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

    private void LeftCollision(float length)
    {
        if (PlayerController.instance.velocity.x <= 0)
        {
            RaycastHit2D hit = DetectCollision(topLeftPoint, bottomLeftPoint, Vector2.left, length, numberOfRays);
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
    }

    private void RightCollision(float length)
    {
        if (PlayerController.instance.velocity.x >= 0)
        {
            RaycastHit2D hit = DetectCollision(bottomRightPoint, topRightPoint, Vector2.right, length, numberOfRays);
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
    }

    private void BottomCollision(float length)
    {
        if (PlayerController.instance.velocity.y <= 0)
        {
            RaycastHit2D hit = DetectCollision(bottomLeftPoint, bottomRightPoint, -Vector2.up, length, numberOfRays);
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
    }

    private void TopCollision(float length)
    {
        if (PlayerController.instance.velocity.y >= 0)
        {
            RaycastHit2D hit = DetectCollision(topRightPoint, topLeftPoint, Vector2.up, length, numberOfRays);
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
    }

    private void DiagonalsRightCollision(float length) 
    {
        RaycastHit2D hitDiagonal = DetectCollision(bottomRightPoint, bottomRightPoint, new Vector2(1, -1), distanceRaycast, 1);
        Vector2 checkRight = new Vector2(bottomRightPoint.x, bottomRightPoint.y + 0.1f);
        RaycastHit2D hitRight = DetectCollision(checkRight, checkRight, new Vector2(1, 0), distanceRaycast, 1);
        Vector2 checkDown = new Vector2(bottomRightPoint.x - 0.1f, bottomRightPoint.y);
        RaycastHit2D hitDown = DetectCollision(checkDown, checkDown, new Vector2(0, -1), distanceRaycast, 1);
        RaycastHit2D hitRightUpper = DetectCollision(topRightPoint, topRightPoint, new Vector2(1, 0), distanceRaycast, 1);
        RaycastHit2D hitDownLefter = DetectCollision(bottomLeftPoint, bottomLeftPoint, new Vector2(0, -1), distanceRaycast, 1);
        if (hitDiagonal && hitRight && hitDown && !hitRightUpper && !hitDownLefter)
        {
            Debug.Log("blocked!");
            int i = 4;
            while (i > 1)
            {
                RaycastHit2D hitDiagonalFurther = DetectCollision(new Vector2(bottomRightPoint.x + (distanceRaycast * i)/2, bottomRightPoint.y - distanceRaycast * i), new Vector2(bottomRightPoint.x + (distanceRaycast * i)/2, bottomRightPoint.y - distanceRaycast), new Vector2(1, -1), distanceRaycast, 1);
                if (hitDiagonalFurther)
                {
                    break;
                }
                i--;
            }
            playerController.Move(new Vector2(0,1) * (distanceRaycast * i)/24);
        }
    }
    private void DiagonalsLeftCollision(float length) 
    {
        RaycastHit2D hitDiagonal = DetectCollision(bottomLeftPoint, bottomLeftPoint, new Vector2(-1, -1), distanceRaycast, 1);
        Vector2 checkLeft = new Vector2(bottomLeftPoint.x, bottomLeftPoint.y + 0.1f);
        RaycastHit2D hitLeft = DetectCollision(checkLeft, checkLeft, new Vector2(-1, 0), distanceRaycast, 1);
        Vector2 checkDown = new Vector2(bottomLeftPoint.x + 0.1f, bottomLeftPoint.y);
        RaycastHit2D hitDown = DetectCollision(checkDown, checkDown, new Vector2(0, -1), distanceRaycast, 1);
        RaycastHit2D hitLeftUpper = DetectCollision(topLeftPoint, topLeftPoint, new Vector2(1, 0), distanceRaycast, 1);
        RaycastHit2D hitDownLefter = DetectCollision(bottomLeftPoint, bottomLeftPoint, new Vector2(0, -1), distanceRaycast, 1);
        if (hitDiagonal && hitLeft && hitDown && !hitLeftUpper && !hitDownLefter)
            if (hitDiagonal && hitLeft && hitDown)
        {
            int i = 4;
            while (i > 1)
            {
                RaycastHit2D hitDiagonalFurther = DetectCollision(new Vector2(bottomLeftPoint.x - (distanceRaycast * i)/2, bottomLeftPoint.y - distanceRaycast * i), new Vector2(bottomLeftPoint.x - (distanceRaycast * i)/2, bottomLeftPoint.y - distanceRaycast), new Vector2(1, -1), distanceRaycast, 1);
                if (hitDiagonalFurther)
                {
                    break;
                }
                i--;
            }
            playerController.Move(new Vector2(0, 1) * (distanceRaycast * i)/24);
        }
    }


    private RaycastHit2D DetectCollision (Vector2 startPoint, Vector2 endPoint, Vector2 direction, float rayDistance, int rayNumber)
    {
        float x = startPoint.x;
        float y = startPoint.y;
        RaycastHit2D hit = new RaycastHit2D();
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

        return hit;
    }


    private void CanJump()
    {
        Vector2 startPoint = bottomLeftPoint;
        Vector2 endPoint = bottomRightPoint;

        if (!PlayerController.instance.leftDirectionLocked)
        {
            startPoint.x -= xJumpFlexibility;
        }
        if (!PlayerController.instance.rightDirectionLocked)
        {
            endPoint.x += xJumpFlexibility;
        }
        RaycastHit2D hit = DetectCollision(startPoint, endPoint, -Vector2.up, yJumpFlexibility, jumpNumberOfRays);


        if (hit)
        {

            PlayerController.instance.ResetJumpGrab();
        }
        else
        {
            if (PlayerController.instance.jumpCount == 2)
            {
                PlayerController.instance.jumpCount--;
            }
        }
    }


    private RaycastHit2D Raycast(RayInfo ray)
    {
        if (debug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * ray.distance, Color.red, 0.1f);
        }

        return Physics2D.Raycast(ray.origin, ray.direction, ray.distance, mask);
    }

    public struct RayInfo
    {
        public Vector2 origin;
        public Vector2 direction;
        public float distance;
    }

    
    public void UpdateCollisions(ref Vector2 movement) // fonction à tester
    {   //bottomLeftPoint/2 + topLeftPoint/2 = 1/2 * (bottomLeftPoint + topLeftPoint) 

        UpdateBounds();

        RaycastHit2D hitDown = new RaycastHit2D();
        RaycastHit2D hitUp = new RaycastHit2D();
        RaycastHit2D hitLeft = new RaycastHit2D();
        RaycastHit2D hitRight = new RaycastHit2D();


        PlayerController.instance.bottomDirectionLocked = false;
        PlayerController.instance.topDirectionLocked = false;
        PlayerController.instance.leftDirectionLocked = false;
        PlayerController.instance.rightDirectionLocked = false;


        if (movement.y < 0)
        {
            hitDown = DetectCollision(bottomLeftPoint, bottomRightPoint, Vector2.down, Vector2.Dot(Vector2.down, movement), numberOfRays);
            
        }
        else
        {
            hitUp = DetectCollision(topRightPoint, topLeftPoint, Vector2.up, movement.y, numberOfRays);
        }

        if (movement.x < 0)
        {
            hitLeft = DetectCollision(bottomLeftPoint, topLeftPoint, Vector2.left, -movement.x, numberOfRays);
        }
        else
        {
            hitRight = DetectCollision(bottomRightPoint, topRightPoint, Vector2.right, movement.x, numberOfRays);
        }

        if (hitDown)
        {
            movement.y += (-hitDown.distance + Vector2.Dot(Vector2.down, movement));
            PlayerController.instance.velocity.y = 0;
            PlayerController.instance.bottomDirectionLocked = true;

        }
        else if (hitUp)
        {
            movement.y += (hitUp.distance - movement.y);
            PlayerController.instance.velocity.y = 0;
            PlayerController.instance.topDirectionLocked = true;
        }
        if (hitLeft)
        {
            movement.x += (-hitLeft.distance - movement.x);
            PlayerController.instance.velocity.x = 0;
            PlayerController.instance.leftDirectionLocked = true;
        }
        else if (hitRight)
        {
            movement.x += (hitLeft.distance - movement.x);
            PlayerController.instance.velocity.x = 0;
            PlayerController.instance.rightDirectionLocked = true;
        }

        CanJump();

    }
    
}
