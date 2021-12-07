using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * Gère le premier jump, le second jump et le wall jump
 * 
 */

public class Jump : MonoBehaviour
{


    [SerializeField] private float jumpForce;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private Vector2 wallJumpForce;
    private float variableJumpForce;

    private bool onFirstJump;
    private bool onSecondJump;
    private bool onWallJump;
    private bool jumpReleased;

    private float jumpCount;

    private float timeStartJump;
    [SerializeField] private float timeDurationJump;




    // Appelé lorsque la touche de jump est préssée. Decide du jump à faire et le lance.
    public bool StartJump(float playerControllerJumpCount, bool wallJumpPossible)
    {
        jumpCount = playerControllerJumpCount;

        if (jumpCount == 2)
        {
            StartFirstJump();
            return true;
        }

        else if (wallJumpPossible && !onWallJump)
        {
            StartWallJump();
            return true;
        }

        else if (jumpCount == 1)
        {
            StartSecondJump();
            return true;
        }
        return false;
    }

    // Appelé à chaque frame par le playerController pendant un jump (onJump). Lance un Update du jump actuel.
    public void UpdateJump()
    {
        if (onFirstJump)
        {
            UpdateFirstJump();
        }
        else if (onSecondJump)
        {
            UpdateSecondJump();
        }
        else if (onWallJump)
        {
            UpdateWallJump();
        }
        else
        {
            PlayerController.instance.onJump = false;
        }
    }

    // Appelé lorsque la touche de jump est relachée. Indique qu'il faut modifier la taille du premier jump.
    public void JumpRelease()
    {
        jumpReleased = true;
    }


    /*
     *  StartFirstJump, StartSecondJump, StartWallJUmp
     *  lancent leurs jump respectifs.
     */
    private void StartFirstJump()
    {
        PlayerController.instance.jumpCount--;
        PlayerController.instance.onJump = true;

        jumpCount--;
        onFirstJump = true;
        jumpReleased = false;
        PlayerController.instance.velocity.y = jumpForce;
        variableJumpForce = jumpForce;
        timeStartJump = Time.time;
    }

    private void StartSecondJump()
    {
        PlayerController.instance.jumpCount--;
        PlayerController.instance.onJump = true;

        jumpCount--;
        onFirstJump = false;
        onWallJump = false;
        onSecondJump = true;
        timeStartJump = Time.time;

        StartCoroutine("SecondJumpAnimation");
    }

    IEnumerator SecondJumpAnimation()
    {
        float timeCount = 0f; //On veut une full rotation en 1 saut
        while (onSecondJump)
        {
            //On va prendre le gameobject auquel est rattaché le sprite renderer, cad le sprite tout seul, et on le fait tourner
            float dt = Time.deltaTime;
            PlayerController.instance.spriteRenderer.gameObject.transform.rotation = Quaternion.SlerpUnclamped(Quaternion.identity,Quaternion.Euler(0,0,Mathf.Sign(PlayerController.instance.velocity.x)*180),timeCount*8);
            timeCount += dt;
            yield return new WaitForSeconds(dt);
        }
        PlayerController.instance.spriteRenderer.gameObject.transform.rotation = Quaternion.identity;



    }

    private void StartWallJump()
    {
        PlayerController.instance.onJump = true;

        if (PlayerController.instance.grabDirection.x >= 0)
        {
            wallJumpDirection = new Vector2(-1, 0);
        }
        else
        {
            wallJumpDirection = new Vector2(1, 0);
        }
        onWallJump = true;
        onFirstJump = false;
        onSecondJump = false;
        timeStartJump = Time.time;
    }


    /*
     *  UpdateFirstJump, UpdateSecondJump, UpdateWallJUmp
     *  actualisent leur jump respectif et vérifie si il est terminé.
     */
    private void UpdateFirstJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onFirstJump = false;
            PlayerController.instance.onJump = false;
            jumpReleased = false;
        }
        else
        {
            PlayerController.instance.velocity.y = variableJumpForce;
        }

        if (jumpReleased && Time.time - timeStartJump > timeDurationJump * 0.5f)
        {
            variableJumpForce = jumpForce * 0.5f;
        }
    }

    private void UpdateSecondJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onSecondJump = false;
            PlayerController.instance.onJump = false;
            jumpReleased = false;
        }
        else
        {
            PlayerController.instance.velocity.y = jumpForce * 0.7f;
        }
    }

    private void UpdateWallJump()
    {
        if (Time.time - timeStartJump > timeDurationJump)
        {
            onWallJump = false;
            PlayerController.instance.onJump = false;
            jumpReleased = false;
        }
        else
        {
            PlayerController.instance.velocity = new Vector2(wallJumpDirection.x * wallJumpForce.x, wallJumpForce.y);
        }
    }
}
