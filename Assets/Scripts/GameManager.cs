using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;

    public void NextStage()
    {
        stageIndex++;

        totalPoint += stagePoint;
        stagePoint = 0;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            if(health > 1)
            {
                collision.attachedRigidbody.velocity = Vector2.zero;//낙하속도 0
                collision.transform.position = new Vector3(-7, 3, -1);
            }
            HealthDown();
        }

    }
    public void HealthDown()
    {
        if (health > 1) health--;

        else
        {
            player.OnDie();

            Debug.Log("죽었다");
        }
    }
}
