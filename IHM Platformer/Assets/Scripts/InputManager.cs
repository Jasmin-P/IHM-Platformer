using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;

    private Vector2 direction_raw;
    private Vector2 direction;

    public float deadZone;


    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    

    void Update()
    {
        direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal")) + Vector2.up * Filter(Input.GetAxis("Vertical"));
        direction = direction_raw.normalized;


        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.RightKeyPressed();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.LeftkeyPressed();
        }

        if (Input.GetButtonDown("A"))
        {
            if (player.canJump)
            {
                player.Jump();
            }
            else
            {
                StartCoroutine(JumpInputBuffer());
            }
            
        }
    }

    public float Filter(float f)
    {
        if(Mathf.Abs(f) < deadZone)
        {
            return 0f;
        }
        else
        {
            return f;
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

}
