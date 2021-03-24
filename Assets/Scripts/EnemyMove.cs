using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

    public int nextMove;
    public int nextJump;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Think();
    }

    void FixedUpdate()
    {
        //움직이기
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //낭떠러지 AI
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down*3f, new Color(0, 1, 0));//(위치, 쏘는방향, 컬러 값)
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 3, LayerMask.GetMask("Platform"));//(위치, 쏘는방향, 거리, 레이어 값)
        if (rayHit.collider == null)
        {
            Turn();
        }

        if (rigid.velocity.y < 0)//낙하중일 때
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));//(위치, 쏘는방향, 컬러 값)
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));//(위치, 쏘는방향, 거리, 레이어 값)
            if (hit.collider != null)
            {
                if (hit.distance < 0.6f)
                    anim.SetInteger("isJumping", 1);//점프 애니메이션
            }
        }
    }

    void Think()//몬스터 방향
    {
        //움직이는 방향 (-1은 왼쪽/1은 오른쪽)
        nextMove = Random.Range(-1, 2);//(최소값,최대값-1)

        //애니메이션 파라미터
        anim.SetInteger("WalkSpeed", nextMove);
        
        //점프
        nextJump = Random.Range(0, 2);
        if (nextJump == 0 )
        {
            rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
            anim.SetInteger("isJumping", nextJump);
        }

        //스프라이트 방향 전환
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;//nextMove가 1이면 flipX 체크

        //재귀
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);//(함수이름,초)주어진 시간이 지단뒤 지정함수를 실행하는 함수
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;//nextMove가 1이면 flipX 체크
        CancelInvoke();//invoke함수를 취소하는 함수
        Invoke("Think", 2);
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 3.5f, ForceMode2D.Impulse);//Vector2 단일벡터

        Invoke("DeActive", 3);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
