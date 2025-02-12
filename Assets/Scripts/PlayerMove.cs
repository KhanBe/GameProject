﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{       
    public GameManager gameManager;
    
    public float maxSpeed;
    public float curXSpeed;
    public float JumpPower;
    public float positionX;
    public float positionY;
    bool isAlive = true;
    
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;

    private static PlayerMove instance;
    public static PlayerMove Instance {
        get {
            if (instance == null)
            {
                Debug.LogError("PlayerMove Singleton instance가 초기화되지 않았습니다!");
            }
            return instance;
        }
    }

    void Awake()//초기화
    {
        Debug.Log("PlayerMove Awake 호출됨");
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    
    void Update()
    {
        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && isAlive)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);//피격시 점프력상승 방지
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            //sound
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Jump);
            AudioManager.Instance.EffectBgm(true);
        }

        if (Input.GetButtonUp("Horizontal"))
        {//키 뗄 때
            //rigid.velocity.normalized 단위벡터값을 1로 만듦
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);//속도 줄이기
        }

        //방향 전환
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    void FixedUpdate()//초당 5~60번 돈다
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        curXSpeed = rigid.velocity.x;
        //rigid.velocity rigid의 현재 속력
        if (rigid.velocity.x > maxSpeed)//오른쪽 최대속도 설정
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed)//왼쪽 최대속도 설정
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        //레이 캐스트
        if (rigid.velocity.y < 0)//낙하중일 때
        {
            float playerDirection = spriteRenderer.flipX ? -1 : 1;
            Vector2 frontVec = new Vector2(rigid.position.x + playerDirection *0.3f, rigid.position.y);
            
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));//정면 ray
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));// 중앙 ray

            RaycastHit2D rayHit1 = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            RaycastHit2D rayHit2 = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform")); //(위치, 쏘는방향, 거리, 레이어 값) 1.이미 PlayerCollider와 ray가 충돌했음
                                                                                                                //2.방지하기위해 LayerMask쓴다 LayerMask는 Platform레이어만 인식하겠다
            if (rayHit2.collider != null || rayHit1.collider != null)//충돌시 충돌체가 null값이 아님, 그러므로 (충돌체가 null이 아니면=충돌체가 있으면)
            {
                if (rayHit2.distance < 0.6f || rayHit1.distance < 0.6f) //distance ray에 닿았을 때의 거리
                    anim.SetBool("isJumping", false);
            }
        }

        positionX = transform.position.x;//플레이어의 좌표를 스폰매니저에 넘기기
        positionY = transform.position.y;

    }

    void OnCollisionEnter2D(Collision2D collision)//collision = 충돌체
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //밟히는조건
            if( transform.position.y > collision.transform.position.y+0.5f)
            {
                OnAttack(collision.transform);
            }
            else
                OnDamaged(collision.transform.position.x);//충돌체x좌표 파라미터
        }
        else if (collision.gameObject.tag == "Spike"|| collision.gameObject.tag == "Fire")
        {
            OnDamaged(collision.transform.position.x);//충돌체x좌표 파라미터
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag  == "Coin")
        {
            //UI
            gameManager.stagePoint += 100;
            gameManager.coinCount++;

            //GetCoin
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.GetCoin);

            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //스테이지 넘기기
            gameManager.NextStage();
        }
        else if (collision.gameObject.name == "Trap1")
        {
            collision.gameObject.SetActive(false);
        }
    }

    void OnAttack(Transform enemy)//적이 밟혔을 때
    {
        //점수
        gameManager.stagePoint += 100;

        //밟았을 때 튀어오름
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //sound
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.KillMonster);

        //적의 행동
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(float x)
    {
        gameManager.HealthDown();

        gameObject.layer = 11;//레이어 바꾸기

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);//(r,g,b,알파(투명도))

        //튕기는 힘
        float direction = transform.position.x - x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(direction, 1) * 7, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 1);
        //sound
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;//기본 레이어
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        isAlive = false;//죽었음을 표시

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 3.5f, ForceMode2D.Impulse);

        //sound
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
    }

    public void  PlayerReposition()
    {
        transform.position = new Vector3(0, 0, -1);
        rigid.velocity = Vector2.zero;
    }

    public void Revive() {
        isAlive = true;
        spriteRenderer.flipY = false;
        capsuleCollider.enabled = true;
        PlayerReposition();
    }

}
