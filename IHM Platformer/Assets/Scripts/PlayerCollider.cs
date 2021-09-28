using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    Collider2D collider;
    PlayerController playerController;
    Bounds bounds;

    public LayerMask colliderMask;

    public Vector2 bottomLeftPoint;
    public Vector2 bottomRightPoint;
    public Vector2 topLeftPoint;
    public Vector2 topRightPoint;

    public int numberOfRays = 5;
    public float distance = 0.1f;

    private List<RayInfo> rayList;



    public float collisionDistance;

    public bool debug = true;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        playerController = GetComponent<PlayerController>();
        if (!collider)
        {
            Debug.LogError("collider in PlayerCollider is an empty object");
        }

        UpdateBounds();
        InitBottomRaycasts(numberOfRays, distance);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBounds();
        CheckBottomRaycast();
    }

    private void UpdateBounds()
    {
        bounds = collider.bounds;
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

    private void InitBottomRaycasts(int numberOfRay, float distance)
    {
        rayList = new List<RayInfo>();
        float x1 = bottomLeftPoint.x;
        float x2 = bottomRightPoint.x;
        float y = bottomLeftPoint.y;
        float x = x1;

        for(int i = 0; i < numberOfRay; i++)
        {
            RayInfo newRay = new RayInfo();
            newRay.origin = new Vector2(x, y);
            newRay.direction = new Vector2(0, -1);
            newRay.distance = distance;
            rayList.Add(newRay);

            x += (x2 - x1) / (numberOfRay-1);
        }
    }

    private void CheckBottomRaycast()
    {
        bool rayhit = false;

        for (int i = 0; i < rayList.Count; i++)
        {
            RayInfo ray = rayList[i];
            ray.origin.y = bottomLeftPoint.y;
            

            RaycastHit2D hit = Raycast(ray);
            if (hit)
            {
                Debug.DrawLine(ray.origin + Vector2.left, ray.origin + Vector2.left -Vector2.up * hit.distance, Color.black, 30f);
                if (!playerController.bottomDirectionLocked)
                {
                    playerController.Move(-Vector2.up * hit.distance);
                    playerController.bottomDirectionLocked = true;
                }
                
                rayhit = true;
                break;
            }
        }

        if (!rayhit)
        {
            playerController.bottomDirectionLocked = false;
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
