using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    public Transform startingPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        collision.gameObject.transform.position = startingPoint.position;

        collision.gameObject.GetComponent<PlayerController>().DieAnimation();
    }
}
