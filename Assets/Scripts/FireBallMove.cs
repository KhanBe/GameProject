using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMove : MonoBehaviour
{
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    public float Speed;
    public PlayerMove player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(Speed, rigid.velocity.y);
    }
    void OnBecameInvisible()//객체가 화면밖에 안보일때 호출되는 함수
    {
        Destroy(gameObject);
    }
}
