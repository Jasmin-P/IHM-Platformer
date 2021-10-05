using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;

    private Vector2 direction_raw;
    private Vector2 direction;
    public float deadZoneThreshold = 0.5f;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    

    void Update()
    {
        direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal")) + Vector2.up * Filter(Input.GetAxis("Vertical"));
        direction = direction_raw.normalized;

        player.MoveX(direction);

        if (Input.GetButtonDown("A"))
        {
            player.Jump();
        }
    }


    IEnumerator JumpInputBuffer()
    {
        int j = 20;
        bool end = false;
        while(j > 0 && !end)
        {
            if (player.canJump)
            {
                player.Jump();
                end = true;
            }
            j--;
            yield return new WaitForEndOfFrame();
        }
        
        yield return null;
    }

    private float Filter(float f)
    {
        if(f < deadZoneThreshold)
        {
            return 0f;
        }
        else
        {
            return f;
        }
    }

}
