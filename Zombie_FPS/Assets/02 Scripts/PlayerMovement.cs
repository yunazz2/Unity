using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ���� �Է¿� ���� �̵� �� ȸ���� ������ ��ũ��Ʈ
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public float moveSpeed = 5.0f;      // �̵� �ӵ�
    public float rotateSpeed = 180.0f;  // ȸ�� �ӵ�

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // ���� ���� �ֱ⿡ ���缭 �����ϴ� ������Ʈ
    void FixedUpdate()
    {
        Rotate();
        Move();

        // �÷��̾� �ִϸ��̼� ���� Ʈ���� �Ķ���� ���� Move�� move ���� �־��ش�.
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // �÷��̾ �̵���ų �޼ҵ�
    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // �÷��̾ ȸ����ų �޼ҵ�
    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);

        
    }

}
