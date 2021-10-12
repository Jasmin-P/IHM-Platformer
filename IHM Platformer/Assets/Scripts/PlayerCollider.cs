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

    public int numberOfRays = 3;
    public float distance = 0.1f;


    public float xJumpFlexibility = 0.1f;
    public float yJumpFlexibility = 0.5f;
    public int jumpNumberOfRays = 1;


    public float collisionDistance; //Distance fixe. Si la distance à un objet est inférieure ou égale à celle-ci, on considère qu'il y a collision

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
        DiagonalsLeftCollision();
        DiagonalsRightCollision();
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
            Debug.DrawLine(topLeftPoint, bottomLeftPoint, Color.blue, 0.1f);
        }
    }

    private void LeftCollision()
    {
        if (PlayerController.instance.velocity.x <= 0)
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
    }

    private void RightCollision()
    {
        if (PlayerController.instance.velocity.x >= 0)
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
    }

    private void BottomCollision()
    {
        if (PlayerController.instance.velocity.y <= 0)
        {
            RaycastHit2D hit = DetectCollision(bottomLeftPoint, bottomRightPoint, -Vector2.up, distance * 2, numberOfRays);
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

    private void TopCollision()
    {
        if (PlayerController.instance.velocity.y >= 0)
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
    }

    private void DiagonalsRightCollision() 
    {
        RaycastHit2D hitDiagonal = DetectCollision(bottomRightPoint, bottomRightPoint, new Vector2(1, -1), distance, 1);
        Vector2 checkRight = new Vector2(bottomRightPoint.x, bottomRightPoint.y + 0.1f);
        RaycastHit2D hitRight = DetectCollision(checkRight, checkRight, new Vector2(1, 0), distance, 1);
        Vector2 checkDown = new Vector2(bottomRightPoint.x - 0.1f, bottomRightPoint.y);
        RaycastHit2D hitDown = DetectCollision(checkDown, checkDown, new Vector2(0, -1), distance, 1);
        RaycastHit2D hitRightUpper = DetectCollision(topRightPoint, topRightPoint, new Vector2(1, 0), distance, 1);
        RaycastHit2D hitDownLefter = DetectCollision(bottomLeftPoint, bottomLeftPoint, new Vector2(0, -1), distance, 1);
        if (hitDiagonal && hitRight && hitDown && !hitRightUpper && !hitDownLefter)
        {
            Debug.Log("blocked!");
            int i = 4;
            while (i > 1)
            {
                RaycastHit2D hitDiagonalFurther = DetectCollision(new Vector2(bottomRightPoint.x + (distance * i)/2, bottomRightPoint.y - distance*i), new Vector2(bottomRightPoint.x + (distance*i)/2, bottomRightPoint.y - distance), new Vector2(1, -1), distance, 1);
                if (hitDiagonalFurther)
                {
                    break;
                }
                i--;
            }
            playerController.Move(new Vector2(0,1) * (distance*i)/24);
        }
    }
    private void DiagonalsLeftCollision() 
    {
        RaycastHit2D hitDiagonal = DetectCollision(bottomLeftPoint, bottomLeftPoint, new Vector2(-1, -1), distance, 1);
        Vector2 checkLeft = new Vector2(bottomLeftPoint.x, bottomLeftPoint.y + 0.1f);
        RaycastHit2D hitLeft = DetectCollision(checkLeft, checkLeft, new Vector2(-1, 0), distance, 1);
        Vector2 checkDown = new Vector2(bottomLeftPoint.x + 0.1f, bottomLeftPoint.y);
        RaycastHit2D hitDown = DetectCollision(checkDown, checkDown, new Vector2(0, -1), distance, 1);
        RaycastHit2D hitLeftUpper = DetectCollision(topLeftPoint, topLeftPoint, new Vector2(1, 0), distance, 1);
        RaycastHit2D hitDownLefter = DetectCollision(bottomLeftPoint, bottomLeftPoint, new Vector2(0, -1), distance, 1);
        if (hitDiagonal && hitLeft && hitDown && !hitLeftUpper && !hitDownLefter)
            if (hitDiagonal && hitLeft && hitDown)
        {
            int i = 4;
            while (i > 1)
            {
                RaycastHit2D hitDiagonalFurther = DetectCollision(new Vector2(bottomLeftPoint.x - (distance * i)/2, bottomLeftPoint.y - distance * i), new Vector2(bottomLeftPoint.x - (distance * i)/2, bottomLeftPoint.y - distance), new Vector2(1, -1), distance, 1);
                if (hitDiagonalFurther)
                {
                    break;
                }
                i--;
            }
            playerController.Move(new Vector2(0, 1) * (distance*i)/24);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
    }

    Vector2 UpdateCollisions(Vector2 futurePosition) // fonction à tester
    {   //bottomLeftPoint/2 + topLeftPoint/2 = 1/2 * (bottomLeftPoint + topLeftPoint) 
        RaycastHit2D hitDown = DetectCollision(bottomLeftPoint / 2 + topLeftPoint / 2, bottomRightPoint / 2 + topRightPoint / 2, Vector2.down, Vector2.Distance((bottomLeftPoint + topRightPoint) / 2, futurePosition), numberOfRays);
        RaycastHit2D hitUp = DetectCollision(bottomLeftPoint / 2 + topLeftPoint / 2, bottomRightPoint / 2 + topRightPoint / 2, Vector2.up, Vector2.Distance((bottomLeftPoint + topRightPoint) / 2, futurePosition), numberOfRays);
        RaycastHit2D hitLeft = DetectCollision(bottomLeftPoint / 2 + bottomRightPoint / 2, topLeftPoint / 2 + topRightPoint / 2, Vector2.left, Vector2.Distance((bottomLeftPoint + topRightPoint) / 2, futurePosition), numberOfRays); 
        RaycastHit2D hitRight = DetectCollision(bottomLeftPoint / 2 + bottomRightPoint / 2, topLeftPoint / 2 + topRightPoint/2, Vector2.right, Vector2.Distance((bottomLeftPoint + topRightPoint) / 2, futurePosition), numberOfRays);
        Vector2 nextPosition = new Vector2();
        if (hitDown)
        {
            nextPosition.y = hitDown.transform.position.y;
        }
        if (hitUp)
        {
            nextPosition.y = hitUp.transform.position.y;
        }
        if (hitLeft)
        {
            nextPosition.x = hitLeft.transform.position.x;
        }
        if (hitRight)
        {
            nextPosition.x = hitRight.transform.position.x;
        }
        if (!hitLeft && !hitRight)
        {
            nextPosition.x = futurePosition.x;
        }
        if(!hitDown && !hitUp)
        {
            nextPosition.y = futurePosition.y;
        }
        return nextPosition;
    }
}
