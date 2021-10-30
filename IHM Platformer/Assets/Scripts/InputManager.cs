using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;

    private Vector2 direction_raw;
    private Vector2 direction;

    public float deadZone;

    public CameraShaker cameraShaker;


    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    

    void Update()
    {
        direction_raw = Vector2.right * Filter(Input.GetAxis("Horizontal")) + Vector2.up * Filter(Input.GetAxis("Vertical"));
        direction = direction_raw.normalized;

        if (direction.x > 0)
        {
            player.RightKeyPressed();
        }
        else if (direction.x < 0)
        {
            player.LeftkeyPressed();
        }


        /*
        var input = Input.inputString;

        switch (input)
        {
            case ("joystick button 0"):
                if (player.canJump)
                {
                    player.Jump();
                }
                else
                {
                    StartCoroutine(JumpInputBuffer());
                }
                break;
            case ("X"):
                player.Dash(direction);
                break;
            case ("joystick button 1"):
                //player.Sprint()
                print("B pressed");
                break;
            default:
                break;
        }

        */

        

        if (Input.GetButtonDown("A"))
        {
            player.Jump();
            StartCoroutine(cameraShaker.Shake(0.2f, 0.1f));
        }
        if (Input.GetButtonDown("X"))
        {
            player.Dash(direction);
            StartCoroutine(cameraShaker.Shake(0.2f, 0.3f));
        }
        if (Input.GetButtonDown("B"))
        {
            print("B pressed");
            player.Sprint();
        }
        if (Input.GetButtonUp("B"))
        {
            print("B released");
            player.StopSprinting();
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
            if (player.jumpCount > 0)
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
