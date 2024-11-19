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

    void FixedUpdate()
    {
        float PlayerPositionX = PlayerMove.Instance.positionX;

        if( Transform.position.y <= PlayerMove.Instance.positionY &&
            Transform.position.x - 0.5f <= PlayerMove.Instance.positionX &&
            PlayerMove.Instance.positionX<= Transform.position.x + 0.5f &&
            enableSpawn == true)
        {
            Instantiate(Knife, new Vector3(Transform.position.x, Transform.position.y, 0), Quaternion.identity);
            enableSpawn = false;
            Invoke("OnEnable", 1.5f);
        }
            
    }

    void OnEnable()
    {
        enableSpawn = true;
    }
}
