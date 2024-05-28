using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // 컴포넌트를 추출해 변수에 대입
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        // 애니메이션 실행
        anim.Play("Idle");
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
}