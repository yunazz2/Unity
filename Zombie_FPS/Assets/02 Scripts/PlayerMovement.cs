using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 받은 입력에 따라 이동 및 회전을 실행할 스크립트
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public float moveSpeed = 5.0f;      // 이동 속도
    public float rotateSpeed = 180.0f;  // 회전 속도

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // 물리 갱신 주기에 맞춰서 실행하는 업데이트
    void FixedUpdate()
    {
        Rotate();
        Move();

        // 플레이어 애니메이션 블렌드 트리의 파라미터 값인 Move에 move 값을 넣어준다.
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // 플레이어를 이동시킬 메소드
    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 플레이어를 회전시킬 메소드
    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);

        
    }

}
