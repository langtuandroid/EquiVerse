using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class OptionsMenu : MonoBehaviour
    {
        public TMP_Dropdown resolutionDropdown;
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

        void PopulateResolutionDropdown()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
        
            resolutionDropdown.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRate;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRate == currentRefreshRate)
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }

            List<string> options = new List<string>();
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " +
                                          filteredResolutions[i].refreshRate + "Hz";
                options.Add(resolutionOption);
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
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
            Resolution selectedResolution = filteredResolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, true);
        }

        public void OnQualityChanged(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }
    }
}


