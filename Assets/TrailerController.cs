using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TrailerController : MonoBehaviour
{
    public CinemachineFreeLook mainFreeLookCamera;
    public GameObject gameUI;
    public GameObject levelText;

    private bool disabledGameUI = false;
    
    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            if (mainFreeLookCamera.m_XAxis.m_MaxSpeed == 20f)
            {
                mainFreeLookCamera.m_XAxis.m_MaxSpeed = 100f;
            }
            else
            {
                mainFreeLookCamera.m_XAxis.m_MaxSpeed = 20f;
            }
        }
        
        if (UnityEngine.Input.GetKeyDown(KeyCode.O))
        {
            if (!disabledGameUI)
            {
                levelText.SetActive(false);
                gameUI.SetActive(false);
                disabledGameUI = true;
            }
            else
            {
                levelText.SetActive(true);
                gameUI.SetActive(true);
                disabledGameUI = false;
            }
        }
    }
}
