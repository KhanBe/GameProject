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
        //resolutions List에 값 담기
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
                resolutions.Add(Screen.resolutions[i]);
        }
        //역순으로 정렬 1920 x 1080부터 나타내기
        resolutions.Reverse();

        //옵션값 지우기
        resolutionDropdown.options.Clear();

        //
        foreach(Resolution item in resolutions){
            //드롭다운의 옵션데이터
            Dropdown.OptionData option = new Dropdown.OptionData();

            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;

            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();//드롭다운에 값 보여주기

        fullscreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    //옵션눌렀을 경우
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
