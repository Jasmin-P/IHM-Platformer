using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    bool isX;
    [SerializeField]
    float speed;
    [SerializeField]
    float maxDistance;

    Vector2 speed2;
    float distance = 0;
    public Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        speed2 = new Vector2(isX ? speed : 0, isX ? 0 : speed);
        movement = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (maxDistance < distance)
        {
            speed = -speed;
            distance = 0;
        }
        distance += Mathf.Abs(speed) * Time.deltaTime;
        movement.x = isX ? speed * Time.deltaTime : 0;
        movement.y = isX ? 0 : speed * Time.deltaTime;
        transform.position += movement;
    }
}
