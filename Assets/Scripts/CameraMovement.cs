using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The object around which the camera rotates
    public float rotationSpeed = 10f; // Speed of rotation

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateCamera(-1f); // Rotate left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateCamera(1f); // Rotate right
        }
    }

    private void RotateCamera(float direction)
    {
        Vector3 yAxis = Vector3.up; // Axis of rotation (vertical axis)

        // Calculate the desired rotation angle based on the direction and speed
        float angle = direction * rotationSpeed * Time.deltaTime;

        // Rotate the camera around the target object
        transform.RotateAround(target.position, yAxis, angle);
    }
}
