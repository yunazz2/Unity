using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // * �ν����� â���� ���� ������ public ��� ���� - ī�� ���̽�
    public float turnSpeed = 20f;
    
    // * ��� ����(Ư�� �޼ҵ尡 �ƴ� Ŭ������ ���� ����) - �Ľ�Į ���̽�
    Vector3 m_Movement;                             // �̵� ����
    Quaternion m_Rotation = Quaternion.identity;    // ȸ�� ��

    Animator m_Animator;
    Rigidbody m_Rigidbody;


    // Start�� ù ������ ������Ʈ ������ ȣ��
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Update �޼ҵ� ���� ���� ����
        float horizontal = Input.GetAxis("Horizontal"); // ������ ���� ã�Ƽ� horizontal�̶�� ������ �����ϰڴ�.
        float vertical = Input.GetAxis("Vertical");     // ������ ���� ã�Ƽ� vertical�̶�� ������ �����ϰڴ�.

        m_Movement.Set(horizontal, 0f, vertical);       // Vector�� x���� ���� �Է� ��, y���� 0, z���� ���� �Է� ���� ����.
        m_Movement.Normalize();

        // * Approximately : �� ���� �Ǽ� ���� �����ϸ� true�� ��ȯ�Ѵ�.
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // ���� ������ �̵� ���� 0�� �ƴ϶�� ������ ����
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);     // ���� ������ �̵� ���� 0�� �ƴ϶�� ������ ����

        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        // * RotateTowards(���� ȸ�� ��, ��ǥ ȸ�� ��, );
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        // * LookRotation : �ش� �Ķ���� �������� �ٶ󺸴� ȸ�� ����
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        // * deltaPosition : ��Ʈ ������� ���� �����Ӵ� ��ġ�� �̵���
        // * MovePosition : ���� ������Ʈ�� ���ο� ��ġ�� �̵� ��Ų��.
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
