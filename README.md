# GameProject
졸업작품 2D 게임 프로젝트

## 프로젝트 목표
- 게임 개발의 첫 프로젝트로 전반적인 흐름을 알아 가는 것.
- 구상, 스프라이트 디자인, 개발 모두 알고싶었다.

## 장르
2D 횡스크롤 어드벤쳐 도트게임

## 사용 기술
- 유니티
- C#
- 피스켈 (도트 스프라이트 웹프로그램)

## 소스코드
- https://github.com/KhanBe/GameProject/tree/main/Assets/Scripts

## 설명
각종 장애물을 피해 클리어해 나아가는 스테이지 형태의 게임입니다. (슈퍼마리오같은)

## 오브젝트 

|사진|이름|설명|
|:---:|:---:|:---:|
|![image](https://user-images.githubusercontent.com/61501112/153538544-b4edc2a6-a9df-46d8-8d04-8dc15ed75c37.png)|플레이어|캐릭터|
|![image](https://user-images.githubusercontent.com/61501112/153538732-ffac80ad-caa2-4c6e-847d-0f11b9d4072a.png)|땅|지면|
|![image](https://user-images.githubusercontent.com/61501112/153538786-b03c71c1-f921-4749-853a-d5a56f823c5b.png)|슬라임|AI 몬스터|
|![image](https://user-images.githubusercontent.com/61501112/153538849-69d2f92a-f137-4750-b48f-6e0661c4a89c.png)|가시|장애물|
|![image](https://user-images.githubusercontent.com/61501112/153538900-75ff3a56-3b82-4db3-a4e3-a71b89c3ebc8.png)|불덩이|정면에서 온다|
|![image](https://user-images.githubusercontent.com/61501112/153538900-75ff3a56-3b82-4db3-a4e3-a71b89c3ebc8.png)|굴러가는 불덩이|굴러떨어져 온다|
|![image](https://user-images.githubusercontent.com/61501112/153538961-5a083afe-005a-4bde-9314-dd47d06ae966.png)|칼|함정 장애물|
|![image](https://user-images.githubusercontent.com/61501112/153538992-3d0b0d0d-7e52-4344-8c7d-1b3d08068821.png)|동전|다음 스테이지를 갈 KEY 역할|
|![image](https://user-images.githubusercontent.com/61501112/153539051-14de67ef-f6ad-4b11-a302-161ef46fdd44.png)|깃발|스테이지 전환 역할|


## 개선, 보완할 점
- 게임의 난이도를 낮춰본다.
- 게임 내 타임어택 기능이 필요하다. (랭킹 시스템)
- 메인화면을 깔끔하게 구현하는 방법.
- 게임이 친절하지 못해서 난이도가 올라간다. 나는 이점을 원했다.

## 개발 노트

<details>
 <summary>Awake()와 Start()</summary>
 
- Awake()는 항상 오브젝트 생성 시에 호출된다.
- Start()는 게임오브젝트가 활성화되는 첫번째 프레임에 호출된다.
- 만약 게임오브젝트가 비활성화된 씬에서 시작되었다면 오브젝트가 활성화되어 있더라도 Start()는 호출되지 않는다.
- 호출 순서 Awake() -> Start()
 
 ---
 
</details>

<details>
 <summary>Physics Material</summary>
 
- Dynamic Friction : 움직이는 도중 마찰력
- Static Friction : 멈춘상태에서 얼만큼 힘을가해야 움직이는 마찰력
- bounciness : 공의 튀어 오름의 정도
- Friction : 두 물체의 마찰력
- Bounce : 두 물체의 튀어오름
 
 ---
 
</details>

<details>
 <summary>오브젝트 끼리의 충돌 여부 조정</summary>

 - Edit - Project Settings
 
 ---
 
</details>


<details>
 <summary>플레이어가 움직이다가 멈추는 현상 버그</summary>

- 플레이어 Collider형태가 Box -> Capsule로 바꿔준다.
 
 ---
 
</details>

<details>
 <summary>이미지 가져오고 설정할 것</summary>

- pixels PerUnit 
- Filter Mode
- Compression
 
 ---
 
</details>

<details>
 <summary>아틀라스 분리 방법</summary>

- mutiple -> sprite editor -> slice - gride by cell size
 
 ---
 
</details>

<details>
 <summary>도트 간의 충돌 여백 줄이기</summary>

- setting -> default contact offset
 
 ---
 
</details>

<details>
 <summary>rigidbody 2d</summary>

- Linear Drag : 공기 저항, 플레이어 이동에도 관련있음
- Freeze Rotation 체크 해줘야 안넘어짐
 
 ---
 
</details>

<details>
 <summary>Animation 설정</summary>

1.애니메이션 스프라이트 추가
2.Transition으로 연결 (Animator)
3.파라미터 추가
3.5 Transition방향 누르고
4.애니메이션 전활될 때 겹치는구간 삭제 (Inspector)
5.Has Exit Time : 애니메이션 끝날 때까지 상태 유지 (체크 풀기)
6.파라미터 추가
 
---
- Sprite Renderer - Filp x 체크시 뒤돌기 됨
---
- Jump Animation
Jump시 LoopTime 체크 끄기
RayCast : 오브젝트 검색을 위해 Ray를 쏘는 방식
Layer를 새로 만듦
 
 ---
 
</details>

<details>
 <summary>타일 팔레트, 타일맵</summary>

- window -> 2d -> tile palette
- 그림 넣기
- 2d object -> tileMap 생성

- 타일맵의 콜라이더는 Sprite Editor에서 편집
- Sprite Editor -> Custom Physics Shape -> 변경할 스프라이트 클릭 -> Generate클릭후 변경

- Tile Palette의 Active Tilemap 선택을 잘해야함
 
 ---
 
</details>

<details>
 <summary>점수 스테이지 관리</summary>

- create empty -> (rename)gameManager 생성
- 플레이어 스크립트에 public GameManager gameManager; 생성
- 플레이어 인스펙터에 gameManager를 넣음
 
 ---
 
</details>

<details>
 <summary>Sound</summary>

1. 플레이어에 Audio Source 컴포넌트 추가

2. 그리고 플레이어 스크립트에서
 
```
 public AudioClip audioJump;
public AudioClip audioAttack;
public AudioClip audioDamaged;
public AudioClip audioItem;
```
처럼 변수 추가, 객체 생성(GetComponent) 후 각각 자리에

```
audioSource.clip = audioJump;
audioSource.Play();//(마지막에 붙여줘야함)
```
이런식으로 넣어준다
 
3. Play on Awake 체크 풀기
---
 
 - sound가 모두 나오지 않고 오브젝트가 사라질경우 소리도 사라짐
 ---

</details>

<details>
 <summary>효과음 거리감 표현 방법</summary>

1. 기본메인카메라에 오디오리스너 컴포넌트빼준다
2. 캐릭터에 오디오리스너 컴포넌트 삽입
3. audio Source의 Spatial Blend 를 3D로 변경
4. 3D Sound Settings에서 최대최소 거리 설정 (최대10정도)
 
 ---
 
</details>
