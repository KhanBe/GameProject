using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn = false;
    public GameObject FireBall;
    public PlayerMove player;

    void Start()
    {
        InvokeRepeating("SpawnFireBall", 1, 3.5f);
    }

    void SpawnFireBall()
    {
        float PlayerPositionX = player.positionX;//플레이어 포지션 가져오기
        float PlayerPositionY = player.positionY;
        
        if (enableSpawn)
        {
            Instantiate(FireBall, new Vector3(PlayerPositionX+9, PlayerPositionY, 0), Quaternion.identity);//복제함수
        }
    }
}
