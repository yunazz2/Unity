using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // ������Ʈ ĳ�ø� ó���� ����
    private Transform tr;
    // Animation ������Ʈ�� ������ ����
    private Animation anim;

    // �̵� �ӵ� ����
    public float moveSpeed = 10.0f;
    // ȸ�� �ӵ� ����
    public float turnSpeed = 80.0f;

    void Start()
    {
        // ������Ʈ�� ������ ������ ����
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        // �ִϸ��̼� ����
        anim.Play("Idle");
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");  // �¿�
        float v = Input.GetAxis("Vertical");    // ����
        float r = Input.GetAxis("Mouse X");     // ���콺 ���� ���⿡ ���� -1 �Ǵ� 1

        // �����¿� �̵� ���� ���� ���
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right* h);

        // Translate �Լ��� ����� �̵� ����
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);

        // Rotate �Լ��� �̿��� ȸ�� ����
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // ���ΰ� ĳ������ �ִϸ��̼� ����
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        // Ű���� �Է� ���� �������� ������ �ִϸ��̼� ����
        // * CrossFade("������ �ִϸ��̼� Ŭ����", �ٸ� �ִϸ��̼����� ���̵� �ƿ��Ǵ� �ð�);
        if(v >= 1.0f)
        {
            anim.CrossFade("RunF", 0.25f);  // ���� �ִϸ��̼� ����
        }
        else if (v <= -1.0f)
        {
            anim.CrossFade("RunB", 0.25f);  // ���� �ִϸ��̼� ����
        }
        else if(h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);  // ������ �̵� �ִϸ��̼� ����
        }
        else if(h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);  // ���� �̵� �ִϸ��̼� ����
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);  // ���� �� Idle �ִϸ��̼� ����
        }
    }
}