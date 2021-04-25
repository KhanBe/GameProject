using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawnManager : MonoBehaviour
{
    public bool enableSpawn = false;
    public GameObject Knife;
    Transform Transform;

    void Awake()
    {
        Transform = GetComponent<Transform>();
    }
    void Start()
    {
        InvokeRepeating("SpawnKnife", 1, 2);
    }


    void SpawnKnife()
    {

        if (enableSpawn)
        {
            Instantiate(Knife, new Vector3(Transform.position.x, Transform.position.y, 0), Quaternion.identity);//복제함수
        }
    }
}
