using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    public GameManager gameManager;
    public float maxSpeed;
    public float JumpPower;
    public float positionX;
    public float positionY;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;

    void Awake()//초기화
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);//피격시 점프력상승 방지
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            //sound
            PlaySound("JUMP");
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
        if(collision.gameObject.tag  == "Item")
        {
            //점수
            gameManager.stagePoint += 100;
 
            //코인 행동
            CoinSound coinSound = collision.GetComponent<CoinSound>();
            coinSound.getSound();
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //스테이지 넘기기
            gameManager.NextStage();
            //sound
            PlaySound("FINISH");
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
        PlaySound("ATTACK");

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
        rigid.AddForce(new Vector2(direction, 1)*7, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 1);
        //sound
        PlaySound("DAMAGED");
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 3.5f, ForceMode2D.Impulse);
        //sound
        Invoke("stop", 1);
        PlaySound("DIE");
    }
    void stop()
    {
        Time.timeScale = 0;//시간 멈추기
    }
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }
}
