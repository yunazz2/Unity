using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // �÷��̾ �̵��� �ӷ�
    public float speed = 5;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");  // ���� �Է� �� - �� : -1, �� : 1
        float v = Input.GetAxis("Vertical");    // ���� �Է� �� - �� : 1, �� : -1

        Vector3 dir = Vector3.right * h + Vector3.up * v;

        transform.Translate(dir * speed * Time.deltaTime);
    }
}
