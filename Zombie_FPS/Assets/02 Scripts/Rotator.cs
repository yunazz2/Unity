using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ������ ������ ��̾����ϱ� ���ۺ��� ���ư��� ȿ���� �ֱ� ���� ��ũ��Ʈ
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60.0f;

    void Update()
    {
        transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);    
    }
}
