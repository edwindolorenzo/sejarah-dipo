using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerRideController : PhysicsObject
{
    public float jumpTakeOffSpeed = 7;

    protected override void ComputeVelocity()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            velocity.y = jumpTakeOffSpeed;
        }
    }
}
