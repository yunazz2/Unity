using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ʈ�� ���� ������ ���� ��ũ��Ʈ
public class NonSleepMode : MonoBehaviour
{
    void Start()
    {
        // �� ���� �߿��� ���� ���� ��ȯ���� �ʵ���
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        
    }
}
