using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
    }
    void FixedUpdate()
    {
        
    }
    void OnBecameInvisible()//객체가 화면밖에 안보일때 호출되는 함수
    {
        Destroy(gameObject);
    }
}
