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
                //튜토리얼
                CanvasUI.Instance.ActivateAllChildrenExceptFirst();
                SceneManager.LoadScene(1);
                //불덩이 활성화
                GameManager.Instance.GetComponent<SpawnManager>().enableSpawn = true;
                GameManager.Instance.StageChange(0, 1);
                Debug.Log("새게임");
                break;
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("나가기");
                break;
            case BTNType.Rank:
                //input
                Debug.Log("랭크시스템");
                break;
        }
    }

    public void BtnEnter() {

    }

    public void BtnExit() {

    }

    public void BtnUp() {
        BtnUpSound();
    }

    public void BtnDown() {

    }

    void BtnUpSound() {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }
}