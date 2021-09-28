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
            player.Jump();
        }
    }

}
