using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    Collider2D playerCollider;
    Bounds bounds;

    public Vector2 bottomLeftPoint;
    public Vector2 bottomRightPoint;
    public Vector2 topLeftPoint;
    public Vector2 topRightPoint;

    public int numberOfRays = 3;
    public float distanceRaycast = 0.1f;


    private float diagonalHitMovement = 0.001f;

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

        UpdateBounds();

        mask = LayerMask.GetMask("Wall");
    }


    private void UpdateBounds()
    {
        bounds = playerCollider.bounds;
        bounds.Expand(-collisionDistance);
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
            PlayerController.instance.Move(new Vector2(0,1) * (distanceRaycast * i)/24);
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
            PlayerController.instance.Move(new Vector2(0, 1) * (distanceRaycast * i)/24);
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

    private RaycastHit2D DetectCollision(Vector2 origin, Vector2 direction, float rayDistance)
    {
        RayInfo ray;
        ray.origin = origin;
        ray.direction = direction;
        ray.distance = rayDistance;
        RaycastHit2D hit = Raycast(ray);

        return hit;
    }


    private void CanJump()
    {
        Vector2 startPoint = bottomLeftPoint;
        Vector2 endPoint = bottomRightPoint;

        RaycastHit2D hit = DetectCollision(startPoint, endPoint, -Vector2.up, yJumpFlexibility, jumpNumberOfRays);


        if (hit)
        {

            PlayerController.instance.GroundTouched();
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

        RaycastHit2D hitMovingPlatformDown = new RaycastHit2D();
        RaycastHit2D hitMovingPlatformLeft = new RaycastHit2D();
        RaycastHit2D hitMovingPlatformRight = new RaycastHit2D();
        RaycastHit2D hitDown = new RaycastHit2D();
        RaycastHit2D hitUp = new RaycastHit2D();
        RaycastHit2D hitLeft = new RaycastHit2D();
        RaycastHit2D hitRight = new RaycastHit2D();

        hitMovingPlatformDown = DetectCollision(bottomLeftPoint, bottomRightPoint, Vector2.down, Vector2.Dot(Vector2.down, movement), numberOfRays);
        hitMovingPlatformLeft = DetectCollision(bottomLeftPoint, topLeftPoint, Vector2.left, -movement.x, numberOfRays);
        hitMovingPlatformRight = DetectCollision(bottomRightPoint, topRightPoint, Vector2.right, movement.x, numberOfRays);

        if (hitMovingPlatformDown && hitMovingPlatformDown.collider.CompareTag("MovingPlatform")) 
        {
            Debug.Log("moving on !");
            float movementX = hitMovingPlatformDown.collider.GetComponent<MovingPlatform>().movement.x;
            float movementY = hitMovingPlatformDown.collider.GetComponent<MovingPlatform>().movement.y;
            movement.x += movementX;
            movement.y += movementY;
        }
        if (hitMovingPlatformLeft && hitMovingPlatformLeft.collider.CompareTag("MovingPlatform"))
        {
        }
        //if (hitMovingPlatformRight && hitMovingPlatformRight.collider.CompareTag("MovingPlatform"))
        //{
        //    movement.x += Vector2.Dot(movement, Vector2.left);
        //    movement.y += Vector2.Dot(movement, Vector2.down);
        //}

        if (movement.y < 0)
        {
            PlayerController.instance.bottomDirectionLocked = false;
            hitDown = DetectCollision(bottomLeftPoint, bottomRightPoint, Vector2.down, Vector2.Dot(Vector2.down, movement), numberOfRays);
        }
        else
        {
            PlayerController.instance.topDirectionLocked = false;
            hitUp = DetectCollision(topRightPoint, topLeftPoint, Vector2.up, movement.y, numberOfRays);
        }

        if (movement.x < 0)
        {
            PlayerController.instance.leftDirectionLocked = false;
            hitLeft = DetectCollision(bottomLeftPoint, topLeftPoint, Vector2.left, -movement.x, numberOfRays);
        }
        else
        {
            PlayerController.instance.rightDirectionLocked = false;
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
            Debug.Log("hit!");
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

        if (!(hitDown || hitLeft || hitRight || hitUp))
        {
            UpdateDiagonalCollisions(ref movement);
        }
        
    }

    public void UpdateDiagonalCollisions(ref Vector2 movement)
    {
        RaycastHit2D hit;
        Vector2 direction = movement.normalized;
        float length = movement.magnitude;
        bool left = movement.x < 0;
        bool down = movement.y < 0;

        if (left){
            if (down)
            {
                hit = DetectCollision(bottomLeftPoint, direction, length);
            }
            else
            {
                hit = DetectCollision(topLeftPoint, direction, length);
            }
        }
        else
        {
            if (movement.y < 0)
            {
                hit = DetectCollision(bottomRightPoint, direction, length);
            }
            else
            {
                hit = DetectCollision(topRightPoint, direction, length);
            }
        }

        if (hit)
        {


            movement += (hit.distance - length) * direction;
            PlayerController.instance.velocity.y = 0;
            PlayerController.instance.velocity.x = 0;


            if (down)
            {
                Bounds hitBounds = hit.collider.bounds;

                Vector2 newPos;

                if (left)
                {
                    newPos = new Vector2(bounds.min.x + diagonalHitMovement, bounds.max.y);
                }
                else
                {
                    newPos = new Vector2(bounds.max.x - diagonalHitMovement, bounds.max.y);
                }

                movement = newPos - new Vector2(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y);
            }
            

        }
    }
}
