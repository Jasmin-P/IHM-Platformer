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

        if (direction.x > 0)
        {
            player.RightKeyPressed();
        }
        else if (direction.x < 0)
        {
            player.LeftkeyPressed();
        }



        player.Grab(direction);


        if (Filter(Input.GetAxis("Dash")) > 0.5f || Input.GetButtonDown("X"))
        {
            Vector2 dashDirection = new Vector2(direction.x, direction.y);

            if (Mathf.Abs(dashDirection.x) > 0.5)
            {
                dashDirection.x = Mathf.Sign(dashDirection.x);
            }
            else
            {
                dashDirection.x = 0;
            }

            if (Mathf.Abs(dashDirection.y) > 0.5)
            {
                dashDirection.y = Mathf.Sign(dashDirection.y);
            }
            else
            {
                dashDirection.y = 0;
            }

            player.Dash(dashDirection.normalized);
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
        }
        if (Input.GetButtonUp("A"))
        {
            player.JumpRelease();
        }

        /*
        if (Input.GetButtonDown("X"))
        {
            player.Dash(direction);
        }
        */

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
        if (Input.GetButtonDown("Plus"))
        {
            PauseController.Instance.Pause();
            Debug.Log("Pause!");
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
