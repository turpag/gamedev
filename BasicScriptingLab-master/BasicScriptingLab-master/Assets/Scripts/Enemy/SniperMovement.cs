﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMovement : EnemyMovement
{

    /* The Start function first finds the player by calling the base class's Start method. Then, the start below tracks various 
     * components of the target in preparation for the FirstOrderIntercept function, which tracks down the player and then shoots
     * our sniper in the direction of the player.
     */
    protected override void Start()
    {
        base.Start();

        Vector2 targetVel = target.GetComponent<PlayerMovement>().estimatedVelocity;
        Vector2 targetPos = target.transform.position;
        Vector2 myPos = transform.position;

        Vector2 interceptPoint = FirstOrderIntercept(myPos, speed, targetPos, targetVel);
        gameObject.GetComponent<Rigidbody2D>().velocity = (interceptPoint - myPos).normalized * speed;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Math below borrowed from: http://wiki.unity3d.com/index.php/Calculating_Lead_For_Projectiles //
    // Slight modifications were made to remove the need for shooterVelocity and use Vector2              //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    //First-order intercept using absolute target position
    public static Vector2 FirstOrderIntercept
    (
        Vector2 shooterPosition,
        float shotSpeed,
        Vector2 targetPosition,
        Vector2 targetVelocity
    )
    {
        Vector2 targetRelativePosition = targetPosition - shooterPosition;
        Vector2 targetRelativeVelocity = targetVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //First-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector2 targetRelativePosition,
        Vector2 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //Handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector2.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //Don't shoot back in time
        }

        float b = 2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //Determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //Both are positive
                else
                    return t1; //Only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //Don't shoot back in time
        }
        else if (determinant < 0f) //Determinant < 0; no intercept path
            return 0f;
        else //Determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //Don't shoot back in time
    }
}
