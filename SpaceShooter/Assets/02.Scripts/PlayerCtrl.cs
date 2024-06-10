using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    // 컴포넌트 캐시를 처리할 변수
    private Transform tr;
    // Animation 컴포넌트를 저장할 변수
    private Animation anim;

    // 이동 속도 변수
    public float moveSpeed = 10.0f;
    // 회전 속도 변수
    public float turnSpeed = 80.0f;

    // 초기 생명 값
    private readonly float initHp = 100.0f;
    // 현재 생명 값
    public float currHp;
    // Hpbar를 연결할 변수
    private Image hpBar;

    // 델리게이트 선언
    public delegate void PlayerDieHandler();
    // 이벤트 선언
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        // Hpbar 연결
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        // HP 초기화
        currHp = initHp;

        // 컴포넌트를 추출해 변수에 대입
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        // 애니메이션 실행
        anim.Play("Idle");

        // 게임을 시작할 때마다 주인공 캐릭터가 바라보는 방향을 일정하게 만듦
        turnSpeed = 0.0f;   // 회전 속도
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");  // 좌우
        float v = Input.GetAxis("Vertical");    // 전후
        float r = Input.GetAxis("Mouse X");     // 마우스 수평 방향에 따라 -1 또는 1

        // 전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right* h);

        // Translate 함수를 사용한 이동 로직
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);

        // Rotate 함수를 이용한 회전 로직
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // 주인공 캐릭터의 애니메이션 설정
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        // 키보드 입력 값을 기준으로 동작할 애니메이션 수행
        // * CrossFade("변경할 애니메이션 클립명", 다른 애니메이션으로 페이드 아웃되는 시간);
        if(v >= 1.0f)
        {
            anim.CrossFade("RunF", 0.25f);  // 전진 애니메이션 실행
        }
        else if (v <= -1.0f)
        {
            anim.CrossFade("RunB", 0.25f);  // 후진 애니메이션 실행
        }
        else if(h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);  // 오른쪽 이동 애니메이션 실행
        }
        else if(h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);  // 왼쪽 이동 애니메이션 실행
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);  // 정지 시 Idle 애니메이션 실행
        }
    }

    // 충돌한 Collider의 IsTrigger 옵션이 체크됐을 때 발생
    private void OnTriggerEnter(Collider collider)
    {
        // 충돌한 Collider가 몬스터의 PUNCH이면 플레이어의 HP 차감
        if(currHp >= 0.0f && collider.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            DisplayHealth();

            Debug.Log($"Player HP = {currHp / initHp}");

            // 플레이어의 생명이 0 이하이면 사망 처리
            if(currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    // 플레이어의 사망 처리
    void PlayerDie()
    {
        Debug.Log("플레이어 사망!");

        //// Monster 태그를 가진 모든 게임 오브젝트를 찾아 옴
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        //// 모든 몬스터의 OnPlayerDie 함수를 순차적으로 호출
        //foreach (GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        // 플레이어 사망 이벤트 호출(발생)
        OnPlayerDie();

        // GameManager 스크립트의 IsGameOver 프로퍼티 값을 변경
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;
    }

    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}