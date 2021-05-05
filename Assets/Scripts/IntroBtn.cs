using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IntroBtn : MonoBehaviour
{
    public BTNType currentType;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.New:
                SceneManager.LoadScene(0);//1스테이지
                Debug.Log("새게임");
                break;
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("나가기");
                break;
        }
    }

}