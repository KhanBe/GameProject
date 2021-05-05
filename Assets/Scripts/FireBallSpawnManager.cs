using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawnManager : MonoBehaviour
{
    public bool enableSpawn = false;
    public GameObject FireBall;
    Transform Transform;

    void Awake()
    {
        Transform = GetComponent<Transform>();
    }
    void Start()
    {
        InvokeRepeating("SpawnKnife", 0, 3.7f);
    }

    void SpawnKnife()
    {       
        if (enableSpawn)
        {
            Instantiate(FireBall, new Vector3(Transform.position.x, Transform.position.y, 0), Quaternion.identity);//복제함수
        }
    }
}
