using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // * 인스펙터 창에서 접근 가능한 public 멤버 변수 - 카멜 케이스
    public float turnSpeed = 20f;
    
    // * 멤버 변수(특정 메소드가 아닌 클래스에 속한 변수) - 파스칼 케이스
    Vector3 m_Movement;                             // 이동 벡터
    Quaternion m_Rotation = Quaternion.identity;    // 회전 값

    Animator m_Animator;
    Rigidbody m_Rigidbody;


    // Start는 첫 프레임 업데이트 이전에 호출
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Update 메소드 안의 로컬 변수
        float horizontal = Input.GetAxis("Horizontal"); // 수평축 값을 찾아서 horizontal이라는 변수에 저장하겠다.
        float vertical = Input.GetAxis("Vertical");     // 수직축 값을 찾아서 vertical이라는 변수에 저장하겠다.

        m_Movement.Set(horizontal, 0f, vertical);       // Vector의 x축은 수평 입력 값, y축은 0, z축은 수직 입력 값이 들어간다.
        m_Movement.Normalize();

        // * Approximately : 두 개의 실수 값이 유사하면 true를 반환한다.
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // 수평 축으로 이동 값이 0이 아니라면 변수에 저장
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);     // 수직 축으로 이동 값이 0이 아니라면 변수에 저장

        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        // * RotateTowards(현재 회전 값, 목표 회전 값, );
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        // * LookRotation : 해당 파라미터 방향으로 바라보는 회전 생성
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        // * deltaPosition : 루트 모션으로 인한 프레임당 위치의 이동량
        // * MovePosition : 게임 오브젝트를 새로운 위치로 이동 시킨다.
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
