using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class GraphicsOptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown screenModeDropdown;

    private List<Resolution> filteredResolutions;
    private Resolution[] resolutions;

    private int currentResolutionIndex = 0;

    // Keys for PlayerPrefs
    private string resolutionWidthKey = "ResolutionWidth";
    private string resolutionHeightKey = "ResolutionHeight";
    private string qualityLevelKey = "QualityLevel";
    private string screenModeKey = "ScreenMode";

    void Start()
    {
        PopulateQualityDropdown();
        PopulateResolutionDropdown();
        PopulateScreenModeDropdown(); 

        screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

        // Check if there are saved settings in PlayerPrefs
        if(PlayerPrefs.HasKey(resolutionWidthKey) && PlayerPrefs.HasKey(resolutionHeightKey) &&
           PlayerPrefs.HasKey(qualityLevelKey) && PlayerPrefs.HasKey(screenModeKey))
        {
            // Load saved settings from PlayerPrefs
            LoadGraphicsSettingsFromPlayerPrefs();
        }
        else
        {
            // Apply default settings
            SetDefaultGraphicsSettings();
        }
    }
    
    void UpdateDropdownValues()
    {
        // Update resolution dropdown
        int savedResolutionIndex = FindCurrentResolutionIndex();
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Update quality dropdown
        int savedQualityLevel = QualitySettings.GetQualityLevel();
        qualityDropdown.value = savedQualityLevel;
        qualityDropdown.RefreshShownValue();

        // Update screen mode dropdown
        screenModeDropdown.value = PlayerPrefs.GetInt(screenModeKey);
    }
    void LoadGraphicsSettingsFromPlayerPrefs()
    {
        int savedScreenWidth = PlayerPrefs.GetInt(resolutionWidthKey);
        int savedScreenHeight = PlayerPrefs.GetInt(resolutionHeightKey);
        int savedQualityLevel = PlayerPrefs.GetInt(qualityLevelKey);
        int savedScreenModeIndex = PlayerPrefs.GetInt(screenModeKey); 

        // Apply saved settings
        Screen.SetResolution(savedScreenWidth, savedScreenHeight, true);
        QualitySettings.SetQualityLevel(savedQualityLevel);
        
        switch (savedScreenModeIndex)
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

        // Update currentResolutionIndex
        currentResolutionIndex = FindCurrentResolutionIndex();

        // Update dropdown values to reflect loaded settings
        UpdateDropdownValues();
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

    void SetDefaultGraphicsSettings()
    {
        int defaultScreenWidth = Screen.currentResolution.width;
        int defaultScreenHeight = Screen.currentResolution.height;

        PlayerPrefs.SetInt(resolutionWidthKey, defaultScreenWidth);
        PlayerPrefs.SetInt(resolutionHeightKey, defaultScreenHeight);

        int highestQualityLevel = QualitySettings.names.Length - 1;
        PlayerPrefs.SetInt(qualityLevelKey, highestQualityLevel);

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        PlayerPrefs.SetInt(screenModeKey, 0);

        Screen.SetResolution(defaultScreenWidth, defaultScreenHeight, true);
        QualitySettings.SetQualityLevel(highestQualityLevel);
        screenModeDropdown.value = 0;

        currentResolutionIndex = FindCurrentResolutionIndex();
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
        PlayerPrefs.SetInt(screenModeKey, index);
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
        PlayerPrefs.SetInt(resolutionWidthKey, selectedResolution.width);
        PlayerPrefs.SetInt(resolutionHeightKey, selectedResolution.height);
    }

    void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(qualityLevelKey, index);
    }

    int FindCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        string currentResolutionString = $"{currentResolution.width}x{currentResolution.height}";
        return filteredResolutions.FindIndex(res => $"{res.width}x{res.height}" == currentResolutionString);
    }
}
