using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown screenModeDropdown;

    private List<Resolution> filteredResolutions;
    private Resolution[] resolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Start()
    {
        PopulateQualityDropdown();
        PopulateResolutionDropdown();
        PopulateScreenModeDropdown();

        screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);
    }

    //Erwin: Dit is mijn eigen code maar goed, toch een puntje van verbetering gevonden! Er zijn een aantal dingen die hier dubbel gebeuren. Je zou een functie kunnen maken die dit 1 keer doet waardoor de readability omhoog gaat.
        void PopuplateDropdown(TMP_Dropdown dropdown, List<string> list, int currentIndex) {
        dropdown.ClearOptions();
        resolutionDropdown.AddOptions(list);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        HashSet<int> uniqueRefreshRates = new HashSet<int>();

        resolutionDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions(); 
        currentRefreshRate = Screen.currentResolution.refreshRate;

        foreach (var res in resolutions) {
            uniqueRefreshRates.Add(res.refreshRate);
            if (res.refreshRate == currentRefreshRate) {
                filteredResolutions.Add(res);
            }
        }

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++) {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            resolutionOptions.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        List<string> refreshRateOptions = new List<string>();
        List<int> sortedRefreshRates = uniqueRefreshRates.ToList();
        sortedRefreshRates.Sort();

        foreach (int rate in sortedRefreshRates) {
            refreshRateOptions.Add(rate + "Hz");
        }

        refreshRateDropdown.AddOptions(refreshRateOptions);
        refreshRateDropdown.value = refreshRateDropdown.options.FindIndex(option => option.text == currentRefreshRate + "Hz");
        refreshRateDropdown.RefreshShownValue();
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
        Resolution selectedResolution = filteredResolutions[index];

        string selectedRefreshRateString = refreshRateDropdown.options[refreshRateDropdown.value].text;
        int selectedRefreshRate = int.Parse(selectedRefreshRateString.Replace("Hz", ""));


        Screen.SetResolution(selectedResolution.width, selectedResolution.height, true, selectedRefreshRate );
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}


