using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Vector2 dashDirection;

    private float timeStartDash;


    [SerializeField] private float positionDisplacement;

    [SerializeField] private float timeFreezeOnDash;
    [SerializeField] private float timeInsideDash;
    [SerializeField] private float timeDecelerationAfterDash;



    private void Start()
    {
        dashDirection = new Vector2(0, 0);
    }

    public void StartDash(Vector2 dashDirection)
    {
        if (PlayerController.instance.canDash)
        {
            if (dashDirection == new Vector2(0, 0))
            {
                dashDirection = Vector2.right;
            }
            PlayerController.instance.onDash = true;
            PlayerController.instance.canDash = false;
            this.dashDirection = dashDirection;
            timeStartDash = Time.time;
        }
    }

    public void UpdateDash()
    {
        float currentTime = Time.time - timeStartDash;

        if (currentTime < timeFreezeOnDash)
        {
            PlayerController.instance.velocity = new Vector2(0, 0);
        }
        else if (currentTime < timeFreezeOnDash + timeInsideDash)
        {
            PlayerController.instance.velocity = GetCurrentDashSpeed() * dashDirection;
        }

        else if (currentTime < timeFreezeOnDash + timeInsideDash + timeDecelerationAfterDash)
        {
            PlayerController.instance.velocity = dashDirection * PlayerController.instance.actualMaxSpeed;
        }

        else
        {
            PlayerController.instance.onDash = false;
        }
    }

    private float GetCurrentDashSpeed()
    {
        float currentDashSpeed = positionDisplacement / timeInsideDash;
        return currentDashSpeed;
    }
}
