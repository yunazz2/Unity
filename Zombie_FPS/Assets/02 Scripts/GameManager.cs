using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� �������� GameManager �ν��Ͻ� �ۼ� : �ٸ� ��ũ��Ʈ���� ������ ������ �̷��� �ۼ� ��
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    } // �ٵ� �̷��� �� �� �� ���δ� ������ �� �ν��Ͻ��� ���� ���� �޾ư����ϴµ� �ȿ� ���� �ٲ����� ���ϵ��� ��ȣ�ϱ� ����

    public bool isGameOver = false;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        
    }
}
