using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartButton;

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
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
            UIStage.text = "STAGE " + (stageIndex+1);
        }
        else//마지막 스테이지 클리어시
        {
            Time.timeScale = 0;//시간 멈추기
      
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

    private void OnTriggerEnter2D(Collider2D collision)//낙하 충돌체
    {
        if (collision.gameObject.tag == "Player")
        {   
            if(health > 1)
            {
                PlayerReposition();
            }
            HealthDown();
        }

    }
    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            UIHealth[0].color = new Color(1, 0, 0, 0.4f);

            player.OnDie();//죽음

            ViewButton();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;//시간 멈춘거 풀기
    }
}
