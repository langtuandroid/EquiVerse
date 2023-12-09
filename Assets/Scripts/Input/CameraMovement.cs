using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The object around which the camera rotates
    public float rotationSpeed = 13f; // Speed of rotation
    public float zoomSpeed = 13f;
    public float maxDistance = 80f;
    public float minDistance = 20f; // Added minimum zoom distance
    private bool movedLeft = false, movedRight = false, movedUp = false, movedDown = false;
    
    [HideInInspector]
    public static bool cameraLocked = true;

    public static bool cameraMovedInAllDirections = false;

    private new Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
        cameraMovedInAllDirections = false;
        cameraLocked = true;
    }

    private void Update()
    {
        if (!cameraLocked)
        {
            MoveCamera();
            CheckCameraTutorial();
        }
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateCamera(1f); // Rotate left
            movedLeft = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateCamera(-1f); // Rotate right
            movedRight = true;
        }
        
        // Clamp zoom within the defined limits
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minDistance, maxDistance);

        if (camera.fieldOfView <= maxDistance && camera.fieldOfView >= minDistance)
        {
            if (Input.GetKey(KeyCode.W))
            {
                ZoomCamera(-1f);
                movedUp = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                ZoomCamera(1f);
                movedDown = true;
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

    private void CheckCameraTutorial()
    {
        if (movedLeft && movedRight && movedUp && movedDown)
        {
            cameraMovedInAllDirections = true;
        }
    }
}


