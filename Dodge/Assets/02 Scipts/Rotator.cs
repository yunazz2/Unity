using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);   // �̵��̳� ȸ���� ���� Time.deltaTime�� ������ �����ش�~! : �÷��̾�� ��ǻ�� ����� �ٸ��� ������ ������ ������ ������ ������ �� �� ������ ���� �����ַ��� ����.
    }
}
