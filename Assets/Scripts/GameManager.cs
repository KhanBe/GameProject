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
    public GameObject[] Stages;

    public Image UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public Text DiedCount;
    public Text CoinText;
    public GameObject RestartBoard;

    bool BoardOn = false;

    bool isAlive = true;

    public Text timeText;
    public float second;
    public int minute;
    public int hour;

    public int[] coin = {15, 3, 1, 4, 2, 4};
    public int coinCount = 0;

    public AudioClip audioFinish;
    public AudioClip audioDied;

    AudioSource audioSource;

    public bool isClear = false;

    private GameManager() {}
    //Singleton-----------------
    private static GameManager instance = null;
    public static GameManager Instance {
        get {
            return instance;
        }
    }
    //--------------------------

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }   
        instance = this;
        DontDestroyOnLoad(gameObject);

        count = UIData.Instance.DiedCount;//씬 간 전달된 데이터 표시
        DiedCount.text = "X " + (count);

        stageIndex = UIData.Instance.UIStage;
        UIStage.text = "STAGE " + (stageIndex + 1);

        totalPoint = UIData.Instance.UIPoint;
        UIPoint.text = totalPoint.ToString();

        second = UIData.Instance.second;
        minute = UIData.Instance.minute;
        hour = UIData.Instance.hour;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();//점수

        CoinText.text = coinCount.ToString() + " / " + coin[stageIndex].ToString();//코인상황표

        if (Input.GetKeyDown(KeyCode.R))//R키 눌렀을 시
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !BoardOn)
        {
            ViewButton();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && BoardOn)
        {
            CloseButton();
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
        if(coin[stageIndex] == coinCount)//코인 다모았을 경우
        {
            //소리
            audioSource.clip = audioFinish;
            audioSource.Play();

            Invoke("getNextStage", 0.6f);
        }
        else
        {
            audioSource.clip = audioDied;//sound
            audioSource.Play();

            HealthDown();
        }
    }

    void getNextStage() 
    {
        //stage 변경
        if (stageIndex < Stages.Length - 1)//
        {
            stageIndex++;
            SceneManager.LoadScene(stageIndex + 1);

            PlayerReposition();

            //UI 텍스트 Stage표시
            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else//마지막 스테이지 클리어시
        {
            Time.timeScale = 0;//시간 멈추기

            isClear = true;//클리어유무

            GameObject RestartBoard = gameObject.transform.GetChild(0).gameObject;
            Text btnText = RestartBoard.transform.GetChild(0).GetComponentInChildren<Text>();

            //Text btnText = UIRestartButton.GetComponentInChildren<Text>();
            btnText.text = "Clear!";

            ViewButton();
        }

        totalPoint += stagePoint;
        stagePoint = 0;
        coinCount = 0;
        UIData.Instance.UIStage = stageIndex;
        UIData.Instance.UIPoint = totalPoint;
        UIData.Instance.second = second;
        UIData.Instance.minute = minute;
        UIData.Instance.hour = hour;
    }

    void ViewButton()//UIBoard띄우기
    {
        RestartBoard.SetActive(true);
        BoardOn = true;
    }

    void CloseButton()
    {
        RestartBoard.SetActive(false);
        BoardOn = false;
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
        //죽음
        PlayerMove.Instance.OnDie();

        isAlive = false;

        count++;//죽은 수

        UIData.Instance.DiedCount = count;//죽었을시 UIData에 전달
        UIData.Instance.UIPoint = totalPoint;
        UIData.Instance.UIStage = stageIndex;

        ViewButton();//버튼UI보이기
    }

    void PlayerReposition()
    {
        PlayerMove.Instance.transform.position = new Vector3(0, 0, -1);
        PlayerMove.Instance.VelocityZero();
    }

    public void Restart()//버튼을 눌렀을 경우 함수
    {
        if (isAlive) count++;

        UIData.Instance.DiedCount = count;//살아있는상태에서 R눌러도 count적용
        UIData.Instance.UIStage = stageIndex;

        UIData.Instance.second = second;
        UIData.Instance.minute = minute;
        UIData.Instance.hour = hour;

        UIData.Instance.UIPoint = totalPoint;//추가

        if (stageIndex == 0) SceneManager.LoadScene(1);
        else if (stageIndex == 1) SceneManager.LoadScene(2);
        else if (stageIndex == 2) SceneManager.LoadScene(3);
        else if (stageIndex == 3) SceneManager.LoadScene(4);
        else if (stageIndex == 4) SceneManager.LoadScene(5);
        else if (stageIndex == 5)
        {
            if (isClear) {//클리어시 버튼 눌렀을 경우
                SceneManager.LoadScene(0);
                UIDataReset();
            }           
            else SceneManager.LoadScene(6);//클리어 못했을 경우
        }

        stagePoint = 0;
        DiedCount.text = "X " + (count);

        PlayerReposition();// 처음리스폰
        Time.timeScale = 1;//시간 멈춘거 풀기
   
        isAlive = true;
        PlayerMove.Instance.Revive();

        CloseButton();//버튼UI끄기    
    }

    public void Quit()//Quit버튼 눌렀을 경우 함수
    {
        UIDataReset();
        SceneManager.LoadScene(0);
    }

    void UIDataReset()//데이터 리셋함수
    {
        UIData.Instance.DiedCount = 0;//UI초기화

        UIData.Instance.UIStage = 0;

        UIData.Instance.UIPoint = 0;

        UIData.Instance.second = 0;
        UIData.Instance.minute = 0;
        UIData.Instance.hour = 0;
    }

    IEnumerator waitSec()
    {
        //1초 기다린다.
        Time.timeScale = 0f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
    }
}
