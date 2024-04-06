using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class GraphicsOptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown screenModeDropdown;
    //public TMP_Dropdown frameRateDropdown;

    private List<Resolution> filteredResolutions;
    private Resolution[] resolutions;

    private int currentResolutionIndex = 0;

    void Start()
    {
        PopulateQualityDropdown();
        PopulateResolutionDropdown();
        PopulateScreenModeDropdown(); 
        //PopulateFrameRateDropdown();

        screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        //frameRateDropdown.onValueChanged.AddListener(OnFrameRateChanged);

        SetDefaultResolution();
    }

    void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        HashSet<string> uniqueResolutions = new HashSet<string>();

        resolutionDropdown.ClearOptions();
        currentResolutionIndex = FindCurrentResolutionIndex();

        foreach (var res in resolutions)
        {
            string resolutionString = $"{res.width}x{res.height}";

            if (uniqueResolutions.Add(resolutionString))
            {
                filteredResolutions.Add(res);
            }
        }

        List<string> resolutionOptions = filteredResolutions.Select(res => $"{res.width}x{res.height}").ToList();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void SetDefaultResolution()
    {
        Resolution currentResolution = Screen.currentResolution;
        string currentResolutionString = $"{currentResolution.width}x{currentResolution.height}";
        int defaultIndex = filteredResolutions.FindIndex(res => $"{res.width}x{res.height}" == currentResolutionString);

        if (defaultIndex != -1)
        {
            resolutionDropdown.value = defaultIndex;
            resolutionDropdown.RefreshShownValue();
        }
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

    // void PopulateFrameRateDropdown()
    // {
    //     frameRateDropdown.ClearOptions();
    //
    //     List<string> frameRateOptions = new List<string>
    //     {
    //         "30 FPS",
    //         "60 FPS",
    //         "120 FPS"
    //     };
    //
    //     frameRateDropdown.AddOptions(frameRateOptions);
    //     frameRateDropdown.value = frameRateOptions.IndexOf($"{Application.targetFrameRate} FPS");
    //     frameRateDropdown.RefreshShownValue();
    // }

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

    void OnResolutionChanged(int index)
    {
        Resolution selectedResolution = filteredResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, true);
    }

    void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // void OnFrameRateChanged(int index)
    // {
    //     switch (index)
    //     {
    //         case 0:
    //             Application.targetFrameRate = 30;
    //             break;
    //         case 1:
    //             Application.targetFrameRate = 60;
    //             break;
    //         case 2:
    //             Application.targetFrameRate = 120;
    //             break;
    //         // Add more options as needed
    //         default:
    //             break;
    //     }
    // }

    int FindCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        string currentResolutionString = $"{currentResolution.width}x{currentResolution.height}";
        return filteredResolutions.FindIndex(res => $"{res.width}x{res.height}" == currentResolutionString);
    }
}
