using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;


    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    

    void Update()
    {


        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.RightKeyPressed();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.LeftkeyPressed();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
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
