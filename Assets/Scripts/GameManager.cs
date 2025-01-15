using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{                           
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int DeathCount = 0;

    public GameObject[] Stages;
    public int FinalStage = 5;
    public int PlayStage = 1;

    public Image UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public Text DiedCount;
    public Text CoinText;
    public GameObject RestartBoard;
    public GameObject RankManager;

    bool BoardOn = false;

    public Text timeText;
    public float second;
    public int minute;
    public int hour;

    public int[] coin = {};
    public int coinCount = 0;

    public bool gameMode = false;
    public bool isClear = false;

    private GameManager() {}

    private static GameManager instance = null;
    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        FinalStage = Stages.Length - 1;


        DeathCount = UIData.Instance.DiedCount;//씬 간 전달된 데이터 표시
        DiedCount.text = "X " + DeathCount;

        stageIndex = UIData.Instance.UIStage;
        UIStage.text = "STAGE " + stageIndex;

        totalPoint = UIData.Instance.UIPoint;
        UIPoint.text = totalPoint.ToString();

        second = UIData.Instance.second;
        minute = UIData.Instance.minute;
        hour = UIData.Instance.hour;
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();//점수

        CoinText.text = coinCount.ToString() + " / " + coin[stageIndex].ToString();//코인상황표

        if (Input.GetKeyDown(KeyCode.R) && gameMode)//R키 눌렀을 시
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

    public void ShowSubmitboard()
    {   
        GameObject submitboardPanel = RankManager.GetComponent<RankManager>().submitboardPanel;
        if (submitboardPanel.activeSelf) return;

        TMP_Text submitUserTimeText = RankManager.GetComponent<RankManager>().submitUserTimeText;
        submitUserTimeText.text = GameManager.Instance.timeText.text;

        TMP_Text submitUserDeathText = RankManager.GetComponent<RankManager>().submitUserDeathText;
        submitUserDeathText.text = GameManager.Instance.DeathCount.ToString();

        submitboardPanel.SetActive(true);
    }

    public void NextStage()
    {
        if(coin[stageIndex] == coinCount)//코인 다모았을 경우
        {
            //소리
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.GetNextStage);
            getNextStage();
        }
        else
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.CantNextStage);
            HealthDown();
        }
    }

    public void getNextStage() 
    {
        //stage 변경
        if (stageIndex < Stages.Length - 1)//
        {
            Debug.Log("StageIndex : "+ stageIndex);
            StageChange(stageIndex, stageIndex + 1);
            stageIndex++;

            PlayerMove.Instance.Revive();

            //UI 텍스트 Stage표시
            UIStage.text = "STAGE " + (stageIndex);
        }
        else//마지막 스테이지 클리어시
        {
            Time.timeScale = 0;//시간 멈추기
            isClear = true;//클리어유무
            
            ShowSubmitboard();
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

    public void StageChange(int to, int from) {
        GameManager.Instance.Stages[to].SetActive(false);
        GameManager.Instance.Stages[from].SetActive(true);
    }

    public void StageReset(int index) {
        Transform tf = GameManager.Instance.Stages[index].transform;

        foreach (Transform child in tf) {
            if (child.gameObject.activeSelf == false) child.gameObject.SetActive(true);
        }
    }

    public void ExitSubmitboard() {
        SceneManager.LoadScene(0);
    }

    void ViewButton()//UIBoard띄우기
    {
        if (RestartBoard.activeSelf) return;

        RestartBoard.SetActive(true);
        BoardOn = true;
    }

    void CloseButton()
    {
        if (!RestartBoard.activeSelf) return;

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

        UIData.Instance.DiedCount = DeathCount;//죽었을시 UIData에 전달
        UIData.Instance.UIPoint = totalPoint;
        UIData.Instance.UIStage = stageIndex;

        ViewButton();//버튼UI보이기
    }

    public void Restart()//버튼을 눌렀을 경우 함수
    {
        DeathCount++;

        stagePoint = 0;
        coinCount = 0;

        //살아있는상태에서 R눌러도 count적용
        UIData.Instance.DiedCount = DeathCount;
        UIData.Instance.UIStage = stageIndex;
        UIData.Instance.UIPoint = totalPoint;//추가
        UIData.Instance.second = second;
        UIData.Instance.minute = minute;
        UIData.Instance.hour = hour;


        SceneManager.LoadScene(PlayStage);
        StageChange(stageIndex, stageIndex);
        StageReset(stageIndex);

        stagePoint = 0;
        DiedCount.text = "X " + (DeathCount);

        Time.timeScale = 1;//시간 멈춘거 풀기
        PlayerMove.Instance.Revive();

        Debug.Log("stagetIndex : " + stageIndex);
        CloseButton();//버튼UI끄기    
    }

    public void Quit()//Quit버튼 눌렀을 경우 함수
    {
        CloseButton();

        Time.timeScale = 1;//시간 멈춘거 풀기
        CanvasUI.Instance.SetAllChildrenExceptFirst(false);
        gameMode = false;
        GameManager.Instance.StageChange(stageIndex, 0);
        PlayerMove.Instance.Revive();
        //불덩이 끄기
        GetComponent<SpawnManager>().enableSpawn = false;
        stageIndex = 0;
        SceneManager.LoadScene(0);
    }

    public void UIDataReset()//데이터 리셋함수
    {   
        //실질적 데이터
        totalPoint = 0;
        stageIndex = 1;
        coinCount = 0;
        DeathCount = 0;
        second = 0;
        minute = 0;
        hour = 0;

        DiedCount.text = "X " + (DeathCount);
        UIStage.text = "STAGE " + (stageIndex);
        
        /*UI초기화
        UIData.Instance.DiedCount = 0;
        UIData.Instance.UIStage = 0;
        UIData.Instance.UIPoint = 0;
        UIData.Instance.second = 0;
        UIData.Instance.minute = 0;
        UIData.Instance.hour = 0;
        */
    }

    IEnumerator waitSec()
    {
        //1초 기다린다.
        Time.timeScale = 0f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
    }
}
