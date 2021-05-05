using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public int count = 0;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image UIHealth;
    public Text UIPoint;//
    public Text UIStage;//
    public Text DiedCount;//
    public GameObject UIRestartButton;
    bool isAlive = true;

    public Text timeText;
    public float second;
    public int minute;
    public int hour;

    public bool isClear = false;

    private void Awake()
    {
        count = UIData.instanceData.DiedCount;//씬 간 전달된 데이터 표시
        DiedCount.text = "X " + (count);

        stageIndex = UIData.instanceData.UIStage;
        UIStage.text = "STAGE " + (stageIndex + 1);

        totalPoint = UIData.instanceData.UIPoint;
        UIPoint.text = totalPoint.ToString();

        second = UIData.instanceData.second;
        minute = UIData.instanceData.minute;
        hour = UIData.instanceData.hour;
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();//점수

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isAlive) count++;

            Restart();
        }

        //시간
        second += Time.deltaTime;

        if (second >= 60)
        {
            second = 0;
            minute++;
        }
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }
        timeText.text = hour.ToString() + ":" + minute.ToString() + ":" + Mathf.Round(second).ToString();
    }

    public void NextStage()
    {
        //stage 변경
        if(stageIndex < Stages.Length - 1)//
        {
            //다음 스테이지 변경
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            //UI 텍스트 Stage표시
            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else//마지막 스테이지 클리어시
        {
            Time.timeScale = 0;//시간 멈추기

            isClear = true;//클리어유무

            Text btnText = UIRestartButton.GetComponentInChildren<Text>();
            btnText.text = "Clear!";

            ViewButton();
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    void ViewButton()
    {
        UIRestartButton.SetActive(true);
    }

    void CloseButton()
    {
        UIRestartButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)//낙하 충돌체
    {
        if (collision.gameObject.tag == "Player")
        {   
            HealthDown();
        }
    }

    public void HealthDown()
    {
        player.OnDie();//죽음

        isAlive = false;

        count++;//죽은 수

        UIData.instanceData.DiedCount = count;//죽었을시 UIData에 전달
        UIData.instanceData.UIPoint = totalPoint;
        UIData.instanceData.UIStage = stageIndex;

        ViewButton();//버튼UI보이기
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()//버튼을 눌렀을 경우 함수
    {
        UIData.instanceData.DiedCount = count;//살아있는상태에서 R눌러도 count적용

        UIData.instanceData.second = second;
        UIData.instanceData.minute = minute;
        UIData.instanceData.hour = hour;

        if (stageIndex == 0) SceneManager.LoadScene(0);
        else if (stageIndex == 1) SceneManager.LoadScene(1);
        else if (stageIndex == 2) SceneManager.LoadScene(2);
        else if (stageIndex == 3) SceneManager.LoadScene(3);
        else if (stageIndex == 4)
        {
            if (isClear) {
                SceneManager.LoadScene(5);
                UIData.instanceData.DiedCount=0;//UI초기화


                UIData.instanceData.UIStage = 0;


                UIData.instanceData.UIPoint = 0;


                UIData.instanceData.second = 0;
                UIData.instanceData.minute = 0;
                UIData.instanceData.hour = 0;
            }
            
            else SceneManager.LoadScene(4);
        }

        stagePoint = 0;

        DiedCount.text = "X " + (count);

        PlayerReposition();// 처음리스폰

        Time.timeScale = 1;//시간 멈춘거 풀기

        
        isAlive = true;//
        
        CloseButton();//버튼UI끄기
    }
}
