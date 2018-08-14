﻿using UnityEngine;

public static class PlayerInput
{
    private static float lookAngle = 0f;
    private static float tiltAngle = 0f;

    //public static Vector3 GetMovementInput(Camera relativeCamera)
    //{
    //    Vector3 moveVector;
    //    float horizontalAxis = Input.GetAxis("Horizontal");
    //    float verticalAxis = Input.GetAxis("Vertical");

    //    if (relativeCamera != null)
    //    {
    //        // Calculate the move vector relative to camera rotation
    //        Vector3 scalerVector = new Vector3(1f, 0f, 1f);
    //        Vector3 cameraForward = Vector3.Scale(relativeCamera.transform.forward, scalerVector).normalized;
    //        Vector3 cameraRight = Vector3.Scale(relativeCamera.transform.right, scalerVector).normalized;

    //        moveVector = (cameraForward * verticalAxis + cameraRight * horizontalAxis);
    //    }
    //    else
    //    {
    //        // Use world relative directions
    //        moveVector = (Vector3.forward * verticalAxis + Vector3.right * horizontalAxis);
    //    }

    //    if (moveVector.magnitude > 1f)
    //    {
    //        moveVector.Normalize();
    //    }

    //    return moveVector;
    //}

    public static Quaternion GetMouseRotationInput(float mouseSensitivity = 3f, float minTiltAngle = -75f, float maxTiltAngle = 45f)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += mouseY * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = MathfExtensions.ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        var controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
        return controlRotation;
    }

    public static bool GetSprint()
    {
        return Input.GetButton("Sprint");
    }

    public static bool GetJump()
    {
        return Input.GetButtonDown("Jump");
    }

    public static bool GetToggleWalk()
    {
        return Input.GetButtonDown("Toggle Walk");
    }

    public static bool GetAttackLeft()
    {
        return Input.GetButtonDown("AttackL");
    }

    public static bool GetAttackRight()
    {
        return Input.GetButtonDown("AttackR");
    }
    public static bool GetCastLeft()
    {
        return Input.GetButtonDown("CastL");
    }

    public static bool GetCastRight()
    {
        return Input.GetButtonDown("CastR");
    }

    public static bool GetDash()
    {
        return Input.GetButtonDown("Dash");
    }

    public static bool GetInteract()
    {
        return Input.GetButtonDown("Interact");
    }

}
