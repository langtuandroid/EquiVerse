using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown screenModeDropdown;

    void Start()
    {
        PopulateQualityDropdown();
        PopulateResolutionDropdown();
        PopulateScreenModeDropdown();

        screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);
    }

    void PopulateResolutionDropdown()
    {
        var resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height} [{resolutions[i].refreshRate}Hz]";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateQualityDropdown()
    {
        string[] qualityLevels = QualitySettings.names;
        qualityDropdown.ClearOptions();

        List<string> qualityOptions = new List<string>(qualityLevels);
        qualityDropdown.AddOptions(qualityOptions);

        int currentQualityLevel = QualitySettings.GetQualityLevel();
        qualityDropdown.value = currentQualityLevel;
    }

    void PopulateScreenModeDropdown()
    {
        screenModeDropdown.ClearOptions();

        List<string> screenModeOptions = new List<string>
        {
            "Full Screen",
            "Windowed",
            "Borderless"
        };

        screenModeDropdown.AddOptions(screenModeOptions);
        SetDropdownValueBasedOnCurrentScreenMode();
    }

    void OnScreenModeChanged(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            default:
                break;
        }
    }

    void SetDropdownValueBasedOnCurrentScreenMode()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                screenModeDropdown.value = 0;
                break;
            case FullScreenMode.Windowed:
                screenModeDropdown.value = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                screenModeDropdown.value = 2;
                break;
            default:
                break;
        }
    }

    public void ApplyChanges()
    {
        OnResolutionChanged(resolutionDropdown.value);
        OnQualityChanged(qualityDropdown.value);
    }

    public void OnResolutionChanged(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}


