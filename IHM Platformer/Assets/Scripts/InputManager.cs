using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;

    private Vector2 direction_raw; //Vecteur non normalisé
    private Vector2 direction; //Vecteur normalisé de la direction dans laquelle on va
    public float deadZoneThreshold;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private float Filter(float f) //Sert à ne faire passer que les valeurs supérieures à deadZoneThreshold
    {
        if (Mathf.Abs(f) > deadZoneThreshold)
        {
            return f;
        }
        else
        {
            return 0f;
        }
    }


    void Update()
    {
        direction_raw = Vector3.right * Filter(Input.GetAxis("Horizontal")) + Vector3.up * Filter(Input.GetAxis("Vertical")) ;
        direction = direction_raw.normalized;

        if (Input.GetButton("A"))
        {
            player.Jump();
        }

        player.MoveX(direction);
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
