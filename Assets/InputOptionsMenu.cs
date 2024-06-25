using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.UI;

public class InputOptionsMenu : MonoBehaviour
{
    public CameraMovement cameraMovement;
    
    public Slider zoomMovementSpeedSlider;
    public Slider xAxisMovementSpeedSlider;
    public Slider yAxisMovementSpeedSlider;
    public Toggle xAxisInvertToggle;
    public Toggle yAxisInvertToggle;

    private void Start()
    {
        zoomMovementSpeedSlider.value = cameraMovement.scrollSpeed;
        xAxisMovementSpeedSlider.value = cameraMovement.globalXAxisSpeedScale;
        yAxisMovementSpeedSlider.value = cameraMovement.globalYAxisSpeedScale;
        xAxisInvertToggle.isOn = cameraMovement.mainCam.m_XAxis.m_InvertInput;
        yAxisInvertToggle.isOn = cameraMovement.mainCam.m_YAxis.m_InvertInput;
    }

    public void SetZoomMovementSpeedValue()
    {
        cameraMovement.scrollSpeed = zoomMovementSpeedSlider.value;
    }

    public void SetXAxisMovementSpeedValue()
    {
        cameraMovement.globalXAxisSpeedScale = xAxisMovementSpeedSlider.value;
    }
    
    public void SetYAxisMovementSpeedValue()
    {
        cameraMovement.globalYAxisSpeedScale = yAxisMovementSpeedSlider.value;
    }

    public void SetXAxisInvertValue()
    {
        if (cameraMovement.xAxisInversedValue == 1)
        {
            cameraMovement.xAxisInversedValue = -1;
        }
        else
        {
            cameraMovement.xAxisInversedValue = 1;
        }
    }

    public void SetYAxisInvertValue()
    {
        if (cameraMovement.yAxisInversedValue == 1)
        {
            cameraMovement.yAxisInversedValue = -1;
        }
        else
        {
            cameraMovement.yAxisInversedValue = 1; 
        }
    }
}
