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
    public Toggle screenShakeToggle;
    
    private const string ZoomSpeedKey = "ZoomSpeed";
    private const string XAxisSpeedKey = "XAxisSpeed";
    private const string YAxisSpeedKey = "YAxisSpeed";
    private const string XAxisInvertKey = "XAxisInvert";
    private const string YAxisInvertKey = "YAxisInvert";
    private const string ScreenShakeKey = "ScreenShake";
    
    public static bool screenShakeEnabled = true;

    private void Start()
    {
        cameraMovement.scrollSpeed = PlayerPrefs.GetFloat(ZoomSpeedKey, cameraMovement.scrollSpeed);
        cameraMovement.globalXAxisSpeedScale = PlayerPrefs.GetFloat(XAxisSpeedKey, cameraMovement.globalXAxisSpeedScale);
        cameraMovement.globalYAxisSpeedScale = PlayerPrefs.GetFloat(YAxisSpeedKey, cameraMovement.globalYAxisSpeedScale);
        cameraMovement.xAxisInversedValue = PlayerPrefs.GetInt(XAxisInvertKey, cameraMovement.xAxisInversedValue == -1 ? -1 : 1);
        cameraMovement.yAxisInversedValue = PlayerPrefs.GetInt(YAxisInvertKey, cameraMovement.yAxisInversedValue == -1 ? -1 : 1);
        screenShakeEnabled = PlayerPrefs.GetInt(ScreenShakeKey, screenShakeEnabled ? 1 : 0) == 1;

        zoomMovementSpeedSlider.value = cameraMovement.scrollSpeed;
        xAxisMovementSpeedSlider.value = cameraMovement.globalXAxisSpeedScale;
        yAxisMovementSpeedSlider.value = cameraMovement.globalYAxisSpeedScale;
        xAxisInvertToggle.isOn = cameraMovement.xAxisInversedValue == -1;
        yAxisInvertToggle.isOn = cameraMovement.yAxisInversedValue == -1;
        screenShakeToggle.isOn = screenShakeEnabled;
    }

    public void SetZoomMovementSpeedValue()
    {
        cameraMovement.scrollSpeed = zoomMovementSpeedSlider.value;
        PlayerPrefs.SetFloat(ZoomSpeedKey, cameraMovement.scrollSpeed);
    }

    public void SetXAxisMovementSpeedValue()
    {
        cameraMovement.globalXAxisSpeedScale = xAxisMovementSpeedSlider.value;
        PlayerPrefs.SetFloat(XAxisSpeedKey, cameraMovement.globalXAxisSpeedScale);
    }
    
    public void SetYAxisMovementSpeedValue()
    {
        cameraMovement.globalYAxisSpeedScale = yAxisMovementSpeedSlider.value;
        PlayerPrefs.SetFloat(YAxisSpeedKey, cameraMovement.globalYAxisSpeedScale);
    }

    public void SetXAxisInvertValue()
    {
        cameraMovement.xAxisInversedValue = xAxisInvertToggle.isOn ? -1 : 1;
        PlayerPrefs.SetInt(XAxisInvertKey, cameraMovement.xAxisInversedValue);
    }

    public void SetYAxisInvertValue()
    {
        cameraMovement.yAxisInversedValue = yAxisInvertToggle.isOn ? -1 : 1;
        PlayerPrefs.SetInt(YAxisInvertKey, cameraMovement.yAxisInversedValue);
    }

    public void SetScreenShakeValue()
    {
        screenShakeEnabled = screenShakeToggle.isOn;
        PlayerPrefs.SetInt(ScreenShakeKey, screenShakeEnabled ? 1 : 0);
    }
}