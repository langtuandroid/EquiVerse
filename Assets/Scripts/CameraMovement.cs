using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The object around which the camera rotates
    public float rotationSpeed = 13f; // Speed of rotation
    public float zoomSpeed = 13f;

    private new Camera camera;
    private float maxDistance = 2f;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

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
        if (camera.fieldOfView <= maxDistance || camera.fieldOfView >= -maxDistance)
        {
            if (Input.GetKey(KeyCode.W))
            {
                ZoomCamera(-1f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                ZoomCamera(1f);
            }
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

    private void ZoomCamera(float direction)
    {
        float zoomDirection = direction * zoomSpeed * Time.deltaTime;
        camera.fieldOfView += zoomDirection;

    }
}

