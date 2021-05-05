using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    public AudioClip audioKnife;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        //sound
        audioSource.clip = audioKnife;
        audioSource.Play();
    }

    void OnBecameInvisible()//객체가 화면밖에 안보일때 호출되는 함수
    {
        Destroy(gameObject);
    }
}
