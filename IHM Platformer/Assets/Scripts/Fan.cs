using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{

    [SerializeField]
    float maxVelocity;
    [SerializeField]
    float velocityIncrement;
    [SerializeField]
    float fanHeight;
    [SerializeField]
    int numberOfRays;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        Debug.Log("oui");
        RaycastHit2D hit = DetectCollision(new Vector2(transform.position.x - 1, transform.position.y + 0.6f), new Vector2(transform.position.x+1, transform.position.y + 0.6f), Vector2.up, fanHeight, numberOfRays);
     
        if (hit)
        {
            Debug.Log(hit.transform.gameObject);

            if (PlayerController.instance.velocity.y < maxVelocity)
            {
                PlayerController.instance.velocity.y += velocityIncrement;
            }
            
            
        }
    }
    private RaycastHit2D DetectCollision(Vector2 startPoint, Vector2 endPoint, Vector2 direction, float rayDistance, int rayNumber)
    {
        float x = startPoint.x;
        float y = startPoint.y;


        RaycastHit2D hit = new RaycastHit2D();
        for (int i = 0; i < rayNumber; i++)
        {
            x += (endPoint.x - startPoint.x) / (rayNumber + 1);
            y += (endPoint.y - startPoint.y) / (rayNumber + 1);
            hit = Physics2D.Raycast(new Vector2(x, y), direction, rayDistance);
            Debug.DrawLine(new Vector2(x, y), new Vector2(x, y + fanHeight), Color.blue, 0.1f);
            if (hit)
            {
                return hit;
            }
            
        }

        return hit;
    }
    public struct RayInfo
    {
        public Vector2 origin;
        public Vector2 direction;
        public float distance;
    }
}
