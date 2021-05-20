using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BTNType
{
    New,
    Quit
}
public class MainUI : MonoBehaviour
{
    FullScreenMode screenMode;

    public Toggle fullscreenToggle;

    public Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();

    public int resolutionNum;

    int optionNum = 0;

    private void Awake()
    {
        InitUI();
    }
    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 120 || Screen.resolutions[i].refreshRate == 60|| Screen.resolutions[i].refreshRate == 239)
                resolutions.Add(Screen.resolutions[i]);
        }

        resolutions.Reverse();

        resolutionDropdown.options.Clear();

        foreach(Resolution item in resolutions){

            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
