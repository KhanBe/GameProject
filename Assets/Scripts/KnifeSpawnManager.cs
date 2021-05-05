using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawnManager : MonoBehaviour
{
    public bool enableSpawn = false;
    public GameObject Knife;
    Transform Transform;
    public PlayerMove player;

    void Awake()
    {
        Transform = GetComponent<Transform>();
    }
    void Start()
    {
        //InvokeRepeating("SpawnKnife", 1, 2);
    }
    void FixedUpdate()
    {
        float PlayerPositionX = player.positionX;

        if( Transform.position.y <= player.positionY &&Transform.position.x-0.5f <= player.positionX && player.positionX<= Transform.position.x + 0.5f&& enableSpawn == true)
        {
            Instantiate(Knife, new Vector3(Transform.position.x, Transform.position.y, 0), Quaternion.identity);
            enableSpawn = false;
            Invoke("OnEnable", 1);
        }
            
    }

    void OnEnable()
    {
        enableSpawn = true;
    }
    /*void SpawnKnife()
    {
        
        if (enableSpawn)
        {
            Instantiate(Knife, new Vector3(Transform.position.x, Transform.position.y, 0), Quaternion.identity);//복제함수
        }
    }*/
}
